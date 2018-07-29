import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule} from '@angular/forms';
import { RouterModule} from '@angular/router';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { NavbarComponent } from './navbar/navbar.component';
import { RegisterComponent } from './register/register.component';
import { QualityControlsComponent } from './quality-controls/quality-controls.component';
import { QualityControlDetailComponent } from './quality-control-detail/quality-control-detail.component';
import { QualityControlService } from './Services/quality-control.service';
import { ProductsComponent } from './products/products.component';
import { DefectService } from './Services/defect.service';
import { ProductService } from './Services/product.service';
import { InspectionService } from './Services/inspection.service';
import { InstructionService } from './Services/instruction.service';

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
    ProductsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot([
      {path: 'login', component: LoginComponent},
      {path: 'register', component: RegisterComponent},
      {path: 'controls', component: QualityControlsComponent},
      {path: 'products', component: ProductsComponent}
   ])
  ],
  providers: [
    QualityControlService, 
    DefectService,
    ProductService,
    InspectionService,
    InstructionService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
