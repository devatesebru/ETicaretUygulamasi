import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { AlertifyOptions, AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { HubUrls } from '../../../constants/hub-urls';
import { ReceiveFunctions } from '../../../constants/receive-functions';
import { SignalRService } from '../../../services/common/signalr.service';

@Component({
  selector: 'app-dashbord',
  templateUrl: './dashbord.component.html',
  styleUrls: ['./dashbord.component.scss']
})
export class DashbordComponent extends BaseComponent implements OnInit {

  constructor(private alertify: AlertifyService, spinner: NgxSpinnerService, private signalRservice: SignalRService) { 
    super(spinner)
    signalRservice.start(HubUrls.ProductHub)
  }

  ngOnInit(): void {

    this.signalRservice.on(ReceiveFunctions.ProductAddedMessageReceiveFunction, message => {
      this.alertify.message(message, {
        messageType: MessageType.Notify,
        position: Position.TopRight
      });
    });

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
