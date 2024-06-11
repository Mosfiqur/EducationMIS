import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpClientService } from '../../../services/httpClientService'
import { AuthService } from 'src/app/services/auth.service';
import { AppUser } from 'src/app/models/auth/AppUser';
import { AppUserAuth } from 'src/app/models/auth/AppUserAuth';
import { ModalService } from 'src/app/services/modal.service';
import { ForgotPasswordComponent } from 'src/app/shared/components/forgot-password/forgot-password.component';
import { IForgotPasswordModel } from 'src/app/models/auth/forgot-password.model';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  providers: [HttpClientService],
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  password: string = "";
  email: string = "";
  public showErrorMsg: boolean = false;
  user: AppUser = new AppUser();
  userObject: AppUserAuth = null;

  loginForm: FormGroup;

  constructor(
    private router: Router, 
    private formBuilder: FormBuilder, 
    private authService: AuthService,
    private modalService: ModalService,
    private toastrService: ToastrService
    ) {
  }

  get loginFormControl() { return this.loginForm.controls; }

  ngOnInit() {
    this.initValidator();
  }

  goToSignup() {
    this.router.navigate(['authentication/sign-up']);
  }

  initValidator() {
    this.loginForm = this.formBuilder.group({
      password: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(255)]],
      email: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(255), Validators.email]]
    })
  };

  async verifyLogin() {    
    if (this.loginForm.invalid) {      
      return;
    }

    this.user.email = this.loginForm.value.email;
    this.user.password = this.loginForm.value.password;


   this.authService.login(this.user)
      .then((data) => {        
        this.userObject = data;
        this.goToDashboard();
      });

    //return id.toString();

    var headers = new Headers({ 'Content-Type': 'text/plain' });
    var validLogin = "false";


  }


  goToDashboard() {    
    this.router.navigate(['unicef/dashboard/home']);

  }

  forgotPassword(){
    this.modalService.open<ForgotPasswordComponent, IForgotPasswordModel>(ForgotPasswordComponent, {});
  }
}
