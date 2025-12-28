import { Component, Injector, OnInit, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalRef } from 'ngx-bootstrap/modal';
import {
    UniversityServiceProxy,
    CreateUniversityDto,
    UpdateUniversityDto,
    UniversityDto
} from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './university-form.component.html'
})
export class UniversityFormComponent extends AppComponentBase implements OnInit {
    saving = false;
    id: number;
    university: any = {};
    isEditMode = false;

    @Output() onSave = new EventEmitter<any>();

    constructor(
        injector: Injector,
        public bsModalRef: BsModalRef,
        private _universityService: UniversityServiceProxy,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
    }

    ngOnInit(): void {
        if (this.id) {
            // Edit mode
            this.isEditMode = true;
            this._universityService.get(this.id).subscribe((result: UniversityDto) => {
                this.university = result;
                this.cdr.detectChanges();
            });
        } else {
            // Create mode
            this.isEditMode = false;
            this.university = new CreateUniversityDto();
            this.university.isActive = true;
            this.university.isFeatured = false;
            this.university.type = 1; // Public
            this.university.rating = 0;
            this.university.displayOrder = 0;
        }
    }

    save(): void {
        this.saving = true;
        this.cdr.detectChanges();

        // Ensure rating is a number
        if (this.university.rating) {
            this.university.rating = Number(this.university.rating);
        }

        const saveObservable = this.isEditMode
            ? this._universityService.update(this.university)
            : this._universityService.create(this.university);

        saveObservable.subscribe(
            () => {
                this.notify.success(this.l('SavedSuccessfully'));
                this.bsModalRef.hide();
                this.onSave.emit();
            },
            (error) => {
                this.saving = false;
                this.cdr.detectChanges();
                console.error('Save error:', error);
            }
        );
    }
}
