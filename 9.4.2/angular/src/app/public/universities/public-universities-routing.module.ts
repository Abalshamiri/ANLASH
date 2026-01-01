import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PublicUniversitiesComponent } from './public-universities.component';
import { UniversityDetailComponent } from './university-detail.component';

const routes: Routes = [
    {
        path: '',
        component: PublicUniversitiesComponent
    },
    {
        path: ':slug',
        component: UniversityDetailComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class PublicUniversitiesRoutingModule { }
