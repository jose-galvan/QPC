import { Product } from './../Models/Product';
import { ProductService } from './../Services/product.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  products: Product[];
  selectedProduct: Product;
  constructor(private service:ProductService) { }

  ngOnInit() {
    this.GetProducts();
  }

  GetProducts(){

    this.service.GetAll()
      .subscribe(result => {this.products = result});
  }

  onKey(event: any) { // without type info
    console.log(event);
    
      this.service.Get(event.target.value)
          .subscribe(
            products => this.products = products);
  }

  SelectProduct(product:Product){
    this.selectedProduct = product;
  }

}
