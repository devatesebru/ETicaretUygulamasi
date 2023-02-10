import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Create_Product } from '../../../contracts/create_product';
import { List_Product } from '../../../contracts/list_product';
import { HttpClientService } from '../http-client.service';


@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClientService: HttpClientService) { }
  //veri ekleme ve çekme işlemini buradan yapacağız

  create(product: Create_Product, successCallBack?: any, errorCallBack?: (errorMessage: string) => void) {
    this.httpClientService.post({
      controller: "products"
    }, product).subscribe(result => { successCallBack(); }, (errorResponse: HttpErrorResponse) => {
      const _erorr: Array<{ key: string, value: Array<string> }> = errorResponse.error;
      let message = "";
      _erorr.forEach((v, index) => {
        v.value.forEach((_v, _index) => {
          message += `${_v}<br>`;
        });
      });
      errorCallBack(message);
    });
  }




  //bütün operasyonları servisten yapacağız
  async read(page: number = 0, size: number = 5, successCallBack?: () => void, errorCalback?: (errorMessage: string) => void): Promise<{ totalCount: number; products: List_Product[] }> {
    debugger;
    const promiseData: Promise<{ totalCount: number; products: List_Product[] }> = this.httpClientService.get<{totalCount: number; products: List_Product[]} >({
      controller: "products",
      queryString: `page=${page}&size=${size}`



    }).toPromise();
    promiseData.then(d => successCallBack()).catch((errorResponse: HttpErrorResponse) => errorCalback(errorResponse.message))
    return await promiseData;
  }












}