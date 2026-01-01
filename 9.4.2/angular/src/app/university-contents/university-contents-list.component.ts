import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
    UniversityContentServiceProxy,
    UniversityContentDto,
    UniversityServiceProxy,
    UniversityDto
} from '@shared/service-proxies/service-proxies';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ContentFormComponent } from './content-form.component';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-university-contents-list',
    templateUrl: './university-contents-list.component.html',
    styleUrls: ['./university-contents-list.component.css']
})
export class UniversityContentsListComponent extends AppComponentBase implements OnInit {

    contents: UniversityContentDto[] = [];
    universities: UniversityDto[] = [];
    bsModalRef: BsModalRef;

    // Filters
    universityFilter: number | undefined;
    contentTypeFilter: number | undefined;

    // Tabs for content organization
    activeTab = 'all';

    // Pagination
    pageNumber = 1;
    pageSize = 20;
    totalCount = 0;
    isTableLoading = false;

    // Content Types
    contentTypes = [
        { value: 1, label: 'About | عن الجامعة', icon: 'fa-info-circle' },
        { value: 2, label: 'Admission | القبول', icon: 'fa-user-plus' },
        { value: 3, label: 'Facilities | المرافق', icon: 'fa-building' },
        { value: 4, label: 'Academic | الأكاديمية', icon: 'fa-graduation-cap' },
        { value: 5, label: 'Student Life | حياة الطلاب', icon: 'fa-users' },
        { value: 6, label: 'Research | البحث العلمي', icon: 'fa-flask' },
        { value: 7, label: 'Contact | اتصل بنا', icon: 'fa-phone' }
    ];

    constructor(
        injector: Injector,
        private _contentService: UniversityContentServiceProxy,
        private _universityService: UniversityServiceProxy,
        private _modalService: BsModalService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.loadUniversities();
        this.loadContents();
    }

    loadUniversities(): void {
        this._universityService
            .getAll(
                undefined, undefined, undefined, undefined,
                undefined, true, undefined, undefined,
                undefined, 0, 1000
            )
            .subscribe(result => {
                this.universities = result.items;
            });
    }

    loadContents(): void {
        this.isTableLoading = true;

        this._contentService
            .getAll(
                'contentType ASC',
                (this.pageNumber - 1) * this.pageSize,
                this.pageSize
            )
            .pipe(finalize(() => { this.isTableLoading = false; }))
            .subscribe(result => {
                this.contents = result.items;
                this.totalCount = result.totalCount;
            });
    }

    filterByType(type: number | undefined): void {
        this.contentTypeFilter = type;
        this.pageNumber = 1;
        this.loadContents();
    }

    createContent(): void {
        this.bsModalRef = this._modalService.show(ContentFormComponent, {
            initialState: {},
            class: 'modal-xl'
        });

        this.bsModalRef.content.onSave.subscribe(() => {
            this.loadContents();
        });
    }

    editContent(content: UniversityContentDto): void {
        this.bsModalRef = this._modalService.show(ContentFormComponent, {
            initialState: { contentId: content.id },
            class: 'modal-xl'
        });

        this.bsModalRef.content.onSave.subscribe(() => {
            this.loadContents();
        });
    }

    deleteContent(content: UniversityContentDto): void {
        abp.message.confirm(
            this.l('ContentDeleteWarningMessage', this.getContentTypeLabel(content.contentType)),
            this.l('AreYouSure'),
            (result: boolean) => {
                if (result) {
                    this._contentService
                        .delete(content.id)
                        .subscribe(() => {
                            abp.notify.success(this.l('SuccessfullyDeleted'));
                            this.loadContents();
                        });
                }
            }
        );
    }

    getContentTypeLabel(type: number): string {
        const found = this.contentTypes.find(t => t.value === type);
        return found ? found.label : '';
    }

    getContentTypeIcon(type: number): string {
        const found = this.contentTypes.find(t => t.value === type);
        return found ? found.icon : 'fa-file';
    }

    getUniversityName(universityId: number): string {
        const university = this.universities.find(u => u.id === universityId);
        return university ? university.name : '';
    }

    pageChanged(event: any): void {
        this.pageNumber = event.page;
        this.loadContents();
    }
}
