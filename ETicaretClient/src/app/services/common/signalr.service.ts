import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  constructor() { }
  private _connection: HubConnection;
  
  get connection(): HubConnection {
    return this._connection;
  }

  //başlatılmış bir hub verecek bana
  start(hubUrl:string) {
    if (!this.connection || this._connection?.state == HubConnectionState.Disconnected) {

      const builder: HubConnectionBuilder = new HubConnectionBuilder();
      const hubConnection: HubConnection = builder.withUrl(hubUrl).withAutomaticReconnect().build();

      hubConnection.onreconnected(connectionId => console.log("Reconnected"));
      //kopanbağlantının tekrardan bağlanma aşamasında olduğunu belli ediyor
      hubConnection.onreconnecting(error => console.log("Reconnecting"));
      //tekrar edilen bağlantı kurulamadığında hata vericek işte bağlantı koptu diyeceğiz
      hubConnection.onclose(error => console.log("Close reconnection"));

      //burada bağlantı olana kadar sürekli bu fonksiyonu çağır dedik ,başarılı ise git connected yaz
      hubConnection.start().then(() =>
        console.log("Connected"))
        .catch(error => setTimeout(() => this.start(hubUrl), 2000));
       this._connection = hubConnection;
    } 
    //kopan bağlantı tekrar bağlantı sağlanırsa burada durum yönetimi gerçekleştirebiliriz
    //tekrardan bağlandı yazdırdım
    

  }

  //signalR üzerinden bir clientin diğer clientlere mesaj yollamak için
//backend de hangi fonksiyona mesaj yollayacaksak onun ismini buradan almamız lazım
  invoke(procedureName:string ,message:any,successCallBack?:(value) => void,  errorCallBack?:(error) => void) {

    this.connection.invoke(procedureName, message)
      .then(successCallBack)
      .catch(errorCallBack);
  }

  //server dan  gelen mesajları runtime zamanı yakalamak için kullanacağım
  on(procedureName: string, callBack: (...message : any) => void) {
    this.connection.on(procedureName, callBack);
  }
}
