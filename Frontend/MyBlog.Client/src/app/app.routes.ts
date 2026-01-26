import { Routes } from '@angular/router';
import { LoginComponent } from '../features/auth/login/login.component';
import { RegisterComponent } from '../features/auth/register/register.component';
import { DashboardLayoutComponent } from '../features/dashboard/layout/dashboard-layout.component';
import { DashboardHomeComponent } from '../features/dashboard/layout/dashboard-home.component';
import { CategoryComponent } from '../features/dashboard/category/category.component';
import { AuthGuard } from '../core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'dashboard',
    component: DashboardLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        component: DashboardHomeComponent
      },
      {
        path: 'category',
        component: CategoryComponent,
        canActivate: [AuthGuard]
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
