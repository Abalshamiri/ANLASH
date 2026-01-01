import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

import { UniversityProgramsRoutingModule } from './university-programs-routing.module';
import { SharedModule } from '@shared/shared.module';

// Components
import { UniversityProgramsListComponent } from './university-programs-list.component';
import { ProgramFormComponent } from './program-form.component';

@NgModule({
    declarations: [
        UniversityProgramsListComponent,
        ProgramFormComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        SharedModule,
        UniversityProgramsRoutingModule,
        ModalModule.forChild(),
        PaginationModule.forRoot(),
        BsDropdownModule.forRoot()
    ]
})
export class UniversityProgramsModule { }
