import { Component, OnInit, Injector, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppComponentBase } from '@shared/app-component-base';
import {
    UniversityContentServiceProxy,
    CreateUniversityContentDto,
    UpdateUniversityContentDto,
    UniversityServiceProxy,
    UniversityDto
} from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-content-form',
    templateUrl: './content-form.component.html',
    styleUrls: ['./content-form.component.css']
})
export class ContentFormComponent extends AppComponentBase implements OnInit {

    @Output() onSave = new EventEmitter<any>();

    contentForm: FormGroup;
    contentId: number | undefined;
    isEditMode = false;
    saving = false;

    universities: UniversityDto[] = [];

    // Content Types
    contentTypes = [
        { value: 1, label: 'About | عن الجامعة' },
        { value: 2, label: 'Admission | القبول' },
        { value: 3, label: 'Facilities | المرافق' },
        { value: 4, label: 'Academic | الأكاديمية' },
        { value: 5, label: 'Student Life | حياة الطلاب' },
        { value: 6, label: 'Research | البحث العلمي' },
        { value: 7, label: 'Contact | اتصل بنا' }
    ];

    constructor(
        injector: Injector,
        private fb: FormBuilder,
        private _contentService: UniversityContentServiceProxy,
        private _universityService: UniversityServiceProxy,
        public bsModalRef: BsModalRef
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.buildForm();
        this.loadUniversities();

        if (this.contentId) {
            this.isEditMode = true;
            this.loadContent();
        }
    }

    buildForm(): void {
        this.contentForm = this.fb.group({
            universityId: [null, Validators.required],
            contentType: [null, Validators.required],
            title: ['', [Validators.required, Validators.maxLength(200)]],
            titleAr: ['', [Validators.required, Validators.maxLength(200)]],
            content: ['', [Validators.required, Validators.minLength(50)]],
            contentAr: ['', [Validators.required, Validators.minLength(50)]],
            displayOrder: [0],
            isActive: [true]
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

    loadContent(): void {
        this._contentService
            .get(this.contentId)
            .subscribe(content => {
                this.contentForm.patchValue(content);
            });
    }

    save(): void {
        if (this.contentForm.invalid) {
            this.validateAllFormFields(this.contentForm);
            return;
        }

        this.saving = true;

        const formValue = this.contentForm.value;

        if (this.isEditMode) {
            const dto = new UpdateUniversityContentDto();
            Object.assign(dto, formValue);
            dto.id = this.contentId;

            this._contentService
                .update(dto)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.success(this.l('SavedSuccessfully'));
                    this.bsModalRef.hide();
                    this.onSave.emit();
                });
        } else {
            const dto = new CreateUniversityContentDto();
            Object.assign(dto, formValue);

            this._contentService
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
