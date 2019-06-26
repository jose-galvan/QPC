import { QualityControlService } from './../Services/quality-control.service';
import { DefectService } from './../Services/defect.service';
import { Defect } from './../Models/Defect';
import { ProductService } from './../Services/product.service';
import { QualityControl } from './../Models/QualityControl';
import { Component, OnInit } from '@angular/core';
import { Product } from '../Models/Product';

@Component({
  selector: 'control-request',
  templateUrl: './control-request.component.html',
  styleUrls: ['./control-request.component.css']
})
export class ControlRequestComponent implements OnInit {

  control:QualityControl;

  selectedProduct:number;
  products: Product[];
  selectedDefect: number;
  defects: Defect[];

  constructor(private service:QualityControlService, 
              private productService: ProductService,
              private defectService: DefectService) { }

  ngOnInit() {
    this.control = new QualityControl();
    this.productService.GetAll()
        .subscribe(products => this.products = products);
  }

  onChange(event: any){
    this.defectService.GetByProduct(this.selectedProduct)
        .subscribe(defects => this.defects = defects);
  }

  Save(){
    var request: any ={
      name: this.control.name,
      description: this.control.description,
      serial: this.control.serial,
      product: this.selectedProduct,
      defect: this.selectedDefect
    }
    this.service.Create(request)
          .subscribe(result => console.log(result));
  }

}
