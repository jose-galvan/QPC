import { QualityControlService } from './../Services/quality-control.service';
import { QualityControl } from './../Models/QualityControl';
import { Instruction } from './../Models/Instruction';
import { InstructionService } from './../Services/instruction.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'instructions',
  templateUrl: './instructions.component.html',
  styleUrls: ['./instructions.component.css']
})
export class InstructionsComponent implements OnInit {

  id:string;
  instructions: Instruction[];
  control: QualityControl;

  constructor(private route: ActivatedRoute,
    private router: Router, private service: InstructionService, 
    private controlService: QualityControlService) { }


  ngOnInit() { 
    this.id = this.route.snapshot.paramMap.get('id');
    this.controlService.GetById(this.id)
        .subscribe(control => this.control = control);
    this.getInstructions();
  }

  getInstructions(){
    this.service.GetByControl(this.id)
        .subscribe(result => {this.instructions = result});
  }

  UpdateInstruction(instruction: Instruction){
    this.service.UpdateStatus(instruction)
    .subscribe(()=> this.getInstructions());
  }
}
