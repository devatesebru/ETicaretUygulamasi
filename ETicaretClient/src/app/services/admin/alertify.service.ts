import { Injectable } from '@angular/core';
declare var alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  message(message: string, options: Partial<AlertifyOptions>)
  {
    alertify.set('notifier', 'position', options.position);
    const msj = alertify[options.messageType](message);
    alertify.set('notifier','delay', options.delay);
    if(options.dismissOthers==true){
      msj.dismissOthers(); 
    }
  }
  dismiss(){
    alertify.dismissAll();
  }
  
}
export class AlertifyOptions{
  messageType: MessageType = MessageType.Message;
  position : Position = Position.Bottomleft;
  delay: number = 3;
  dismissOthers: boolean = false;

  

}


export enum MessageType{
  Error = "error",
  Message ="message",
  Notify = "notify",
  Success = "success",
  Warning = "warning"
}


export enum Position{
  TopRight = "top-right",
  TopCenter = "top-center",
  Topleft = "top-left",
  BottomRight = "bottom-right",
  BottomCenter ="bottom-center",
  Bottomleft ="bottom-left"
}


