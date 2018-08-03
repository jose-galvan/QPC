import { Instruction } from './../Models/Instruction';
import { HttpClient } from '@angular/common/http';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';

@Injectable()
export class InstructionService extends CommonService{

  constructor(http: HttpClient) {
    super('http://localhost:49529/api/instructions', http);
  }

  GetByControl(controlId){
    return this.http.get(`http://localhost:49529/api/control/${controlId}/instructions`)
              .map(response => response)
              .catch(this.handleError);
  }

  UpdateStatus(instruction:Instruction){
    return this.http.put('http://localhost:49529/api/instructions/' + instruction.id, null);
  }

}
