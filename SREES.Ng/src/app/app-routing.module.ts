import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NetworkMapComponent } from './components/network-map/network-map.component';
import { OutagesComponent } from './components/outages/outages.component';
import { RegionsComponent } from './components/regions/regions.component';
import { SubstationsComponent } from './components/substations/substations.component';
import { LayoutComponent } from './components/layout/layout.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'network-map', component: NetworkMapComponent },
      { path: 'outages', component: OutagesComponent },
      { path: 'regions', component: RegionsComponent },
      { path: 'substations', component: SubstationsComponent }
    ]
  },
  { path: '**', redirectTo: '/dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }