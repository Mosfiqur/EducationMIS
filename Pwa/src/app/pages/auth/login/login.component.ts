import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpClientService } from '../../../services/httpClientService'
import { AuthService } from 'src/app/services/auth.service';
import { AppUser } from 'src/app/models/auth/AppUser';
import { AppUserAuth } from 'src/app/models/auth/AppUserAuth';
import { UserDB } from 'src/app/localdb/UserDB';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { ModalService } from 'src/app/services/modal.service';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { ToastrService } from 'ngx-toastr';
import { LoadingSpinnerService } from 'src/app/core/loading-spinner/loading-spinner.service';
import { UnicefDBSchema } from 'src/app/localdb/UnicefDBSchema';

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

  constructor(private router: Router, private formBuilder: FormBuilder,
    private authService: AuthService, private userDb: UserDB, private dbService:NgxIndexedDBService,
    private modalService : ModalService,private toastrService:ToastrService, private loadingSpinnerService:LoadingSpinnerService) {
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
        this.userDb.deleteUser().subscribe(res => {
          this.userDb.saveUser(data).then(result => {
            console.log('Result: ', result);
            this.goToDashboard();
          })
        }, (err) => {
          console.log('Error deleting user: ', err);
        });
      });

    //return id.toString();

    var headers = new Headers({ 'Content-Type': 'text/plain' });
    var validLogin = "false";


  }

  async clearDatabase(){
    if(await this.modalService.confirm("Warning !! Confirmation Message","",MessageConstant.eraseAllData)){
      this.loadingSpinnerService.showLoadingScreen("DeleteDatabase");
      await this.dbService.deleteDatabase().subscribe(async (deleteStatus) => {
        this.toastrService.success(MessageConstant.deleteSuccessful);
        this.loadingSpinnerService.hideLoadingScreen("DeleteDatabase");
        location.reload(true);
      });
    }

  }

  goToDashboard() {
    this.router.navigate(['unicefpwa/dashboard']);
  }
}
