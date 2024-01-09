import { Component, ViewChild } from '@angular/core';
import { Sidebar } from 'primeng/sidebar';
import { Router } from '@angular/router';
@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent {
  @ViewChild('sidebarRef') sidebarRef!: Sidebar;
  sidebarVisible: boolean = false;
  constructor(private router: Router) { }


  closeCallback(e: any): void {

    this.sidebarRef.close(e);
  }
  Gotopage(page: string) {

    this.router.navigate([page]);

    this.sidebarVisible = !this.sidebarVisible;

  }

}
