import { Location, NgClass } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, Event, RouterOutlet, RouterEvent, NavigationEnd, RouterLink } from '@angular/router';
import { TitleService } from '../../infrastructure/services/title.service';
import { filter } from 'rxjs';
import { LogoutService } from '../../infrastructure/services/logout.service';

@Component({
  selector: 'app-administrator',
  standalone: true,
  imports: [NgClass, RouterOutlet, RouterLink],
  templateUrl: './administrator.component.html',
  styleUrl: './administrator.component.css'
})
export class AdministratorComponent implements OnInit{
  isNavExtended = true;
  isReturnable = false;
  title: string = "";
  logout: () => Promise<boolean>;
  ngOnInit(): void {
    this.title = "Dashboard"
  }
  constructor(
    private titleService: TitleService, 
    private router:Router, 
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
      this.isReturnable = event.url.split('/').at(-1) !== 'administrator'
    })
  }
  navigateBack(){
    this.location.back()
  }
}
