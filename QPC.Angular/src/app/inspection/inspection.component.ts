import { QualityControlService } from './../Services/quality-control.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'inspection',
  templateUrl: './inspection.component.html',
  styleUrls: ['./inspection.component.css']
})

export class InspectionComponent implements OnInit {

  id:string;
  constructor(private route: ActivatedRoute,
    private router: Router, private service : QualityControlService) 
    {
    }
    
    ngOnInit() { 
      
      this.id = this.route.snapshot.paramMap.get('id');
  }

}
