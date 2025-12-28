import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
    UniversityServiceProxy,
    UniversityDto,
    UniversityDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';
import { UniversityFormComponent } from './university-form.component';

@Component({
    templateUrl: './university-list.component.html',
    animations: [appModuleAnimation()]
})
export class UniversityListComponent extends AppComponentBase implements OnInit {
    universities: UniversityDto[] = [];
    filterText = '';
    sorting = 'name';
    skipCount = 0;
    maxResultCount = 10;
    totalCount = 0;

    constructor(
        injector: Injector,
        private _universityService: UniversityServiceProxy,
        private _modalService: BsModalService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.getUniversities();
    }

    getUniversities(): void {
        this._universityService
            .getAll(
                this.filterText,      // searchTerm
                undefined,             // country
                undefined,             // city
                undefined,             // type
                undefined,             // isFeatured
                undefined,             // isActive
                undefined,             // minRating
                this.sorting,          // orderBy
                false,                 // isDescending
                this.skipCount,        // skipCount
                this.maxResultCount    // maxResultCount
            )
            .subscribe((result: UniversityDtoPagedResultDto) => {
                this.universities = result.items;
                this.totalCount = result.totalCount;
            });
    }

    create(): void {
        this.showFormModal();
    }

    edit(university: UniversityDto): void {
        this.showFormModal(university.id);
    }

    delete(university: UniversityDto): void {
        this.message.confirm(
            this.l('UniversityDeleteWarningMessage', university.name),
            this.l('AreYouSure'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._universityService.delete(university.id).subscribe(() => {
                        this.notify.success(this.l('SuccessfullyDeleted'));
                        this.getUniversities();
                    });
                }
            }
        );
    }

    pageChanged(event: any): void {
        this.skipCount = (event.page - 1) * this.maxResultCount;
        this.getUniversities();
    }

    private showFormModal(id?: number): void {
        let modalRef: BsModalRef;
        modalRef = this._modalService.show(UniversityFormComponent, {
            class: 'modal-lg',
            initialState: { id: id }
        });

        modalRef.content.onSave.subscribe(() => {
            this.getUniversities();
        });
    }
}
