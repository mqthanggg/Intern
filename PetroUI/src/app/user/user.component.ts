import { NgClass } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { TitleService } from './title.service';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [RouterOutlet, NgClass, RouterLink],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit{
  isNavExtended = true;
  title: string = "";
  ngOnInit(): void {
    this.title = "Dashboard"
  }
  constructor(private titleService: TitleService){
    this.titleService.title$.subscribe((title: string) => {
      this.title = title
    })
  }
}
