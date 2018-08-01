import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthGuard } from './auth/auth.guard';
import { AuthService } from './Services/auth.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import {FormsModule} from '@angular/forms';
import { RouterModule} from '@angular/router';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { NavbarComponent } from './navbar/navbar.component';
import { RegisterComponent } from './register/register.component';
import { QualityControlService } from './Services/quality-control.service';
import { DefectService } from './Services/defect.service';
import { ProductService } from './Services/product.service';
import { InspectionService } from './Services/inspection.service';
import { InstructionService } from './Services/instruction.service';
import { QualityControlsComponent } from './quality-controls/quality-controls.component';
import { QualityControlDetailComponent } from './quality-control-detail/quality-control-detail.component';
import { AppErrorHandler } from 'app/common/app-error-handler';
import {HttpModule} from '@angular/http';
import { HomeComponent } from './home/home.component';
import { AuthInterceptor } from './auth/auth.interceptor';
import { NotFoundComponent } from './not-found/not-found.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavbarComponent,
    SidebarComponent,
    NavbarComponent,
    RegisterComponent,
    QualityControlsComponent,
    QualityControlDetailComponent,
    HomeComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    FormsModule, HttpModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/controls',  pathMatch: 'full' },
      {path: 'controls', component: QualityControlsComponent, canActivate:[AuthGuard] },
      {path: 'control/:id', component: QualityControlDetailComponent, canActivate:[AuthGuard] },
      {path: 'login', component: LoginComponent},
      {path: 'register', component: RegisterComponent},
      {path:'**', component:NotFoundComponent}
   ])
  ],
  providers: [
    QualityControlService, 
    DefectService,
    ProductService,
    InspectionService,
    InstructionService, 
    AuthService,
    HttpClientModule,
    AuthGuard,
    {
      provide : HTTP_INTERCEPTORS,
      useClass : AuthInterceptor,
      multi : true
    },
    {provide: ErrorHandler, useClass: AppErrorHandler}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
