import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UniversityFaqsListComponent } from './university-faqs-list.component';

const routes: Routes = [
    {
        path: '',
        component: UniversityFaqsListComponent,
        pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UniversityFaqsRoutingModule { }
