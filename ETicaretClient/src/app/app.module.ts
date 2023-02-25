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
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { DeleteDirective } from './directives/admin/delete.directive';
import { DeleteDialogComponent } from './dialogs/delete-dialog/delete-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { FileUploadComponent } from './services/common/file-upload/file-upload.component';

@NgModule({
  declarations: [
    AppComponent,
    DeleteDialogComponent,

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
    MatButtonModule
  ],
  providers: [{
    provide: "baseUrl", useValue: "https://localhost:7225/api", multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
