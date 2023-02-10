import { Component,OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, _MatTableDataSource } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from '../../../../base/base.component';
import { List_Product } from '../../../../contracts/list_product';
import { AlertifyService, MessageType, Position } from '../../../../services/admin/alertify.service';
import { ProductService } from '../../../../services/common/models/product.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends BaseComponent implements OnInit{
  constructor(spinner: NgxSpinnerService, private productServise: ProductService, private alertifyService: AlertifyService) {
    super(spinner);
  }

  displayedColumns: string[] = ['name', 'stock', 'price', 'createdDate', 'updatedDate'];


  dataSource: MatTableDataSource<List_Product> = null;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  


  async getProducts() {
    //spinner çağırmak için yukarıda extend ve spinner service yi çağırmamız lazım
    //alertify da kullanacağız onuda ekleyelim 

    this.showSpinner(SpinnerType.BallAtom);

    const allProducts: {
      totalCount: number; products: List_Product[]
    } = await this.productServise.read(this.paginator ? this.paginator.pageIndex : 0, this.paginator ?   this.paginator.pageSize : 5 , () => this.hideSpinner(SpinnerType.BallAtom), errorMessage => this.alertifyService.message(errorMessage, {
      dismissOthers: true,
      messageType: MessageType.Error,
      position: Position.TopRight

    }))
    debugger;
    this.dataSource = new MatTableDataSource<List_Product>(allProducts.products);
    this.paginator.length = allProducts.totalCount;
 /*   this.dataSource.paginator = this.paginator;*/
  }

  async pageChanged() {
   await this.getProducts();
  }

  async ngOnInit() {
   await  this.getProducts();
  }
}

