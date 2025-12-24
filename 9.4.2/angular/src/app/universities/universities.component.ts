import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase,
    PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
    UniversityServiceProxy,
    UniversityDto,
    UniversityDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';
import { CreateUniversityDialogComponent } from './create-university/create-university-dialog.component';
import { EditUniversityDialogComponent } from './edit-university/edit-university-dialog.component';
import { PermissionCheckerService } from 'abp-ng2-module';

class PagedUniversitiesRequestDto extends PagedRequestDto {
    searchTerm: string;
    country: string;
    city: string;
    type: number | null;
    isActive: boolean | null;
    isFeatured: boolean | null;
}

@Component({
    templateUrl: './universities.component.html',
    animations: [appModuleAnimation()]
})
export class UniversitiesComponent extends PagedListingComponentBase<UniversityDto> {
    universities: UniversityDto[] = [];
    searchTerm = '';
    country = '';
    city = '';
    type: number | null = null;
    isActive: boolean | null = null;
    isFeatured: boolean | null = null;
    advancedFiltersVisible = false;
    currentLanguage: string;
    permission: PermissionCheckerService;

    constructor(
        injector: Injector,
        private _universityService: UniversityServiceProxy,
        private _modalService: BsModalService,
        cd: ChangeDetectorRef
    ) {
        super(injector, cd);
        this.currentLanguage = abp.localization.currentLanguage.name;
        this.permission = injector.get(PermissionCheckerService);
    }

    isGranted(permissionName: string): boolean {
        return this.permission.isGranted(permissionName);
    }

    getDisplayName(university: UniversityDto): string {
        return this.currentLanguage === 'ar' && university.nameAr
            ? university.nameAr
            : university.name;
    }

    getDisplayDescription(university: UniversityDto): string {
        return this.currentLanguage === 'ar' && university.descriptionAr
            ? university.descriptionAr
            : university.description;
    }

    createUniversity(): void {
        this.showCreateOrEditDialog();
    }

    editUniversity(university: UniversityDto): void {
        this.showCreateOrEditDialog(university.id);
    }

    clearFilters(): void {
        this.searchTerm = '';
        this.country = '';
        this.city = '';
        this.type = null;
        this.isActive = null;
        this.isFeatured = null;
        this.getDataPage(1);
    }

    protected list(
        request: PagedUniversitiesRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {
        request.searchTerm = this.searchTerm;
        request.country = this.country;
        request.city = this.city;
        request.type = this.type;
        request.isActive = this.isActive;
        request.isFeatured = this.isFeatured;

        this._universityService
            .getAll(
                request.searchTerm || undefined,
                request.country || undefined,
                request.city || undefined,
                request.type !== null ? request.type : undefined,
                request.isActive !== null ? request.isActive : undefined,
                request.isFeatured !== null ? request.isFeatured : undefined,
                undefined, // minRating
                undefined, // orderBy
                undefined, // isDescending
                request.skipCount,
                request.maxResultCount
            )
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: UniversityDtoPagedResultDto) => {
                this.universities = result.items;
                this.showPaging(result, pageNumber);
            });
    }

    protected delete(university: UniversityDto): void {
        abp.message.confirm(
            this.l('UniversityDeleteWarningMessage', this.getDisplayName(university)),
            undefined,
            (result: boolean) => {
                if (result) {
                    this._universityService.delete(university.id).subscribe(() => {
                        abp.notify.success(this.l('SuccessfullyDeleted'));
                        this.refresh();
                    });
                }
            }
        );
    }

    private showCreateOrEditDialog(id?: number): void {
        let dialog;
        if (!id) {
            dialog = this._modalService.show(
                CreateUniversityDialogComponent,
                {
                    class: 'modal-lg',
                }
            );
        } else {
            dialog = this._modalService.show(
                EditUniversityDialogComponent,
                {
                    class: 'modal-lg',
                    initialState: {
                        id: id,
                    },
                }
            );
        }

        dialog.content.onSave.subscribe(() => {
            this.refresh();
        });
    }
}
