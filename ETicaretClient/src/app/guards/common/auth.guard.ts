import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { SpinnerType } from '../../base/base.component';
import { AuthService, _isAuthenticated } from '../../services/common/auth.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../services/ui/custom-toastr.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private jwtHelper: JwtHelperService, private router: Router, private toastrService: CustomToastrService, private spinner: NgxSpinnerService) {

  }

  canActivate(
    route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.spinner.show(SpinnerType.BallAtom);

   // const token: string = localStorage.getItem("accessToken");

   //// const decodetoken = this.jwtHelper.decodeToken(token);
   //// const expirationDate: Date = this.jwtHelper.getTokenExpirationDate(token);
   // let expired: boolean;
   // try {
   //   expired = this.jwtHelper.isTokenExpired(token);
   // } catch (e) {
   //   expired = true;
    // }
    if (!_isAuthenticated) {
      this.router.navigate(["login"], { queryParams: { returnUrl: state.url } });
      this.toastrService.message("Oturum açın lütfen ", "Yetkisiz Erişim!", {
        messageType: ToastrMessageType.Warning,
        position: ToastrPosition.TopRight
      })
    }
    this.spinner.hide(SpinnerType.BallAtom);

    return true;
  }
  
}
