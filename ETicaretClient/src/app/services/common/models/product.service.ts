import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Create_Product } from '../../../contracts/create_product';
import { HttpClientService } from '../http-client.service';


@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClientService: HttpClientService) { }
    //veri ekleme ve çekme işlemini buradan yapacağız

  create(product: Create_Product, succesCallback?: any) {
    this.httpClientService.post({
      controller: "products"
    }, product).subscribe(result => { succesCallback(); });

    }
}
