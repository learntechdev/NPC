import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { SidebarModule } from 'primeng/sidebar';
import { HomeRoutingModule } from './home-routing.module';
import { LayoutComponent } from './layout/layout.component';
@NgModule({
  declarations: [
    LayoutComponent,
   
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    SidebarModule,
    ButtonModule,
    AvatarModule

  ]
})
export class HomeModule { }
