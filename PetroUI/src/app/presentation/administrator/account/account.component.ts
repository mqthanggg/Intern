import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { TitleService } from '../../../infrastructure/services/title.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { UserRecord, UserRecordWithPage } from './user-record';
import { NgClass } from '@angular/common';
import { FormControl, ReactiveFormsModule, UntypedFormGroup, Validators } from '@angular/forms';
import {  Router } from '@angular/router';
import { debounceTime } from 'rxjs';
import { AccountService } from '../../../infrastructure/services/account.service';

@Component({
  selector: 'app-account',
  standalone: true,
  imports: [NgClass,ReactiveFormsModule],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css'
})
export class AccountComponent implements OnInit, OnChanges{
  @Input() page: string = ''
  userList: UserRecord[] = []
  pages: number = 0
  selectedUserId: number = 0
  isEditDisplay: boolean = false
  isRemoveDisplay: boolean = false
  isSignUpDisplay: boolean = false
  isRoleDisplay: boolean = false
  isActiveDisplay: boolean = false
  pageForm: FormControl = new FormControl<number>(0, [Validators.required])
  editForm: UntypedFormGroup = new UntypedFormGroup({
    oldPassword: new FormControl('',[Validators.required]),
    newPassword: new FormControl('',[Validators.minLength(8),Validators.required]),
    reTypePassword: new FormControl('',[Validators.minLength(8),Validators.required])
  })
  signUpForm: UntypedFormGroup = new UntypedFormGroup({
    username: new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z0-9_.@]+$/)]),
    password: new FormControl('', [Validators.required, Validators.minLength(8)]),
    reTypePassword: new FormControl('', [Validators.required, Validators.minLength(8)]),
    role: new FormControl(1, [Validators.required])
  })
  filterForm: UntypedFormGroup = new UntypedFormGroup({
    username: new FormControl<string | null>(null,[Validators.pattern(/^[a-zA-Z0-9_.@]+$/)]),
    role: new FormControl<number | null>(null),
    active: new FormControl<number | null>(null)
  })
  constructor(
    private titleService: TitleService,
    private http: HttpClient,
    private router: Router,
    private filter: AccountService
  ){
    setTimeout(() => {
      this.titleService.updateTitle("Account")
    },0)
  }
  findUsernameById = () => {
    let foundUser = this.userList.find((value) => value.userId == this.selectedUserId)
    return foundUser ? foundUser.username : ""
  } 
  ngOnInit(): void {
    const currentFilter = this.filter.getFilter()
    currentFilter ? this.filterForm.setValue(
      currentFilter
    ) : {}    
    this.pageForm.setValue(
      this.getPage()
    )
    this.pageForm.valueChanges.pipe(
      debounceTime(1000)
    ).
    subscribe({
      next: (value) => {
        if (value > this.pages || value < 0){
          this.pageForm.setValue(this.page, {emitEvent: false})
        }
        else
          this.router.navigate(['/administrator/account',value])
      }
    })
    this.filterForm.valueChanges.pipe(
      debounceTime(1000)
    ).
    subscribe({
      next: (value) => {
        if (value.username == '')
          value.username = null
        this.filter.updateFilter(value)
        if (this.getPage() == 1){
          this.http.post<UserRecordWithPage>(
            environment.serverURI + `/accounts/${this.page}`,
            this.filter.getFilter(),
            {
              observe: 'response',
              withCredentials: true
            }
          ).subscribe({
            next: (res) => {
              this.userList = res.body?.users ?? []
              this.pages = res.body?.pageNumber ?? 0
            },
            error: (err) => {
              console.error(err);
            }
          })
        }
        else
          this.router.navigate(['/administrator/account',1])
      }
    })
    this.http.post<UserRecordWithPage>(
      environment.serverURI + `/accounts/${this.page}`,
      this.filter.getFilter(),
      {
        observe: 'response',
        withCredentials: true
      }
    ).subscribe({
      next: (res) => {
        this.userList = res.body?.users ?? []
        this.pages = res.body?.pageNumber ?? 0
      },
      error: (err) => {
        console.error(err);
      }
    })
  }
  editFormSubmit(){
    if (
      this.editForm.get('reTypePassword')?.value !=
      this.editForm.get('newPassword')?.value
    )
      this.editForm.setErrors({
        passwordMismatch: true
      })
    else
      this.http.put(
        environment.serverURI + `/account/${this.selectedUserId}`,
        this.editForm.value,
        {
          observe: 'response',
          withCredentials: true
        }
      ).subscribe({
        next: (_) => {
          window.location.reload()
        },
        error: (err) => {
          console.error(err);
        }
      })
  }
  removeSubmit(){
    this.http.delete(
      environment.serverURI + `/account/${this.selectedUserId}`,
      {
        observe: "response"
      }
    ).
    subscribe({
      next: (_) => {
        window.location.reload()
      },
      error: (err) => {
        console.error(err);
      }
    })
  }
  signUpSubmit(){
    if (
      this.signUpForm.get('password')?.value != this.signUpForm.get('reTypePassword')?.value
    ){
      this.signUpForm.setErrors({
        passwordMismatch: true
      })
    }
    else {
      this.http.post(
        environment.serverURI + '/account',
        this.signUpForm.value,
        {
          observe: "response",
          withCredentials: true
        }
      ).
      subscribe({
        next: (_) => {
          window.location.reload()
        },
        error: (err) => {
          console.error(err);
        }
      })
    }
  }
  toggleEdit(id: number){
    this.selectedUserId = id
    this.isEditDisplay = true
    this.isRemoveDisplay = this.isSignUpDisplay = false
  }
  closeDialog(){
    this.selectedUserId = 0
    this.isEditDisplay = this.isRemoveDisplay = this.isSignUpDisplay = false
  }
  toggleRemove(id: number){
    this.selectedUserId = id
    this.isRemoveDisplay = true
    this.isEditDisplay = this.isSignUpDisplay = false
  }
  toggleSignUp(){
    this.isSignUpDisplay = true
    this.isEditDisplay = this.isRemoveDisplay = false
  }
  goNextPage(){
    if (this.getPage()+1 <= this.pages)
      this.router.navigate(['/administrator/account',this.getPage()+1])
  }
  goPreviousPage(){
    if (this.getPage()-1 >= 0)
      this.router.navigate(['/administrator/account',this.getPage()-1]) 
  }
  goEnd(){
    this.router.navigate(['administrator/account', this.pages])
  }
  goBegin(){
    this.router.navigate(['administrator/account', 1])
  }
  getPage(){
    return Number(this.page)
  }
  ngOnChanges(changes: SimpleChanges): void {
    this.http.post<UserRecordWithPage>(
      environment.serverURI + `/accounts/${changes['page'].currentValue}`,
      this.filter.getFilter(),
      {
        observe: 'response',
        withCredentials: true
      }
    ).subscribe({
      next: (res) => {
        this.userList = res.body?.users ?? []
        this.pages = res.body?.pageNumber ?? 0
      },
      error: (err) => {
        console.error(err);
      }
    })
    this.pageForm.setValue(changes['page'].currentValue)
  }
  toggleRole(){
    this.isRoleDisplay = !this.isRoleDisplay
  }
  changeRole(value: number | null){
    this.isRoleDisplay = false
    this.filterForm.get('role')?.setValue(value)
  }
  toggleActive(){
    this.isActiveDisplay = !this.isActiveDisplay
  }
  changeActive(value: number | null){
    this.isActiveDisplay = false
    this.filterForm.get('active')?.setValue(value)
  }
}
