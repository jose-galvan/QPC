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
      .subscribe(products => {this.products = products});
  }

  onKey(event: any) { 
    console.log(event);
    
    if((event as KeyboardEvent).code =='Escape')
      this.SelectProduct(null);

    this.service.Get(event.target.value)
        .subscribe(
          products=> this.products = products);
  }

  SelectProduct(product:Product){
    this.selectedProduct = product;
  }

  addProduct(){
    var product = new Product()
    product.id =0;
    this.selectedProduct= product;

  }
  updateView(event: any) { 
    if((event as MouseEvent).srcElement.nodeName == "LI")
      return;
    this.SelectProduct(null);    
  }
}
