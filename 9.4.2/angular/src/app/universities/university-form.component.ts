import { Component, Injector, OnInit, Output, EventEmitter, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalRef } from 'ngx-bootstrap/modal';
import {
    UniversityServiceProxy,
    CreateUniversityDto,
    UpdateUniversityDto,
    UniversityDto,
    BlobStorageServiceProxy,
    UploadFileInput
} from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './university-form.component.html',
    styleUrls: ['./university-form.component.css']
})
export class UniversityFormComponent extends AppComponentBase implements OnInit {
    saving = false;
    id: number;
    university: any = {};
    isEditMode = false;
    activeTab: string = 'basic';

    // Lookups
    countries: any[] = [];
    cities: any[] = [];
    loadingCountries = false;
    loadingCities = false;

    // Logo upload
    selectedLogoFile: File | null = null;
    logoPreview: string | null = null;
    uploadingLogo: boolean = false;
    @ViewChild('logoInput') logoInput: ElementRef;

    // Cover image upload
    selectedCoverFile: File | null = null;
    coverPreview: string | null = null;
    uploadingCover: boolean = false;
    @ViewChild('coverInput') coverInput: ElementRef;

    // University Types
    universityTypes = [
        { value: 1, label: 'Public | حكومية' },
        { value: 2, label: 'Private | خاصة' },
        { value: 3, label: 'NonProfit | غير ربحية' }
    ];

    // Intake Months
    months = [
        { value: 1, label: 'January | يناير' },
        { value: 2, label: 'February | فبراير' },
        { value: 3, label: 'March | مارس' },
        { value: 4, label: 'April | أبريل' },
        { value: 5, label: 'May | مايو' },
        { value: 6, label: 'June | يونيو' },
        { value: 7, label: 'July | يوليو' },
        { value: 8, label: 'August | أغسطس' },
        { value: 9, label: 'September | سبتمبر' },
        { value: 10, label: 'October | أكتوبر' },
        { value: 11, label: 'November | نوفمبر' },
        { value: 12, label: 'December | ديسمبر' }
    ];

    selectedIntakeMonths: number[] = [];

    // Audit info
    creatorName: string = '';
    lastModifierName: string = '';
    deleterName: string = '';

    @Output() onSave = new EventEmitter<any>();

    constructor(
        injector: Injector,
        public bsModalRef: BsModalRef,
        private _universityService: UniversityServiceProxy,
        private _blobStorageService: BlobStorageServiceProxy,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.loadCountries();

        if (this.id) {
            // Edit mode
            this.isEditMode = true;
            this._universityService.get(this.id).subscribe((result: UniversityDto) => {
                this.university = result;

                // Parse intake months
                if (this.university.intakeMonths) {
                    this.selectedIntakeMonths = this.university.intakeMonths.split(',').map(m => parseInt(m));
                }

                // Load cities for selected country
                if (this.university.countryId) {
                    this.loadCities(this.university.countryId);
                }

                // Load logo/cover URLs
                if (this.university.logoBlobId) {
                    this.university.logoUrl = `https://localhost:44311/api/services/app/BlobStorage/Download?id=${this.university.logoBlobId}`;
                }
                if (this.university.coverImageBlobId) {
                    this.university.coverImageUrl = `https://localhost:44311/api/services/app/BlobStorage/Download?id=${this.university.coverImageBlobId}`;
                }

                // Load audit info
                this.loadAuditInfo();

                this.cdr.detectChanges();
            });
        } else {
            // Create mode
            this.isEditMode = false;
            this.university = new CreateUniversityDto();
            this.university.isActive = true;
            this.university.isFeatured = false;
            this.university.type = 1;
            this.university.rating = 0;
            this.university.displayOrder = 0;
        }
    }

    switchTab(tabName: string): void {
        this.activeTab = tabName;
    }

    loadCountries(): void {
        // Manual country list - in production, load from API
        this.countries = [
            { id: 1, name: 'Saudi Arabia', nameAr: 'السعودية' },
            { id: 2, name: 'Egypt', nameAr: 'مصر' },
            { id: 3, name: 'UAE', nameAr: 'الإمارات' },
            { id: 4, name: 'Jordan', nameAr: 'الأردن' },
            { id: 5, name: 'Kuwait', nameAr: 'الكويت' }
        ];
        this.loadingCountries = false;
    }

