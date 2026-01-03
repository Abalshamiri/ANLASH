import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { AppComponent } from './app.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    {
                        path: 'home',
                        loadChildren: () => import('./home/home.module').then((m) => m.HomeModule),
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'about',
                        loadChildren: () => import('./about/about.module').then((m) => m.AboutModule),
                        canActivate: [AppRouteGuard]
                    },
                    // TODO: Re-enable after fixing backend DTOs and methods
                    // {
                    //     path: 'universities',
                    //     loadChildren: () => import('./public/universities/public-universities.module').then((m) => m.PublicUniversitiesModule)
                    // },
                    {
                        path: 'users',
                        loadChildren: () => import('./users/users.module').then((m) => m.UsersModule),
                        data: { permission: 'Pages.Users' },
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'roles',
                        loadChildren: () => import('./roles/roles.module').then((m) => m.RolesModule),
                        data: { permission: 'Pages.Roles' },
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'tenants',
                        loadChildren: () => import('./tenants/tenants.module').then((m) => m.TenantsModule),
                        data: { permission: 'Pages.Tenants' },
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'universities',
                        loadChildren: () => import('./universities/universities.module').then((m) => m.UniversitiesModule),
                        data: { permission: 'Pages.Universities' },
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'university-programs',
                        loadChildren: () => import('./university-programs/university-programs.module').then((m) => m.UniversityProgramsModule),
                        data: { permission: 'Pages.Universities' },
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'university-faqs',
                        loadChildren: () => import('./university-faqs/university-faqs.module').then((m) => m.UniversityFaqsModule),
                        data: { permission: 'Pages.Universities' },
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'university-contents',
                        loadChildren: () => import('./university-contents/university-contents.module').then((m) => m.UniversityContentsModule),
                        data: { permission: 'Pages.Universities' },
                        canActivate: [AppRouteGuard]
                    },
                    {
                        path: 'update-password',
                        loadChildren: () => import('./users/users.module').then((m) => m.UsersModule),
                        canActivate: [AppRouteGuard]
                    },
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
