import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PasswordresetComponent } from './passwordreset/passwordreset.component';

const routes: Routes = [{ path: "", component: PasswordresetComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PasswordresetRoutingModule { }
