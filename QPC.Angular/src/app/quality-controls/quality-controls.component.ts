import { Router } from '@angular/router';
import { QualityControlService } from './../Services/quality-control.service';
import { QualityControl } from './../Models/QualityControl';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-quality-controls',
  templateUrl: './quality-controls.component.html',
  styleUrls: ['./quality-controls.component.css']
})
export class QualityControlsComponent implements OnInit {

  controls: QualityControl[];
  constructor(private router:Router, private service : QualityControlService) { }

  ngOnInit() {
    this.GetControls();
  }

  GetControls(){
    this.service.GetAll()
        .subscribe(
          controls => this.controls = controls);
  }

  onKey(event: any) { // without type info
    this.service.Get(event.target.value)
        .subscribe(
          controls => this.controls = controls);
  }

  showControl(selectedControl:QualityControl){
    this.router.navigateByUrl('/control/'+selectedControl.id);
  }

}
