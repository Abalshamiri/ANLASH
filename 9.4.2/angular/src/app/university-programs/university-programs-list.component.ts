import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
    UniversityProgramServiceProxy,
    UniversityProgramDto,
    UniversityServiceProxy,
    UniversityDto,
    UniversityProgramDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ProgramFormComponent } from './program-form.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
    selector: 'app-university-programs-list',
    templateUrl: './university-programs-list.component.html',
    styleUrls: ['./university-programs-list.component.css'],
    animations: [appModuleAnimation()]
})
export class UniversityProgramsListComponent extends AppComponentBase implements OnInit {

    allPrograms: UniversityProgramDto[] = [];
    programs: UniversityProgramDto[] = [];
    universities: UniversityDto[] = [];
    bsModalRef: BsModalRef;

    // Filters
    universityFilter: number | undefined;
    levelFilter: number | undefined;
    modeFilter: number | undefined;
    searchText = '';
    isActiveFilter: boolean | undefined = true;

    // Pagination
    pageNumber = 1;
    pageSize = 10;
    totalCount = 0;
    isTableLoading = false;

    // Sorting
    sorting = 'name ASC';

    // Enums for dropdowns
    programLevels = [
        { value: 1, label: 'Foundation | التأسيسي' },
        { value: 2, label: 'Bachelor | بكالوريوس' },
        { value: 3, label: 'Master | ماجستير' },
        { value: 4, label: 'PhD | دكتوراه' }
    ];

    studyModes = [
        { value: 1, label: 'Full Time | دوام كامل' },
        { value: 2, label: 'Part Time | دوام جزئي' },
        { value: 3, label: 'Online | عن بعد' }
    ];

    constructor(
        injector: Injector,
        private _programService: UniversityProgramServiceProxy,
        private _universityService: UniversityServiceProxy,
        private _modalService: BsModalService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.loadUniversities();
        this.loadPrograms();
    }

    loadUniversities(): void {
        this._universityService
            .getAll(
                undefined,  // searchTerm
                undefined,  // country
                undefined,  // city
                undefined,  // type
                undefined,  // isFeatured
                undefined,  // isActive
                undefined,  // minRating
                undefined,  // orderBy
                undefined,  // isDescending
                0,          // skipCount
                1000        // maxResultCount
            )
            .subscribe(result => {
                this.universities = result.items;
            });
    }

    loadPrograms(): void {
        this.isTableLoading = true;

        this._programService
            .getAll(
                this.sorting,
                0,
                1000  // Load all for client-side filtering
            )
            .pipe(finalize(() => { this.isTableLoading = false; }))
            .subscribe((result: UniversityProgramDtoPagedResultDto) => {
                this.allPrograms = result.items;
                this.applyFilters();
            });
    }

    applyFilters(): void {
        let filtered = [...this.allPrograms];

        // Search filter
        if (this.searchText) {
            const search = this.searchText.toLowerCase();
            filtered = filtered.filter(p =>
                p.name?.toLowerCase().includes(search) ||
                p.nameAr?.toLowerCase().includes(search) ||
                p.field?.toLowerCase().includes(search)
            );
        }

        // University filter
        if (this.universityFilter) {
            filtered = filtered.filter(p => p.universityId === this.universityFilter);
        }

        // Level filter
        if (this.levelFilter) {
            filtered = filtered.filter(p => p.level === this.levelFilter);
        }

        // Mode filter
        if (this.modeFilter) {
            filtered = filtered.filter(p => p.mode === this.modeFilter);
        }

        // Active filter
        if (this.isActiveFilter !== undefined) {
            filtered = filtered.filter(p => p.isActive === this.isActiveFilter);
        }

        this.totalCount = filtered.length;

        // Pagination
        const start = (this.pageNumber - 1) * this.pageSize;
        const end = start + this.pageSize;
        this.programs = filtered.slice(start, end);
    }

    search(): void {
        this.pageNumber = 1;
        this.applyFilters();
    }

    clearFilters(): void {
        this.searchText = '';
        this.universityFilter = undefined;
        this.levelFilter = undefined;
        this.modeFilter = undefined;
        this.isActiveFilter = true;
        this.pageNumber = 1;
        this.applyFilters();
    }

    pageChanged(event: any): void {
        this.pageNumber = event.page;
        this.applyFilters();
    }

    createProgram(): void {
        const initialState = {};

        this.bsModalRef = this._modalService.show(ProgramFormComponent, {
            initialState,
            class: 'modal-lg'
        });

        this.bsModalRef.content.onSave.subscribe(() => {
            this.loadPrograms();
        });
    }

    editProgram(program: UniversityProgramDto): void {
        const initialState = {
            programId: program.id
        };

        this.bsModalRef = this._modalService.show(ProgramFormComponent, {
            initialState,
            class: 'modal-lg'
        });

        this.bsModalRef.content.onSave.subscribe(() => {
            this.loadPrograms();
        });
    }

    deleteProgram(program: UniversityProgramDto): void {
        abp.message.confirm(
            this.l('ProgramDeleteWarningMessage', program.name),
            this.l('AreYouSure'),
            (result: boolean) => {
                if (result) {
                    this._programService
                        .delete(program.id)
                        .subscribe(() => {
                            abp.notify.success(this.l('SuccessfullyDeleted'));
                            this.loadPrograms();
                        });
                }
            }
        );
    }

    toggleFeatured(program: UniversityProgramDto): void {
        this._programService
            .toggleFeatured(program.id)
            .subscribe(() => {
                const message = program.isFeatured
                    ? this.l('RemovedFromFeatured')
                    : this.l('AddedToFeatured');
                abp.notify.success(message);
                this.loadPrograms();
            });
    }

    getLevelLabel(level: number): string {
        const found = this.programLevels.find(l => l.value === level);
        return found ? found.label : '';
    }

    getModeLabel(mode: number): string {
        const found = this.studyModes.find(m => m.value === mode);
        return found ? found.label : '';
    }
}
