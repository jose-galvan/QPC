import { CommonService } from './common.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class ProductService  extends CommonService {

  constructor(http: HttpClient) {
    super('http://localhost:49529/api/products', http)
   }


   
}
