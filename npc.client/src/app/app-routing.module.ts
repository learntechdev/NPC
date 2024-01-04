import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [

  { path: "login", loadChildren: () => import('../app/login/login.module').then(m => m.LoginModule) },
  { path: "register", loadChildren: () => import('../app/register/register.module').then(m => m.RegisterModule) },
  { path: "", loadChildren: () => import('../app/register/register.module').then(m => m.RegisterModule) }


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
