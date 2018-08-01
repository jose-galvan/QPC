import { HttpClient } from '@angular/common/http';
import { BadInputError } from '../common/bad-input-error';
import { NotFoundError } from '../common/not-found-error';
import { AppError } from './../common/app-error';
import { Injectable } from '@angular/core';
import { Observable} from 'rxjs/Observable'
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';

@Injectable()
export class CommonService {
  
  constructor(private url: string, private http: HttpClient) { }

  GetAll(){
        return this.http.get(this.url)
        .map(response => response)
        .catch(this.handleError);
  }

  Create(resource){
     return this.http
       .post(this.url, JSON.stringify(resource))
       .map(response => response)
       .catch(this.handleError);
  }

  Update(resource){
   return this.http
        .patch(this.url + '/'+ resource.id, JSON.stringify(resource))
        .map(response => response)
        .catch(this.handleError);
  }

  Delete(id){
    return this.http.delete(this.url + '/'+ id)
        .map(response => response)
        .catch(this.handleError);
  }

  private handleError(error: Response){
      switch(error.status){
        case 400:
          return Observable.throw(new BadInputError());
        case 404:
          return Observable.throw(new NotFoundError());
        default:
          return Observable.throw(new AppError(error));
      }
  }
}
