import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { UserService } from '../../../services/common/models/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends BaseComponent  {
 
  constructor(private userService: UserService, spinner: NgxSpinnerService) {
  super(spinner)}

  async login(userNameOrEmail: string, password: string) {
    this.showSpinner(SpinnerType.BallAtom);
    await this.userService.login(userNameOrEmail, password, () => this.hideSpinner(SpinnerType.BallAtom));
  }

}
