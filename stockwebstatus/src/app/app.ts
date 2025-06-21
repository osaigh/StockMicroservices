import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Webstatus } from './webstatus/webstatus';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,Webstatus],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'stockwebstatus';
}
