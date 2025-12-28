import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { UniversitiesRoutingModule } from './universities-routing.module';
import { SharedModule } from '@shared/shared.module';

// Components
import { UniversityListComponent } from './university-list.component';
import { UniversityFormComponent } from './university-form.component';

@NgModule({
    declarations: [
        UniversityListComponent,
        UniversityFormComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        SharedModule,
        UniversitiesRoutingModule,
        ModalModule.forChild(),
        PaginationModule.forRoot()
    ]
})
export class UniversitiesModule { }
