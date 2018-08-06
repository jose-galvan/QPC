import { HttpClient } from '@angular/common/http';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';

@Injectable()
export class QualityControlService extends CommonService {

  constructor(http: HttpClient) {
    super('http://localhost:49529/api/controls', http)
   }
   
  GetById(controlId:string){
    return this.http.get('http://localhost:49529/api/control/'+ controlId)
              .map(response => response)
              .catch(this.handleError);
  }
}
