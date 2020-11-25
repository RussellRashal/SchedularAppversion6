
import { EditUserService } from './../../_services/editUser.service';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, MinLengthValidator, ValidatorFn, Validators } from '@angular/forms';
import { StateStorageService } from 'src/app/_services/stateStorage.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'app-passwordReset',
  templateUrl: './passwordReset.component.html',
  styleUrls: ['./passwordReset.component.css']
})
export class PasswordResetComponent implements OnInit {
  userMemberModels;
  ResetPass: FormGroup;
  model: any = {};
  nullError: boolean;
  confirmError: boolean;

  constructor(
    private stateStorageService: StateStorageService,
    private editUserService: EditUserService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.userMemberModels = data['UserMemberModel'];
    }, error => {
      console.log(error);
    });
    this.initForm();
  }
  updatePassword() {

    this.nullError = false;
    this.confirmError = false;

    if (this.ResetPass.value.username === '' ||
      this.ResetPass.value.newPassword === '' ||
      this.ResetPass.value.confirmPassword === ''){
        this.nullError = true;
    }
    else if (this.ResetPass.value.newPassword !==
      this.ResetPass.value.confirmPassword) {
      alert('password does not match');
    }
    else {
      this.model = {
        username: this.ResetPass.value.username,
        newPassword: this.ResetPass.value.newPassword
      };

      this.editUserService.editAnyPasswordAdmin(this.model).subscribe(next => {
        alert(next);
      }, error => {
        alert(error.error);
      });
    }
  }



  initForm() {
    this.ResetPass = new FormGroup({
      username: new FormControl(''),
      newPassword: new FormControl('',
      [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(8)
      ]),
      confirmPassword: new FormControl('')
    });
  }

  getNullError() {
    return this.nullError;
  }

}




