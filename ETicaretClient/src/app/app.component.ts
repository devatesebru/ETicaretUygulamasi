import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { MessageType, Position } from './services/admin/alertify.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from './services/ui/custom-toastr.service';
declare var $: any


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title='ETicaretClient';
  constructor(private toastrService: CustomToastrService) {
  
    toastrService.message("merhaba", "ben ebru",{messageType: ToastrMessageType.Info, position: ToastrPosition.BottomFullWidth});
    toastrService.message("merhaba", "ben ebru",{messageType: ToastrMessageType.Success, position: ToastrPosition.BottomLeft});
    toastrService.message("merhaba", "ben ebru", {messageType: ToastrMessageType.Warning, position: ToastrPosition.TopCenter});
    toastrService.message("merhaba", "ben ebru", {messageType: ToastrMessageType.Error, position: ToastrPosition.TopRight});
  }
 

}

