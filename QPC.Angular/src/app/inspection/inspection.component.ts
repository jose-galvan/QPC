import { Desicion } from './../Models/Desicion';
import { Inspection } from './../Models/Inspection';
import { InspectionService } from './../Services/inspection.service';
import { QualityControlService } from './../Services/quality-control.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { QualityControl } from '../Models/QualityControl';

@Component({
  selector: 'inspection',
  templateUrl: './inspection.component.html',
  styleUrls: ['./inspection.component.css']
})

export class InspectionComponent implements OnInit {

  id:string;
  control: QualityControl;
  inspection: Inspection;
  desicions: Desicion[];
  selectedDesicion: Desicion;
  constructor(private route: ActivatedRoute,
    private router: Router, private service: InspectionService,
       private controlService : QualityControlService) 
    {
    }
    
    ngOnInit() {   
      this.id = this.route.snapshot.paramMap.get('id');
      this.GetDesicions();
      this.controlService.GetById(this.id)
      .subscribe(control =>{
          this.control = control;
          this.GetInspection();
        });
  }

  GetInspection(){
    console.log(this.control);
    this.inspection = new Inspection();
    this.inspection.QualityControlId = this.control.id;
    
    if(this.control.status != undefined){
      this.service.GetByControl(this.control.id)
          .subscribe(inspection => this.inspection = inspection);
    }
  }

  GetDesicions(){
    this.service.GetDesicions()
        .subscribe(desicions => this.desicions = desicions);
  }

  Save(){
    this.service.Create(this.inspection)
        .subscribe(result=> console.log(result)
        );
  }

}
