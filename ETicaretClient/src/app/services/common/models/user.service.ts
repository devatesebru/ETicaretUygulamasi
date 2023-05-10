import { SocialUser } from '@abacritt/angularx-social-login';
import { Injectable } from '@angular/core';
import { firstValueFrom, observable, Observable } from 'rxjs';
import { Token } from '../../../contracts/token/token';
import { TokenResponse } from '../../../contracts/token/tokenResponse';
import { Create_User } from '../../../contracts/users/create_user';
import { User } from '../../../entities/user';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../../ui/custom-toastr.service';
import { HttpClientService } from '../http-client.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClientService: HttpClientService, private toastrService: CustomToastrService ){ }

  async create(user: User): Promise<Create_User> {
    const observable: Observable<Create_User | User > = this.httpClientService.post<Create_User | User>({
      controller: "users"
    }, user);
    return await firstValueFrom(observable) as Create_User;
  }
 
  async login(userNameOrEmail: string, password: string,callBackFunction?: ()=> void): Promise<void> {
    const observable: Observable<any> = this.httpClientService.post<any | TokenResponse>({
      controller: "users",
      action: "login"
    }, { userNameOrEmail, password })
    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;
    if (tokenResponse) {  
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);    
     /* localStorage.setItem("expiration", token.expiration.toString());*/
      this.toastrService.message("Kulllanıcı giriş başarılıyla sağlanmıştır", "Giriş başarılı", {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight
      });
    }

    callBackFunction();
    
  }

  async googleLogin(user: SocialUser, callBackFunction?: () => void): Promise<any> {
    const observable: Observable<SocialUser | TokenResponse> = this.httpClientService.post<SocialUser | TokenResponse>({
      controller: "users",
      action: "google-login",
    }, user);
    const tokenResponse: TokenResponse = await firstValueFrom(observable) as TokenResponse;
    if (tokenResponse) {
      localStorage.setItem("accessToken", tokenResponse.token.accessToken);
      this.toastrService.message("Google üzerinden giriş başarıyla sağlanmıştır.", "Giriş Başarılı", {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight
      })
    }
    callBackFunction();
    
  }
}
