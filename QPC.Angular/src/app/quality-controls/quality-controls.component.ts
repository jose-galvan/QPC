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

  constructor(private service : QualityControlService) { }

  ngOnInit() {
    this.GetControls();
  }

  GetControls(){
    this.service.GetAll()
        .subscribe(
          controls => this.controls = controls);
  }

}
