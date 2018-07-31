import { CommonService } from './common.service';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

@Injectable()
export class QualityControlService extends CommonService {

  constructor(http: Http) {
    super('http://localhost:50598/api/controls', http)
   }
}
