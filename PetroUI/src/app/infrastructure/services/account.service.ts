import { Injectable, signal } from '@angular/core';
import { UserFilter } from '../../presentation/administrator/account/user-record';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private filterSignal = signal<UserFilter>(
    {
      username: null,
      active: null,
      role: null
    }
  )
  updateFilter(filter: UserFilter){
    return this.filterSignal.set(filter)
  }
  getFilter(){
    return this.filterSignal()
  }
  constructor() { 
  }
}
