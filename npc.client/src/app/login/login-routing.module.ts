import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';

const routes: Routes = [

  { path: "", component: LoginComponent },
  { path: 'home', loadChildren: () => import("../home/home.module").then(m => m.HomeModule) },
  { path: 'register', loadChildren: () => import("../register/register.module").then(m => m.RegisterModule) },
  { path: 'passwordreset', loadChildren: () => import("../passwordreset/passwordreset.module").then(m => m.PasswordresetModule) }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LoginRoutingModule { }
