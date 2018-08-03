import { InstructionService } from './../Services/instruction.service';
import { Instruction } from './../Models/Instruction';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'instruction-detail',
  templateUrl: './instruction-detail.component.html',
  styleUrls: ['./instruction-detail.component.css']
})
export class InstructionDetailComponent implements OnInit {

  @Input() controlId: number;

  instruction: Instruction;

  constructor(private service: InstructionService) { }

  ngOnInit() {
    console.log('QC:' +this.controlId);
    
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

  Cancel(){
    
  }

}
