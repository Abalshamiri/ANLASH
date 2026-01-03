import { Component, OnInit, Injector, EventEmitter, Output, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppComponentBase } from '@shared/app-component-base';
import {
    UniversityFAQServiceProxy,
    CreateUniversityFAQDto,
    UpdateUniversityFAQDto,
    UniversityServiceProxy,
    UniversityDto
} from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-faq-form',
    templateUrl: './faq-form.component.html',
    styleUrls: ['./faq-form.component.css']
})
export class FaqFormComponent extends AppComponentBase implements OnInit {

    @Output() onSave = new EventEmitter<any>();

    faqForm: FormGroup;
    faqId: number | undefined;
    isEditMode = false;
    saving = false;

    universities: UniversityDto[] = [];

    constructor(
        injector: Injector,
        private fb: FormBuilder,
        private _faqService: UniversityFAQServiceProxy,
        private _universityService: UniversityServiceProxy,
        public bsModalRef: BsModalRef,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.buildForm();

        // Load universities first, then load FAQ if editing
        this.loadUniversities();

        // Use setTimeout to avoid ExpressionChangedAfterItHasBeenCheckedError
        setTimeout(() => {
            if (this.faqId) {
                this.isEditMode = true;
                this.loadFaq();
            }
            this.cdr.detectChanges();
        }, 0);
    }

    buildForm(): void {
        this.faqForm = this.fb.group({
            universityId: [null, Validators.required],
            question: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(500)]],
            questionAr: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(500)]],
            answer: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(2000)]],
            answerAr: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(2000)]],
            displayOrder: [0],
            isPublished: [false]
        });
    }

    loadUniversities(): void {
        this._universityService
            .getAll(
                undefined, // keyword
                undefined, // type
                undefined, // country
                undefined, // city
                undefined, // isAccredited
                true,      // isActive - only active universities
                undefined, // isFeatured
                undefined, // sorting
                undefined, // filter
                0,         // skipCount
                1000       // maxResultCount
            )
            .subscribe(
                result => {
                    this.universities = result.items || [];
                    console.log('Loaded universities:', this.universities.length);
                    this.cdr.detectChanges();
                },
                error => {
                    console.error('Error loading universities:', error);
                    this.notify.error(this.l('ErrorLoadingUniversities'));
                }
            );
    }

    loadFaq(): void {
        this._faqService
            .get(this.faqId)
            .subscribe(faq => {
                this.faqForm.patchValue(faq);
            });
    }

    save(): void {
        if (this.faqForm.invalid) {
            this.validateAllFormFields(this.faqForm);
            return;
        }

        this.saving = true;

        const formValue = this.faqForm.value;

        if (this.isEditMode) {
            const dto = new UpdateUniversityFAQDto();
            Object.assign(dto, formValue);
            dto.id = this.faqId;

            this._faqService
                .update(dto)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(
                    () => {
                        this.notify.success(this.l('SavedSuccessfully'));
                        this.bsModalRef.hide();
                        this.onSave.emit();
                    },
                    (error) => {
                        console.error('Error updating FAQ:', error);
                        this.notify.error(error.message || this.l('ErrorUpdatingFAQ'));
                    }
                );
        } else {
            const dto = new CreateUniversityFAQDto();
            Object.assign(dto, formValue);

            this._faqService
                .create(dto)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(
                    () => {
                        this.notify.success(this.l('SavedSuccessfully'));
                        this.bsModalRef.hide();
                        this.onSave.emit();
                    },
                    (error) => {
                        console.error('Error creating FAQ:', error);
                        this.notify.error(error.message || this.l('ErrorCreatingFAQ'));
                    }
                );
        }
    }

    validateAllFormFields(formGroup: FormGroup): void {
        Object.keys(formGroup.controls).forEach(field => {
            const control = formGroup.get(field);
            control.markAsTouched({ onlySelf: true });
        });
    }

    close(): void {
        this.bsModalRef.hide();
    }
}
