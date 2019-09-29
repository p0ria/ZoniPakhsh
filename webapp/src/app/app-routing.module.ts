import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {MasterTemplateComponent} from './master-template/master-template.component';
import {LoginComponent} from './login/login.component';
import {AuthGuard} from './services/auth.guard';
import {CategoriesComponent} from './categories/categories.component';
import {UsersComponent} from './users/users.component';
import {ProductsComponent} from './products/products.component';
import {OrdersComponent} from './orders/orders.component';
import {DashboardComponent} from './dashboard/dashboard.component';

const routes: Routes = [
  {path: '', component: MasterTemplateComponent, canActivate: [AuthGuard],
    children: [
      {path: 'categories', component: CategoriesComponent},
      {path: 'categories/:id/products', component: ProductsComponent},
      {path: 'products', component: ProductsComponent},
      {path: 'users', component: UsersComponent},
      {path: 'orders', component: OrdersComponent},
      {path: 'dashboard', component: DashboardComponent}
    ]},
  {path: 'login', component: LoginComponent},
  {path: 'logout', redirectTo: 'login'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
