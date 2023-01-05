import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashbordComponent } from './dashbord.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    DashbordComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([{
      path:"", component: DashbordComponent
    }])
  ]
})
export class DashbordModule { }
