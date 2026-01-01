import { Component, OnInit, Injector, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppComponentBase } from '@shared/app-component-base';
import {
    UniversityProgramServiceProxy,
    CreateUniversityProgramDto,
    UpdateUniversityProgramDto,
    UniversityServiceProxy,
    UniversityDto,
    ProgramLevel,
    StudyMode
} from '@shared/service-proxies/service-proxies';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-program-form',
    templateUrl: './program-form.component.html',
    styleUrls: ['./program-form.component.css']
})
export class ProgramFormComponent extends AppComponentBase implements OnInit {

    @Output() onSave = new EventEmitter<any>();

    programForm: FormGroup;
    programId: number | undefined;
    isEditMode = false;
    saving = false;

    universities: UniversityDto[] = [];

    // Enums for dropdowns
    programLevels = [
        { value: 1, label: this.l('Foundation') + ' | التأسيسي' },
        { value: 2, label: this.l('Bachelor') + ' | بكالوريوس' },
        { value: 3, label: this.l('Master') + ' | ماجستير' },
        { value: 4, label: this.l('PhD') + ' | دكتوراه' }
    ];

    studyModes = [
        { value: 1, label: this.l('FullTime') + ' | دوام كامل' },
        { value: 2, label: this.l('PartTime') + ' | دوام جزئي' },
        { value: 3, label: this.l('Online') + ' | عن بعد' }
    ];

    constructor(
        injector: Injector,
        private fb: FormBuilder,
        private _programService: UniversityProgramServiceProxy,
        private _universityService: UniversityServiceProxy,
        public bsModalRef: BsModalRef
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.buildForm();
        this.loadUniversities();

        if (this.programId) {
            this.isEditMode = true;
            this.loadProgram();
        }
    }

    buildForm(): void {
        this.programForm = this.fb.group({
            universityId: [null, Validators.required],
            name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(300)]],
            nameAr: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(300)]],
            description: ['', Validators.maxLength(2000)],
            descriptionAr: ['', Validators.maxLength(2000)],
            level: [null, Validators.required],
            mode: [null, Validators.required],
            field: ['', Validators.maxLength(200)],
            fieldAr: ['', Validators.maxLength(200)],
            durationYears: [4, [Validators.required, Validators.min(1), Validators.max(10)]],
            durationSemesters: [null],
            durationMonths: [null],
            totalCredits: [null],
            tuitionFee: [null, Validators.min(0)],
            currencyId: [null],
            feeType: [''],
            applicationFee: [null],
            applicationDeadline: [null],
            requirements: [''],
            requirementsAr: [''],
            slug: [''],
            slugAr: [''],
            displayOrder: [0],
            isActive: [true],
            isFeatured: [false]
        });

        // Auto-generate slug on name change
        this.programForm.get('name').valueChanges.subscribe(name => {
            if (name && !this.isEditMode) {
                const slug = this.generateSlug(name);
                this.programForm.patchValue({ slug }, { emitEvent: false });
            }
        });

        this.programForm.get('nameAr').valueChanges.subscribe(nameAr => {
            if (nameAr && !this.isEditMode) {
                const slugAr = this.generateSlug(nameAr);
                this.programForm.patchValue({ slugAr }, { emitEvent: false });
            }
        });
    }

    loadUniversities(): void {
        this._universityService
            .getAll(
                undefined,
                undefined,
                undefined,
                undefined,
                undefined,
                true,  // isActive only
                undefined,
                undefined,
                undefined,
                0,
                1000
            )
            .subscribe(result => {
                this.universities = result.items;
            });
    }

    loadProgram(): void {
        this._programService
            .get(this.programId)
            .subscribe(program => {
                this.programForm.patchValue(program);
            });
    }

    save(): void {
        if (this.programForm.invalid) {
            this.validateAllFormFields(this.programForm);
            return;
        }

        this.saving = true;

        const formValue = this.programForm.value;

        if (this.isEditMode) {
            const dto = new UpdateUniversityProgramDto();
            Object.assign(dto, formValue);
            dto.id = this.programId;

            this._programService
                .update(dto)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.success(this.l('SavedSuccessfully'));
                    this.bsModalRef.hide();
                    this.onSave.emit();
                });
        } else {
            const dto = new CreateUniversityProgramDto();
            Object.assign(dto, formValue);

            this._programService
                .create(dto)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.notify.success(this.l('SavedSuccessfully'));
                    this.bsModalRef.hide();
                    this.onSave.emit();
                });
        }
    }

    generateSlug(text: string): string {
        return text
            .toLowerCase()
            .replace(/[^\w\s-]/g, '')
            .replace(/[\s_-]+/g, '-')
            .replace(/^-+|-+$/g, '');
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
