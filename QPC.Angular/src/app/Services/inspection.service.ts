import { HttpClient } from '@angular/common/http';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';

@Injectable()
export class InspectionService  extends CommonService{

  constructor(http: HttpClient) {
    super('http://localhost:49529/api/inspections', http)
   }
   
  GetByControl(controlId:number){
    return this.http.get(`http://localhost:49529/api/control/${controlId}/inspection`)
              .map(response => response)
              .catch(this.handleError);
  }

  GetDesicions(){
    return this.http.get('http://localhost:49529/api/inspection/desicions')
              .map(response => response)
              .catch(this.handleError);
  }

}
