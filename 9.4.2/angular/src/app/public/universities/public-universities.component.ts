import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { UniversityServiceProxy, UniversityDto } from '@shared/service-proxies/service-proxies';
import { Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { Title, Meta } from '@angular/platform-browser';

@Component({
    selector: 'app-public-universities',
    templateUrl: './public-universities.component.html',
    styleUrls: ['./public-universities.component.css']
})
export class PublicUniversitiesComponent extends AppComponentBase implements OnInit {

    universities: UniversityDto[] = [];

    // Filters
    searchText = '';
    selectedCountry: string | undefined;
    selectedCity: string | undefined;
    selectedType: number | undefined;
    minRating: number | undefined;

    // Pagination
    pageNumber = 1;
    pageSize = 12;
    totalCount = 0;
    isLoading = false;

    // View Mode
    viewMode: 'grid' | 'list' = 'grid';

    // Dropdowns
    countries = ['Saudi Arabia', 'Yemen', 'Egypt', 'Jordan', 'UAE'];
    cities: string[] = [];
    universityTypes = [
        { value: 1, label: 'Government | حكومية' },
        { value: 2, label: 'Private | خاصة' },
        { value: 3, label: 'Nonprofit | غير ربحية' }
    ];

    constructor(
        injector: Injector,
        private _universityService: UniversityServiceProxy,
        private router: Router,
        private titleService: Title,
        private metaService: Meta
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.setSEO();
        this.loadUniversities();
    }

    setSEO(): void {
        this.titleService.setTitle('Universities | الجامعات - ANLASH');
        this.metaService.updateTag({
            name: 'description',
            content: 'Browse and explore universities. Find the best university for your studies.'
        });
        this.metaService.updateTag({
            property: 'og:title',
            content: 'Universities - ANLASH'
        });
        this.metaService.updateTag({
            property: 'og:description',
            content: 'Browse and explore universities'
        });
    }

    loadUniversities(): void {
        this.isLoading = true;

        this._universityService
            .getAll(
                this.searchText || undefined,
                this.selectedCountry,
                this.selectedCity,
                this.selectedType,
                undefined, // isFeatured
                true, // isActive only
                this.minRating,
                undefined, // orderBy
                undefined, // isDescending
                (this.pageNumber - 1) * this.pageSize,
                this.pageSize
            )
            .pipe(finalize(() => { this.isLoading = false; }))
            .subscribe(result => {
                this.universities = result.items;
                this.totalCount = result.totalCount;
            });
    }

    search(): void {
        this.pageNumber = 1;
        this.loadUniversities();
    }

    clearFilters(): void {
        this.searchText = '';
        this.selectedCountry = undefined;
        this.selectedCity = undefined;
        this.selectedType = undefined;
        this.minRating = undefined;
        this.pageNumber = 1;
        this.loadUniversities();
    }

    pageChanged(event: any): void {
        this.pageNumber = event.page;
        this.loadUniversities();
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }

    viewUniversity(university: UniversityDto): void {
        if (university.slug) {
            this.router.navigate(['/universities', university.slug]);
        } else {
            this.router.navigate(['/universities', university.id]);
        }
    }

    setViewMode(mode: 'grid' | 'list'): void {
        this.viewMode = mode;
    }

    getStarArray(rating: number): number[] {
        return Array(5).fill(0).map((_, i) => i < Math.floor(rating) ? 1 : 0);
    }
}
