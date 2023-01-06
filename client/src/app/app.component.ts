import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'The dating app';
  users: any;

  constructor(private accountService: AccountService) {}
  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userData = localStorage.getItem('user');
    if (!userData) return;

    const user: User = JSON.parse(userData);
    this.accountService.setCurrentUser(user);
  }
}
