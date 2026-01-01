import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { PublicUniversitiesRoutingModule } from './public-universities-routing.module';
import { SharedModule } from '@shared/shared.module';

import { PublicUniversitiesComponent } from './public-universities.component';
import { UniversityDetailComponent } from './university-detail.component';

@NgModule({
    declarations: [
        PublicUniversitiesComponent,
        UniversityDetailComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        SharedModule,
        PublicUniversitiesRoutingModule,
        PaginationModule.forRoot()
    ]
})
export class PublicUniversitiesModule { }
