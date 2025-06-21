import { inject, Injectable, OnInit } from '@angular/core';
import { WindowRefService } from './window-ref-service';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';

export interface ServiceStatus{
  name: string;
  url: string;
  status: 'Healthy' | 'Unhealthy' | 'Unknown';
}

@Injectable({
  providedIn: 'root'
})
export class WebStatusService {
  webstatusapi:string ='http://127.0.0.1:5000/api/health-status';
  windowRef: WindowRefService = inject(WindowRefService);
  http: HttpClient = inject(HttpClient);
  
  constructor() { 
    this.init();
  }

 init(): void {
    const win = this.windowRef.nativeWindow;
    if(win){
      console.log(win);
      if(win._env_){
        this.webstatusapi = win._env_.WEBSTATUSAPI
        
      }else{
        console.log("Config unavailable");
      }
      
    }else{
      console.log("Window not available");
    }
  }

  getServiceStatuses():Observable<ServiceStatus[]> {
      return this.http.get<ServiceStatus[]>(this.webstatusapi);
  }
}
