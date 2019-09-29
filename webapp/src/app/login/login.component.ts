import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {ClrLoadingState} from '@clr/angular';
import {LoginService} from '../services/login.service';
import {Router} from '@angular/router';
import {LoginResponse} from '../models/LoginResponse';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });
  public notFound : boolean = false;
  public isLoading : ClrLoadingState = ClrLoadingState.DEFAULT;

  constructor(public loginService : LoginService,
              public router: Router) { }

  ngOnInit() {

  }

  login(){
    this.isLoading = ClrLoadingState.LOADING;
    this.loginService.login(
      this.loginForm.value.username, this.loginForm.value.password
    ).subscribe(
      (response: LoginResponse) => {
        this.notFound = false;
        this.loginService.token = response.access_token;
        this.router.navigate(['']);
        this.isLoading = ClrLoadingState.DEFAULT;
      },
      error => {
        console.log(error);
        this.notFound = true;
        this.isLoading = ClrLoadingState.DEFAULT
      },

    )
  }
}
