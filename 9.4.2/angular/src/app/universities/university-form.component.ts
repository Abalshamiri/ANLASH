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
    templateUrl: './university-form.component.html'
})
export class UniversityFormComponent extends AppComponentBase implements OnInit {
    saving = false;
    id: number;
    university: any = {};
    isEditMode = false;
    activeTab: string = 'basic'; // Current active tab

    // Logo upload properties
    selectedLogoFile: File | null = null;
    logoPreview: string | null = null;
    uploadingLogo: boolean = false;
    @ViewChild('logoInput') logoInput: ElementRef;

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
        if (this.id) {
            // Edit mode
            this.isEditMode = true;
            this._universityService.get(this.id).subscribe((result: UniversityDto) => {
                this.university = result;

                // Load logo from BlobStorage if exists
                if (this.university.logoBlobId) {
                    this.university.logoUrl = `${abp.appPath}api/services/app/BlobStorage/Download?id=${this.university.logoBlobId}`;
                }

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

    /**
     * Switch between tabs
     */
    switchTab(tabName: string): void {
        this.activeTab = tabName;
    }

    async save(): Promise<void> {
        this.saving = true;
        this.cdr.detectChanges();

        // Ensure rating is a number
        if (this.university.rating) {
            this.university.rating = Number(this.university.rating);
        }

        // Upload logo if selected
        if (this.selectedLogoFile) {
            const blobId = await this.uploadLogo();
            if (blobId) {
                this.university.logoBlobId = blobId;
                // Clear logoUrl when using blob
                this.university.logoUrl = null;
            } else {
                // Upload failed, stop save
                this.saving = false;
                this.cdr.detectChanges();
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
            },
            (error) => {
                this.saving = false;
                this.cdr.detectChanges();
                console.error('Save error:', error);
            }
        );
    }

    /**
     * Get creator name for audit trail
     */
    getCreatorName(): string {
        if (!this.university.creatorUserId) {
            return this.l('Unknown');
        }
        // For now, show User ID. In future, fetch actual name from UserService
        return `User #${this.university.creatorUserId}`;
    }

    /**
     * Get last modifier name for audit trail
     */
    getLastModifierName(): string {
        if (!this.university.lastModifierUserId) {
            return this.l('Unknown');
        }
        // For now, show User ID. In future, fetch actual name from UserService
        return `User #${this.university.lastModifierUserId}`;
    }

    /**
     * Get deleter name for audit trail
     */
    getDeleterName(): string {
        if (!this.university.deleterUserId) {
            return this.l('Unknown');
        }
        // For now, show User ID. In future, fetch actual name from UserService
        return `User #${this.university.deleterUserId}`;
    }

    /**
     * Handle logo file selection
     */
    onLogoSelected(event: any): void {
        const file = event.target?.files?.[0];
        if (!file) return;

        // Validate file type
        const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
        if (!allowedTypes.includes(file.type)) {
            this.notify.error('Please select a valid image file (JPG, PNG, GIF)');
            if (this.logoInput) {
                this.logoInput.nativeElement.value = '';
            }
            return;
        }

        // Validate file size (2MB max)
        const maxSize = 2 * 1024 * 1024; // 2MB
        if (file.size > maxSize) {
            this.notify.error('File size must be less than 2MB');
            if (this.logoInput) {
                this.logoInput.nativeElement.value = '';
            }
            return;
        }

        // Create preview
        const reader = new FileReader();
        reader.onload = (e: any) => {
            this.logoPreview = e.target.result;
            this.cdr.detectChanges();
        };
        reader.readAsDataURL(file);

        this.selectedLogoFile = file;
    }

    /**
     * Upload logo to BlobStorage
     */
    async uploadLogo(): Promise<string | null> {
        if (!this.selectedLogoFile) return null;

        this.uploadingLogo = true;
        this.cdr.detectChanges();

        try {
            // Convert file to base64
            const fileBytes = await this.fileToBase64(this.selectedLogoFile);

            const fileExt = this.getFileExtension(this.selectedLogoFile.name);
            const fileName = `university-logo-${Date.now()}.${fileExt}`;

            // Create UploadFileInput
            const input = new UploadFileInput({
                fileBytes: fileBytes,
                fileName: fileName,
                contentType: this.selectedLogoFile.type,
                category: 'UniversityLogos',
                description: `Logo for ${this.university.name || 'University'}`,
                entityType: undefined,
                entityId: undefined
            });

            const result = await this._blobStorageService.upload(input).toPromise();

            return result.id; // Return BlobId (Guid as string)
        } catch (error) {
            this.notify.error('Failed to upload logo');
            console.error('Upload error:', error);
            return null;
        } finally {
            this.uploadingLogo = false;
            this.cdr.detectChanges();
        }
    }

    /**
     * Remove logo
     */
    removeLogo(): void {
        this.selectedLogoFile = null;
        this.logoPreview = null;
        this.university.logoBlobId = null;
        this.university.logoUrl = null;
        if (this.logoInput) {
            this.logoInput.nativeElement.value = '';
        }
        this.cdr.detectChanges();
    }

    /**
     * Convert file to base64
     */
    private fileToBase64(file: File): Promise<string> {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = () => {
                // Remove data:image/...;base64, prefix
                let result = reader.result as string;
                const base64 = result.split(',')[1];
                resolve(base64);
            };
            reader.onerror = error => reject(error);
            reader.readAsDataURL(file);
        });
    }

    /**
     * Get file extension
     */
    private getFileExtension(filename: string): string {
        return filename.split('.').pop()?.toLowerCase() || '';
    }
}
