import { ProductService } from './../Services/product.service';
import { Product } from './../Models/Product';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {

  @Input() product :Product;

  constructor(private service: ProductService) { }

  ngOnInit() {
  }
  
  Update()
  {
    if(this.product.id > 0)
    {
      this.service
        .Update(this.product)
        .subscribe(result => {console.log(result);
        });
    }
    else{
      this.service.Create(this.product)
      .subscribe(result => {console.log(result);
      });
    }
  }

  Cancel()
  {
    this.product = new Product();
  }
}
