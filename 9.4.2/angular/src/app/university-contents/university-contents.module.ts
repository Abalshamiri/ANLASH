import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

import { UniversityContentsRoutingModule } from './university-contents-routing.module';
import { SharedModule } from '@shared/shared.module';
import { UniversityContentServiceProxy } from '@shared/service-proxies/service-proxies';

// Components
import { UniversityContentsListComponent } from './university-contents-list.component';
import { ContentFormComponent } from './content-form.component';

@NgModule({
    declarations: [
        UniversityContentsListComponent,
        ContentFormComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        SharedModule,
        UniversityContentsRoutingModule,
        ModalModule.forChild(),
        TabsModule.forRoot(),
        BsDropdownModule.forRoot()
    ],
    providers: [
        UniversityContentServiceProxy
    ]
})
export class UniversityContentsModule { }
