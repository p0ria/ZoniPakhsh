import {BrowserModule, TransferState} from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ClarityModule } from '@clr/angular';
import { MasterTemplateComponent } from './master-template/master-template.component';
import { HeaderComponent } from './master-template/header/header.component';
import { SideNavComponent } from './master-template/side-nav/side-nav.component';
import { LoginComponent } from './login/login.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {LoginService} from './services/login.service';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {AuthGuard} from './services/auth.guard';
import {AuthInterceptor} from './services/auth.interceptor';
import {ProductService} from './services/product.service';
import { ProductsComponent } from './products/products.component';
import { CategoriesComponent } from './categories/categories.component';
import {CategoryService} from './services/category.service';
import { UsersComponent } from './users/users.component';
import {UserService} from './services/user.service';
import { BaseAddressPipe } from './pipes/base-address.pipe';
import { OrdersComponent } from './orders/orders.component';
import {FlexLayoutModule} from '@angular/flex-layout';
import {OrderService} from './services/order.service';
import { OrderStatePipe } from './pipes/order-state.pipe';
import { DashboardComponent } from './dashboard/dashboard.component';
import {ThemeService} from './services/theme.service';
import { OrderStatusFilterComponent } from './order-status-filter/order-status-filter.component';
import {HashLocationStrategy, LocationStrategy} from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    MasterTemplateComponent,
    HeaderComponent,
    SideNavComponent,
    LoginComponent,
    ProductsComponent,
    CategoriesComponent,
    UsersComponent,
    BaseAddressPipe,
    OrdersComponent,
    OrderStatePipe,
    DashboardComponent,
    OrderStatusFilterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ClarityModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FlexLayoutModule
  ],
  providers: [
    LoginService,
    AuthGuard,
    ProductService,
    CategoryService,
    UserService,
    OrderService,
    ThemeService,
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    {provide: LocationStrategy, useClass: HashLocationStrategy}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
