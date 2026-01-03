import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
    UniversityFAQServiceProxy,
    UniversityFAQDto,
    UniversityServiceProxy,
    UniversityDto,
    FAQOrderDto
} from '@shared/service-proxies/service-proxies';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FaqFormComponent } from './faq-form.component';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';

@Component({
    selector: 'app-university-faqs-list',
    templateUrl: './university-faqs-list.component.html',
    styleUrls: ['./university-faqs-list.component.css'],
    animations: [appModuleAnimation()]
})
export class UniversityFaqsListComponent extends AppComponentBase implements OnInit {

    faqs: UniversityFAQDto[] = [];
    universities: UniversityDto[] = [];
    bsModalRef: BsModalRef;

    // Current university ID (from route or filter)
    universityId: number | undefined;

    // Filters
    universityFilter: number | undefined;
    isPublishedFilter: boolean | undefined;
    searchText = '';

    // Pagination
    pageNumber = 1;
    pageSize = 20;
    totalCount = 0;
    isTableLoading = false;

    // Reordering
    isReorderMode = false;
    hasUnsavedOrder = false;

    constructor(
        injector: Injector,
        private _faqService: UniversityFAQServiceProxy,
        private _universityService: UniversityServiceProxy,
        private _modalService: BsModalService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.loadUniversities();
        this.loadFaqs();
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

    loadFaqs(): void {
        this.isTableLoading = true;

        if (!this.universityId) {
            this.isTableLoading = false;
            return;
        }

        this._faqService
            .getByUniversity(this.universityId)
            .pipe(finalize(() => { this.isTableLoading = false; }))
            .subscribe(result => {
                this.faqs = result.items;
                this.totalCount = result.items.length;
                this.hasUnsavedOrder = false;
            });
    }

    search(): void {
        this.pageNumber = 1;
        this.loadFaqs();
    }

    clearFilters(): void {
        this.searchText = '';
        this.universityFilter = undefined;
        this.isPublishedFilter = undefined;
        this.pageNumber = 1;
        this.loadFaqs();
    }

    pageChanged(event: any): void {
        this.pageNumber = event.page;
        this.loadFaqs();
    }

    createFaq(): void {
        this.bsModalRef = this._modalService.show(FaqFormComponent, {
            initialState: {},
            class: 'modal-lg'
        });

        this.bsModalRef.content.onSave.subscribe(() => {
            this.loadFaqs();
        });
    }

    editFaq(faq: UniversityFAQDto): void {
        this.bsModalRef = this._modalService.show(FaqFormComponent, {
            initialState: { faqId: faq.id },
            class: 'modal-lg'
        });

        this.bsModalRef.content.onSave.subscribe(() => {
            this.loadFaqs();
        });
    }

    deleteFaq(faq: UniversityFAQDto): void {
        abp.message.confirm(
            this.l('FAQDeleteWarningMessage', faq.question),
            this.l('AreYouSure'),
            (result: boolean) => {
                if (result) {
                    this._faqService
                        .delete(faq.id)
                        .subscribe(() => {
                            abp.notify.success(this.l('SuccessfullyDeleted'));
                            this.loadFaqs();
                        });
                }
            }
        );
    }

    togglePublish(faq: UniversityFAQDto): void {
        this._faqService
            .togglePublish(faq.id)
            .subscribe(() => {
                abp.notify.success(this.l(faq.isPublished ? 'UnpublishedSuccessfully' : 'PublishedSuccessfully'));
                this.loadFaqs();
            });
    }

    // Reordering functionality
    toggleReorderMode(): void {
        if (this.isReorderMode && this.hasUnsavedOrder) {
            abp.message.confirm(
                this.l('UnsavedOrderChangesWarning'),
                this.l('AreYouSure'),
                (result: boolean) => {
                    if (result) {
                        this.isReorderMode = false;
                        this.hasUnsavedOrder = false;
                        this.loadFaqs();
                    }
                }
            );
        } else {
            this.isReorderMode = !this.isReorderMode;
            if (!this.isReorderMode) {
                this.loadFaqs();
            }
        }
    }


    drop(event: CdkDragDrop<UniversityFAQDto[]>): void {
        moveItemInArray(this.faqs, event.previousIndex, event.currentIndex);
        this.hasUnsavedOrder = true;

        // Update displayOrder for all items
        this.faqs.forEach((faq, index) => {
            faq.displayOrder = index + 1;
        });
    }

    saveOrder(): void {
        const orders: FAQOrderDto[] = this.faqs.map(faq => ({
            id: faq.id,
            displayOrder: faq.displayOrder
        } as FAQOrderDto));

        this._faqService
            .reorder(orders)
            .subscribe(() => {
                abp.notify.success(this.l('OrderSavedSuccessfully'));
                this.hasUnsavedOrder = false;
                this.isReorderMode = false;
                this.loadFaqs();
            });
    }

    cancelReorder(): void {
        this.isReorderMode = false;
        this.hasUnsavedOrder = false;
        this.loadFaqs();
    }

    getUniversityName(universityId: number): string {
        const university = this.universities.find(u => u.id === universityId);
        return university ? university.name : '';
    }
}
