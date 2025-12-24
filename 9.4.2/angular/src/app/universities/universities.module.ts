import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { UniversitiesRoutingModule } from './universities-routing.module';
import { UniversitiesComponent } from './universities.component';
import { CreateUniversityDialogComponent } from './create-university/create-university-dialog.component';
import { EditUniversityDialogComponent } from './edit-university/edit-university-dialog.component';
import { UniversityServiceProxy } from '@shared/service-proxies/service-proxies';

@NgModule({
    declarations: [
        UniversitiesComponent,
        CreateUniversityDialogComponent,
        EditUniversityDialogComponent
    ],
    imports: [SharedModule, UniversitiesRoutingModule],
    providers: [UniversityServiceProxy]
})
export class UniversitiesModule { }
