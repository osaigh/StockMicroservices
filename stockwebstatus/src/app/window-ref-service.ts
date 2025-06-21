import { Injectable } from '@angular/core';

function getWindow(): Window | null{
  return typeof window !== 'undefined' ? window : null;
}

@Injectable({
  providedIn: 'root'
})
export class WindowRefService {

  constructor() { }

  get nativeWindow(): Window | null{
    return getWindow();
  }
}
