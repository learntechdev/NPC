import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OperationsRoutingModule } from './operations-routing.module';
import { OperationsComponent } from './operations/operations.component';


@NgModule({
  declarations: [
    OperationsComponent
  ],
  imports: [
    CommonModule,
    OperationsRoutingModule
  ]
})
export class OperationsModule { }
