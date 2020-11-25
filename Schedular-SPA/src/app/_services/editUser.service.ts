import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EditUserService {
  baseUrl = environment.apiUrl + 'Auth/';


  constructor(
    private http: HttpClient) { }

  editRole(userRole) {
      return this.http.put(this.baseUrl + 'editRoles', userRole, { responseType: 'text' });
  }

  putEditName(userEdit) {
    return this.http.put(this.baseUrl + 'editName', userEdit, { responseType: 'text' });
  }

  editAnyPasswordAdmin(editUser: any) {
      return this.http.put(this.baseUrl + 'adminPasswordReset' , editUser, { responseType: 'text' });
  }

  editStandardPassword(editUser: any): Observable<any> {
    return this.http.put<any>(this.baseUrl + 'standardPasswordReset', editUser );
  }

  unlockAccount(editUser: any) {
    return this.http.put(this.baseUrl + 'unlockAccount', editUser, {responseType: 'text'});
  }

  enableAccount(editUser: any) {
    return this.http.put(this.baseUrl + 'enableAccount', editUser, {responseType: 'text'});
  }

  disableAccount(editUser: any) {
    return this.http.put(this.baseUrl + 'disableAccount', editUser, {responseType: 'text'});
  }

  allEnabledAccounts(): Observable<any[]>  {
    return this.http.get<any[]>(this.baseUrl + 'allEnabledAccounts');
  }

  allDisabledAccounts(): Observable<any[]>  {
    return this.http.get<any[]>(this.baseUrl + 'allDisabledAccounts');
  }

}
