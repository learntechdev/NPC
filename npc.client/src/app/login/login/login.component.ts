import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  selectedLoginType!: string;
  constructor(private router: Router) { }
  GotoHome() {
    this.router.navigate(['/home/incident']);
  }
}
