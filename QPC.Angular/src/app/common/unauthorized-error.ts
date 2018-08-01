import { Router } from '@angular/router';
import { AppError } from './app-error';
export class UnAuthorizedError extends AppError{
    /**
     *
     */
    constructor(private router: Router) {
        super();
        this.router.navigateByUrl('/login');
    }

}