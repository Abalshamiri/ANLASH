import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UniversityProgramsListComponent } from './university-programs-list.component';

const routes: Routes = [
    {
        path: '',
        component: UniversityProgramsListComponent,
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UniversityProgramsRoutingModule { }
