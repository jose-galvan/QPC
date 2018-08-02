import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { QualityControl } from '../Models/QualityControl';
import { QualityControlService } from '../Services/quality-control.service';

@Component({
  selector: 'app-quality-control-detail',
  templateUrl: './quality-control-detail.component.html',
  styleUrls: ['./quality-control-detail.component.css']
})
export class QualityControlDetailComponent implements OnInit {

  control : QualityControl;

  constructor(private route: ActivatedRoute,
    private router: Router, private service : QualityControlService) { }

  ngOnInit() { 
    var id = this.route.snapshot.paramMap.get('id');
    this.getControl(id);
  }

  getControl(id){
    this.service.GetById(id)
        .subscribe(result =>{this.control = result});
  }

  Update(){
        this.service.Update(this.control)
    .subscribe(result => this.getControl(this.control.id));
  }

}
