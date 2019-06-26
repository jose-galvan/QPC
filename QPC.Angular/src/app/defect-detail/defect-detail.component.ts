import { ProductService } from './../Services/product.service';
import { Product } from './../Models/Product';
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

  products: Product[];
  selectedProduct: number;

  constructor(private service: DefectService, private productService: ProductService) { }

  ngOnInit() {
    this.productService.GetAll()
      .subscribe(products => this.products = products);
  }




  Save()
  {
    if(this.defect.id != 0){
      this.service
        .Update(this.defect)
        .subscribe(result => {console.log(result);
        });
    }
    else{
      this.defect.ProductId = this.selectedProduct;
      this.service.Create(this.defect)
        .subscribe(result =>{
        console.log(result);
      });
    }
  }

  Cancel()
  {
    this.defect = new Defect();
  }
 
}
