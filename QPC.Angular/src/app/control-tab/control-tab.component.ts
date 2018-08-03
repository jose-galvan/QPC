import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'control-tab',
  templateUrl: './control-tab.component.html',
  styleUrls: ['./control-tab.component.css']
})
export class ControlTabComponent {

  @Input() id:string;
  
  constructor() { }
}
