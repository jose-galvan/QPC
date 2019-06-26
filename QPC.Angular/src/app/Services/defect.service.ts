import { HttpClient } from '@angular/common/http';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';

@Injectable()
export class DefectService  extends CommonService{

  constructor(http: HttpClient) {
    super('http://localhost:49529/api/defects', http)
   }

  GetByProduct(product: number){
    return this.http.get(`http://localhost:49529/api/product/${product}/defects`)
    .map(response => response)
    .catch(this.handleError);
  }
}
