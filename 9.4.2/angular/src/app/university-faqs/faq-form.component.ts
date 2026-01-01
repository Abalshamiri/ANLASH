import { Component, OnInit, Injector, EventEmitter, Output } from '@angular/core';
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
        public bsModalRef: BsModalRef
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.buildForm();
        this.loadUniversities();

        if (this.faqId) {
            this.isEditMode = true;
            this.loadFaq();
        }
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
                undefined, undefined, undefined, undefined,
                undefined, true, undefined, undefined,
                undefined, 0, 1000
            )
            .subscribe(result => {
                this.universities = result.items;
            });
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
                .subscribe(() => {
                    this.notify.success(this.l('SavedSuccessfully'));
                    this.bsModalRef.hide();
                    this.onSave.emit();
                });
        } else {
            const dto = new CreateUniversityFAQDto();
            Object.assign(dto, formValue);

            this._faqService
                .create(dto)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.success(this.l('SavedSuccessfully'));
                    this.bsModalRef.hide();
                    this.onSave.emit();
                });
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
