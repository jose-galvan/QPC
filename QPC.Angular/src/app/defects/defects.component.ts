import { DefectService } from './../Services/defect.service';
import { Defect } from './../Models/Defect';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-defects',
  templateUrl: './defects.component.html',
  styleUrls: ['./defects.component.css']
})
export class DefectsComponent implements OnInit {


  defects: Defect[];
  selectedDefect: Defect;
  constructor(private service:DefectService) { }

  ngOnInit() {
    this.GetDefects();
  }

  GetDefects(){

    this.service.GetAll()
      .subscribe(result => {this.defects = result});
  }

  onKey(event: any) {
      this.service.Get(event.target.value)
          .subscribe(
            defects => this.defects = defects);
  }

  SelectDefect(defect:Defect){
    this.selectedDefect = defect;
  }

  addDefect(){
    var newdefect = new Defect();
    newdefect.id = 0;
    this.selectedDefect = newdefect;
  }

  updateView(event: any) { 
    if((event as MouseEvent).srcElement.nodeName == "LI")
      return;
    this.SelectDefect(null);    
  }

}
