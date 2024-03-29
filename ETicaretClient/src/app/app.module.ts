import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AdminModule } from './admin/admin.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UiModule } from './ui/ui.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BaseComponent } from './base/base.component';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { DeleteDirective } from './directives/admin/delete.directive';
import { DeleteDialogComponent } from './dialogs/delete-dialog/delete-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { FileUploadComponent } from './services/common/file-upload/file-upload.component';
import { FileUploadDialogComponent } from './dialogs/file-upload-dialog/file-upload-dialog.component';
import { JwtModule } from '@auth0/angular-jwt';
import { LoginComponent } from './ui/components/login/login.component';
import { FacebookLoginProvider, GoogleLoginProvider, SocialAuthServiceConfig, SocialLoginModule } from '@abacritt/angularx-social-login';
import { HttpErrorHandlerInterceptorService } from './services/common/http-error-handler-interceptor.service';

@NgModule({
  declarations: [
    AppComponent,
    /* DeleteDialogComponent*/
    LoginComponent
  ],
  imports: [
    BrowserModule, 
    AppRoutingModule,
    AdminModule,
    UiModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    NgxSpinnerModule,
    HttpClientModule,
    MatDialogModule,
    MatButtonModule,
    JwtModule.forRoot({
      config: { tokenGetter: () => localStorage.getItem("accessToken"), allowedDomains:["localhost:7225"]}
    }),
    SocialLoginModule
  ],
  providers: [{
    provide: "baseUrl", useValue: "https://localhost:7225/api", multi: true
  },
  {
    provide: "SocialAuthServiceConfig",
    useValue: {
    autoLogin: false,
    providers: [
       {
          id: GoogleLoginProvider.PROVIDER_ID,
        provider: new GoogleLoginProvider("461261631697-sri3rl7kgphihdrlghv3no7u1qui7akq.apps.googleusercontent.com")
      },
      {
        id: FacebookLoginProvider.PROVIDER_ID,
        provider: new FacebookLoginProvider("168121676204063")
      }
     ],
       onError: err => console.log(err)
     } as SocialAuthServiceConfig
    },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorHandlerInterceptorService, multi:true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