    onCountryChange(countryId: number): void {
        this.university.cityId = null;
        this.cities = [];
        if (countryId) {
            this.loadCities(countryId);
        }
    }

    loadCities(countryId: number): void {
        // Manual city list - in production, load from API based on country
        this.cities = [
            { id: 1, name: 'Riyadh', nameAr: 'الرياض' },
            { id: 2, name: 'Jeddah', nameAr: 'جدة' },
            { id: 3, name: 'Cairo', nameAr: 'القاهرة' },
            { id: 4, name: 'Dubai', nameAr: 'دبي' }
        ];
        this.loadingCities = false;
    }

    onLogoSelected(event: any): void {
        const file = event.target.files[0];
        if (file) {
            this.selectedLogoFile = file;
            const reader = new FileReader();
            reader.onload = (e: any) => {
                this.logoPreview = e.target.result;
                this.cdr.detectChanges();
            };
            reader.readAsDataURL(file);
        }
    }

    onCoverSelected(event: any): void {
        const file = event.target.files[0];
        if (file) {
            this.selectedCoverFile = file;
            const reader = new FileReader();
            reader.onload = (e: any) => {
                this.coverPreview = e.target.result;
                this.cdr.detectChanges();
            };
            reader.readAsDataURL(file);
        }
    }

    async uploadLogo(): Promise<string | null> {
        if (!this.selectedLogoFile) return null;

        try {
            this.uploadingLogo = true;
            const input = new UploadFileInput();
            input.init({ file: this.selectedLogoFile, fileName: this.selectedLogoFile.name });

            const result = await this._blobStorageService.upload(input).toPromise();
            this.uploadingLogo = false;
            return result.id;
        } catch (error) {
            this.uploadingLogo = false;
            this.notify.error(this.l('UploadFailed'));
            return null;
        }
    }

    async uploadCover(): Promise<string | null> {
        if (!this.selectedCoverFile) return null;

        try {
            this.uploadingCover = true;
            const input = new UploadFileInput();
            input.init({ file: this.selectedCoverFile, fileName: this.selectedCoverFile.name });

            const result = await this._blobStorageService.upload(input).toPromise();
            this.uploadingCover = false;
            return result.id;
        } catch (error) {
            this.uploadingCover = false;
            this.notify.error(this.l('UploadFailed'));
            return null;
        }
    }

    generateSlug(): void {
        if (this.university.name && !this.isEditMode) {
            this.university.slug = this.university.name
                .toLowerCase()
                .replace(/[^a-z0-9]+/g, '-')
                .replace(/^-+|-+$/g, '');
        }
    }

    loadAuditInfo(): void {
        // Load creator/modifier names from user IDs
        // This would require a UserService call
        // For now, placeholder
        this.creatorName = 'Admin User';
        this.lastModifierName = 'Admin User';
    }

    toggleIntakeMonth(monthValue: number): void {
        const index = this.selectedIntakeMonths.indexOf(monthValue);
        if (index > -1) {
            this.selectedIntakeMonths.splice(index, 1);
        } else {
            this.selectedIntakeMonths.push(monthValue);
        }
    }

    async save(): Promise<void> {
        this.saving = true;
        this.cdr.detectChanges();

        // Convert intake months to comma-separated string
        if (this.selectedIntakeMonths && this.selectedIntakeMonths.length > 0) {
            this.university.intakeMonths = this.selectedIntakeMonths.join(',');
        }

        // Upload logo if selected
        if (this.selectedLogoFile) {
            const blobId = await this.uploadLogo();
            if (blobId) {
                this.university.logoBlobId = blobId;
            } else {
                this.saving = false;
                return;
            }
        }

        // Upload cover if selected
        if (this.selectedCoverFile) {
            const blobId = await this.uploadCover();
            if (blobId) {
                this.university.coverImageBlobId = blobId;
            } else {
                this.saving = false;
                return;
            }
        }

        const saveObservable = this.isEditMode
            ? this._universityService.update(this.university)
            : this._universityService.create(this.university);

        saveObservable.subscribe(
            () => {
                this.notify.success(this.l('SavedSuccessfully'));
                this.bsModalRef.hide();
                this.onSave.emit();
                this.saving = false;
            },
            (error) => {
                console.error('Save error:', error);
                this.notify.error(this.l('SaveFailed'));
                this.saving = false;
            }
        );
    }
}
