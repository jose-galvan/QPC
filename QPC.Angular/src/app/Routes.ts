import { InspectionComponent } from './inspection/inspection.component';
import { InstructionsComponent } from './instructions/instructions.component';
import { AuthGuard } from './auth/auth.guard';
import { NotFoundComponent } from './not-found/not-found.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { DefectsComponent } from './defects/defects.component';
import { ProductsComponent } from './products/products.component';
import { QualityControlDetailComponent } from './quality-control-detail/quality-control-detail.component';
import { QualityControlsComponent } from './quality-controls/quality-controls.component';

const routes =
    [
        { path: '', redirectTo: '/controls',  pathMatch: 'full' },
        {path: 'controls', component: QualityControlsComponent, canActivate:[AuthGuard] },
        {path: 'control/:id', component: QualityControlDetailComponent, canActivate:[AuthGuard] },
        {path: 'instructions/:id', component: InstructionsComponent, canActivate:[AuthGuard] },
        {path: 'inspection/:id', component: InspectionComponent, canActivate:[AuthGuard] },
        {path: 'products', component: ProductsComponent, canActivate:[AuthGuard] },
        {path: 'defects', component: DefectsComponent, canActivate:[AuthGuard] },
        {path: 'login', component: LoginComponent},
        {path: 'register', component: RegisterComponent},
        {path:'**', component:NotFoundComponent, canActivate:[AuthGuard]}
    ];

export class Router{
    public static routes = routes;
}