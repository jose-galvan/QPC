import { DefectService } from './../Services/defect.service';
import { Defect } from './../Models/Defect';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'defect-detail',
  templateUrl: './defect-detail.component.html',
  styleUrls: ['./defect-detail.component.css']
})
export class DefectDetailComponent implements OnInit {

  @Input() defect: Defect;

  constructor(private service: DefectService) { }

  ngOnInit() {
  }

  Update()
  {
    this.service
      .Update(this.defect)
      .subscribe(result => {console.log(result);
      });
  }

  Cancel()
  {
    this.defect = new Defect();
  }


  
}
