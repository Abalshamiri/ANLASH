import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// import { DragDropModule } from '@angular/cdk/drag-drop';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

import { UniversityFaqsRoutingModule } from './university-faqs-routing.module';
import { SharedModule } from '@shared/shared.module';

// Components
import { UniversityFaqsListComponent } from './university-faqs-list.component';
import { FaqFormComponent } from './faq-form.component';

@NgModule({
    declarations: [
        UniversityFaqsListComponent,
        FaqFormComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        // DragDropModule,
        SharedModule,
        UniversityFaqsRoutingModule,
        ModalModule.forChild(),
        PaginationModule.forRoot(),
        BsDropdownModule.forRoot()
    ]
})
export class UniversityFaqsModule { }
