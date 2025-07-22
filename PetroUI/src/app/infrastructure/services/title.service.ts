import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})

export class TitleService {
  private title = new BehaviorSubject<string>('');
  public title$ = this.title.asObservable();
  updateTitle(name: string){
    this.title.next(name);
  }
  constructor() { }
}
