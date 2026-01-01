import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UniversityContentsListComponent } from './university-contents-list.component';

const routes: Routes = [
    {
        path: '',
        component: UniversityContentsListComponent,
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UniversityContentsRoutingModule { }
