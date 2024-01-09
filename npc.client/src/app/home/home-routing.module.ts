import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';

const routes: Routes = [
  {
    path: "", component: LayoutComponent,
    children: [
      { path: "incident", loadChildren: () => import('../home/layout/incident/incident.module').then(m => m.IncidentModule) },
      { path: "operations", loadChildren: () => import('../home/layout/operations/operations.module').then(m => m.OperationsModule) },
      { path: "reports", loadChildren: () => import('../home/layout/reports/reports.module').then(m => m.ReportsModule) }     
    ]
  },
  {
    path: "login", loadChildren: () => import('../login/login.module').then(m => m.LoginModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
