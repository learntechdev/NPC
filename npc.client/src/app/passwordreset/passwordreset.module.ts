import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PasswordresetRoutingModule } from './passwordreset-routing.module';
import { PasswordresetComponent } from './passwordreset/passwordreset.component';


@NgModule({
  declarations: [
    PasswordresetComponent
  ],
  imports: [
    CommonModule,
    PasswordresetRoutingModule
  ]
})
export class PasswordresetModule { }
