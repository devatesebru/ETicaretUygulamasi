import { Component, OnInit } from '@angular/core';
import { AlertifyOptions, AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';

@Component({
  selector: 'app-dashbord',
  templateUrl: './dashbord.component.html',
  styleUrls: ['./dashbord.component.scss']
})
export class DashbordComponent implements OnInit {

  constructor(private alertify: AlertifyService) { }

  ngOnInit(): void {
  

  }

  m(){
    this.alertify.message("Merhaba",
    {messageType :MessageType.Success, 
    delay: 5,
  position: Position.TopRight,

 } );

  }
  d(){
    this.alertify.dismiss();
  }

}
