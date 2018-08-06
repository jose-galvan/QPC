import { QualityControlService } from './../Services/quality-control.service';
import { InstructionService } from './../Services/instruction.service';
import { Instruction } from './../Models/Instruction';
import { Component, OnInit, Input } from '@angular/core';
import { QualityControl } from '../Models/QualityControl';

@Component({
  selector: 'instruction-detail',
  templateUrl: './instruction-detail.component.html',
  styleUrls: ['./instruction-detail.component.css']
})
export class InstructionDetailComponent implements OnInit {

  @Input() controlId: number;

  instruction: Instruction;
  control : QualityControl;

  constructor(private service: InstructionService, private controlService: QualityControlService) { }

  ngOnInit() {
    this.controlService.GetById(this.controlId.toString())
    .subscribe(control =>{
        this.control = control;
      });

    this.instruction = new Instruction();
    this.instruction.name ='New Instruction'
    this.instruction.QualityControlId = this.controlId;
  }

  Save(){
    this.service.Create(this.instruction)
      .subscribe((data : any) =>{ 
        console.log(data);
        this.instruction = null});
  }

}
