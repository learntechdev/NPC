import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [

  { path: "login", loadChildren: () => import('../app/login/login.module').then(m => m.LoginModule) },
  { path: "", loadChildren: () => import('../app/login/login.module').then(m => m.LoginModule) }


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
