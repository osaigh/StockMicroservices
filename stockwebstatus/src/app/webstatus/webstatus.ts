import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ServiceStatus, WebStatusService } from '../web-status-service';
import { catchError, map, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { WindowRefService } from '../window-ref-service';

@Component({
  selector: 'app-webstatus',
  imports: [CommonModule],
  templateUrl: './webstatus.html',
  styleUrl: './webstatus.scss'
})
export class Webstatus implements OnInit{
  services: ServiceStatus[] = [];
  webstatusapi:string ='http://127.0.0.1:5000/api/health-status';
  webStatusService: WebStatusService = inject(WebStatusService);
  

  constructor(){}

  ngOnInit(): void {
   setInterval(() => {
     this.webStatusService.getServiceStatuses().subscribe({
      next: results => {
        this.services = results
        console.log(this.services);
      },
      error: e => console.log(e)
    });
   }, 5000);
  }

  checkHealthStatuses(){
    setInterval(() => {
      this.webStatusService.getServiceStatuses()
      .pipe(map((results) => {
        console.log(results)
        this.services = results;
      }), catchError((e) => {
        console.log(e);
        return "";
      }));
    }, 5000);
  }
}
