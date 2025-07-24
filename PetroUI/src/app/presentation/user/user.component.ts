import { Location, NgClass } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, Event, RouterLink, RouterOutlet, RouterEvent, NavigationEnd } from '@angular/router';
import { TitleService } from '../../infrastructure/services/title.service';
import { filter } from 'rxjs';
import { LogoutService } from '../../infrastructure/services/logout.service';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [RouterOutlet, NgClass, RouterLink],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit{
  isNavExtended = true;
  isReturnable = false;
  title: string = "";
  logout: () => Promise<boolean>;
  ngOnInit(): void {
     this.title = "Dashboard"
  }
  constructor(
    private titleService: TitleService, 
    private router: Router, 
    private location: Location,
    private logoutService: LogoutService
  ){
    this.logout = this.logoutService.logout
    this.titleService.title$.subscribe((title: string) => {
      this.title = title
    })
    this.router.events.pipe(
      filter((event: Event | RouterEvent) => event instanceof NavigationEnd),
    ).subscribe((event) => {
      this.isReturnable = event.url.split('/').at(-1) !== 'user'
    })
  }
  navigateBack(){
    this.location.back()
  }
}
