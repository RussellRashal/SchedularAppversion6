import { NumberClosedTasksByUserComponent } from './ReportFolder/numberClosedTasksByUser/numberClosedTasksByUser.component';
import { HighPriorityComponent } from './ReportFolder/HighPriority/HighPriority.component';
import { HoursWorkedComponent } from './ReportFolder/HoursWorked/HoursWorked.component';
import { CustomerComponent } from './CustomerFolder/Customer/Customer.component';
import { EnableDisableAccountComponent } from './AdminConsole/enableDisableAccount/enableDisableAccount.component';
import { PasswordResetComponent } from './AdminConsole/passwordReset/passwordReset.component';
import { AdminConsoleNavComponent } from './AdminConsole/adminConsole-nav/adminConsole-nav.component';
import { JwtModule } from '@auth0/angular-jwt';
// full calendar.io packages
import { FullCalendarModule } from '@fullcalendar/angular'; // the main connector. must go first
import dayGridPlugin from '@fullcalendar/daygrid'; // a plugin
import interactionPlugin from '@fullcalendar/interaction'; // a plugin

// my components
import { AppRoutingModule } from './app-routing.module';
import { CalendarViewComponent } from './CalendarView/CalendarView.component';
import { TestTaskComponent } from './TestTask/TestTask.component';
import { NavigationBarComponent } from './navigationBar/navigationBar.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { AddTaskComponent } from './addTask/addTask.component';

// angular material
import {MatSelectModule} from '@angular/material/select';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatNativeDateModule} from '@angular/material/core';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatInputModule} from '@angular/material/input';
import {MatTableModule} from '@angular/material/table';
import { TimePickerComponent } from './timePicker/timePicker.component';

import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import {MatButtonToggleModule} from '@angular/material/button-toggle';

// other angular components
import { RouterModule } from '@angular/router';
import { DatePipe, CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { appRoutes } from './routes';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { UpdateTaskComponent } from './updateTask/updateTask.component';
import { LoginPageComponent } from './loginPage/loginPage.component';
import { stateStorageResolver } from './_resolvers/state-storage.resolver';
import { UserLoaderResolver } from './_resolvers/user-loader.resolver';
import { NoteComponent } from './note/note.component';
import { taskScheduleResolver } from './_resolvers/taskSchedule.resolver';
import { RegisterComponent } from './AdminConsole/register/register.component';
import { EditUsernameComponent } from './AdminConsole/editUsername/editUsername.component';
import { EditRoleComponent } from './AdminConsole/editRole/editRole.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { ViewTasksComponent } from './viewTasks/viewTasks.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AttachmentComponent } from './Attachment/Attachment.component';
import { FileUploadModule } from 'ng2-file-upload';
import { StandardUserPasswordComponent } from './StandardUserPassword/StandardUserPassword.component';
import { UnlockAccountComponent } from './AdminConsole/unlockAccount/unlockAccount.component';
import { SidebarNavigationComponent } from './sidebar-navigation/sidebar-navigation.component';
import { EditCustomerComponent } from './CustomerFolder/editCustomer/editCustomer.component';
import { AddCustomerComponent } from './CustomerFolder/AddCustomer/AddCustomer.component';
import { CustomerTasksComponent } from './ReportFolder/CustomerTasks/CustomerTasks.component';
import { ChartsModule } from 'ng2-charts';
import { Autosize } from 'ng-autosize';
import {AutosizeModule} from 'ngx-autosize';
import { MatNavComponent } from './mat-nav/mat-nav.component';


// register FullCalendar plugins
FullCalendarModule.registerPlugins([
  dayGridPlugin,
  interactionPlugin
]);

// needed to send the token back to the api server for authentication
export function tokengetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavigationBarComponent,
    NumberClosedTasksByUserComponent,
    CalendarViewComponent,
    HighPriorityComponent,
    TestTaskComponent,
    AddTaskComponent,
    TestTaskComponent,
    SidebarComponent,
    AddCustomerComponent,
    TimePickerComponent,
    HoursWorkedComponent,
    UpdateTaskComponent,
    CustomerComponent,
    LoginPageComponent,
    UnlockAccountComponent,
    EditCustomerComponent,
    RegisterComponent,
    NoteComponent,
    EditUsernameComponent,
    EditRoleComponent,
    EnableDisableAccountComponent,
    StandardUserPasswordComponent,
    AdminConsoleNavComponent,
    PasswordResetComponent,
    ViewTasksComponent,
    AttachmentComponent,
    AttachmentComponent,
    SidebarNavigationComponent,
    CustomerComponent,
    CustomerTasksComponent,
    CustomerComponent,
    MatNavComponent,
   ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatButtonToggleModule,
    FormsModule,
    CommonModule,
    BrowserAnimationsModule,
    HttpClientModule,
    NgbModule,
    MatSidenavModule,
    ChartsModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatTableModule,
    AutosizeModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
    BrowserModule,
    AppRoutingModule,
    FileUploadModule,
    FullCalendarModule, // register FullCalendar with you app
    ReactiveFormsModule,
    PaginationModule.forRoot(),
    RouterModule.forRoot(appRoutes),
    JwtModule.forRoot({
      config: {
         tokenGetter: tokengetter,
         whitelistedDomains: ['localhost:5000'],
         blacklistedRoutes: ['localhost:5000/api/auth']
      }
   }),
    LayoutModule,
    MatToolbarModule,
    MatIconModule,
    MatListModule,
  ],
  exports: [
    PaginationModule,
    FileUploadModule
  ],
  providers: [
    UserLoaderResolver,
    stateStorageResolver,
    taskScheduleResolver,
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
