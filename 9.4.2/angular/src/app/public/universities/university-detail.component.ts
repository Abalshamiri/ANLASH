import { Component, OnInit, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import {
    UniversityServiceProxy,
    UniversityDto
} from '@shared/service-proxies/service-proxies';
import { Title, Meta } from '@angular/platform-browser';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-university-detail',
    templateUrl: './university-detail.component.html',
    styleUrls: ['./university-detail.component.css']
})
export class UniversityDetailComponent extends AppComponentBase implements OnInit {

    university: UniversityDto;
    isLoading = true;
    slug: string;

    // Tab management
    activeTab = 'overview';

    // Filter for programs
    programLevelFilter: number | undefined;
    programFieldFilter: string | undefined;

    constructor(
        injector: Injector,
        private route: ActivatedRoute,
        private _universityService: UniversityServiceProxy,
        private titleService: Title,
        private metaService: Meta
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.slug = params['slug'];
            this.loadUniversity();
        });
    }

    loadUniversity(): void {
        this.isLoading = true;

        // TODO: getBySlug method - temporarily using first university
        this._universityService
            .getAll(undefined, undefined, undefined, undefined, undefined, true, undefined, undefined, undefined, 0, 1)
            .pipe(finalize(() => { this.isLoading = false; }))
            .subscribe(result => {
                this.university = result.items[0];
                this.setSEO();
            });
    }

    setSEO(): void {
        if (this.university) {
            this.titleService.setTitle(`${this.university.name} - ANLASH`);
            this.metaService.updateTag({
                name: 'description',
                content: this.university.about?.substring(0, 160) || `Learn more about ${this.university.name}`
            });
            this.metaService.updateTag({
                property: 'og:title',
                content: this.university.name
            });
            this.metaService.updateTag({
                property: 'og:description',
                content: this.university.about?.substring(0, 200) || ''
            });
            if (this.university.logoUrl) {
                this.metaService.updateTag({
                    property: 'og:image',
                    content: this.university.logoUrl
                });
            }
        }
    }

    setActiveTab(tab: string): void {
        this.activeTab = tab;
    }

    getFilteredPrograms() {
        if (!this.university || !this.university.programs) {
            return [];
        }

        let filtered = [...this.university.programs];

        if (this.programLevelFilter) {
            filtered = filtered.filter(p => p.level === this.programLevelFilter);
        }

        if (this.programFieldFilter) {
            filtered = filtered.filter(p =>
                p.field?.toLowerCase().includes(this.programFieldFilter.toLowerCase())
            );
        }

        return filtered;
    }

    getPublishedFAQs() {
        return this.university?.faqs?.filter(f => f.isPublished) || [];
    }

    getContentByType(type: number) {
        return this.university?.contents?.find(c => c.contentType === type);
    }

    getStarArray(rating: number): number[] {
        return Array(5).fill(0).map((_, i) => i < Math.floor(rating) ? 1 : 0);
    }
}
