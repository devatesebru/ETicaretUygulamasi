import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { Product } from 'src/app/contracts/product';
import { HttpClientService } from 'src/app/services/common/http-client.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})


export class ProductsComponent extends BaseComponent implements OnInit {

  constructor( spinner: NgxSpinnerService, private httpClientService : HttpClientService) { 
    super(spinner)
  }

  ngOnInit(): void {


    this.showSpinner(SpinnerType.BallAtom);



     this.httpClientService.get<Product[]>({controller: "products" }).subscribe(data=> console.log(data));





    // this.httpClientService.post({controller:"products"},{name: "kalem", stock: 100 , price: 15}).subscribe();

    //  this.httpClientService.post({controller:"products"},{name: "silgi", stock: 45 ,price: 69}).subscribe();
      
    //  this.httpClientService.post({controller:"products"},{name: "kağıt", stock: 20 ,price: 78}).subscribe();
      
    //  this.httpClientService.post({controller:"products"},{name: "uç", stock: 12 ,price: 52}).subscribe();
      
    //  this.httpClientService.post({controller:"products"},{name: "defter", stock: 78 ,price: 50}).subscribe();
    //   this.httpClientService.put({controller: "products"},{id:"857c8de8-47e4-4208-a723-89b627917950",
    // name: "defterrenkli",stock: 1500 , price :50}).subscribe();

    // this.httpClientService.delete({controller:"products"},"857c8de8-47e4-4208-a723-89b627917950").subscribe();
     

    //başka sitedekileri çekktik iki farklı yöntemle
    //   this.httpClientService.get({
    // baseUrl: "https://jsonplaceholder.typicode.com",controller: "posts" }).subscribe(data =>console.log(data));     

    // this.httpClientService.get({
    //   fullEndPoint:"https://jsonplaceholder.typicode.com/posts"}).subscribe(data =>console.log(data));  
  

  }
}
