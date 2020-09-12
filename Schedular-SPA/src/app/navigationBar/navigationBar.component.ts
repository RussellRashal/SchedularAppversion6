import { AddTaskComponent } from './../addTask/addTask.component';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { UpdateTaskComponent } from '../updateTask/updateTask.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigationBar.component.html',
  styleUrls: ['./navigationBar.component.css']
})
export class NavigationBarComponent implements OnInit {
  model: any = {};
  loginReactiveForm: FormGroup;


  constructor(
    private authService: AuthService,
    public dialog: MatDialog,
    private router: Router) { }

  ngOnInit() {
    this.initForm();
  }

  login() {
    this.model.username = this.loginReactiveForm.value.username;
    this.model.password = this.loginReactiveForm.value.password;


    this.authService.login(this.model).subscribe(next => {
      this.router.navigateByUrl('/CalendarView');
    }, error => {
      console.log('failed to login');
    });
  }

  initForm() {
    this.loginReactiveForm = new FormGroup({
      username: new FormControl(),
      password: new FormControl()
    });
  }

  // has the user logged in
  loggedIn() {
    return this.authService.loggedIn();
  }

  loggedOut() {
    const token = localStorage.removeItem('token');
    const username = localStorage.removeItem('user');
    const userId = localStorage.removeItem('role');
  }



  openDialogUpdateTask(action) {
    const dialogRef = this.dialog.open(UpdateTaskComponent, {
      width: '80%',
      height: '60%'
    });
  }

}
