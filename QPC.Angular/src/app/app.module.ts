import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import {FormsModule} from '@angular/forms';
import { RouterModule} from '@angular/router';
import {HttpModule} from '@angular/http';


import { AuthGuard } from './auth/auth.guard';
import { AuthService } from './Services/auth.service';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
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
import { HomeComponent } from './home/home.component';
import { AuthInterceptor } from './auth/auth.interceptor';
import { NotFoundComponent } from './not-found/not-found.component';
import { ProductsComponent } from './products/products.component';
import { ProductDetailComponent } from './product-detail/product-detail.component';
import { DefectsComponent } from './defects/defects.component';
import { DefectDetailComponent } from './defect-detail/defect-detail.component';
import { Router } from './Routes';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    NavbarComponent,
    NavbarComponent,
    RegisterComponent,
    QualityControlsComponent,
    QualityControlDetailComponent,
    HomeComponent,
    NotFoundComponent,
    ProductsComponent,
    ProductDetailComponent,
    DefectsComponent,
    DefectDetailComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    FormsModule, HttpModule,
    RouterModule.forRoot(Router.routes)
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
