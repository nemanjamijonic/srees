import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
// import { LoginComponent } from './components/auth/login/login.component';
// import { RegisterComponent } from './components/auth/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NetworkMapComponent } from './components/network-map/network-map.component';
import { OutagesComponent } from './components/outages/outages.component';
import { RegionsComponent } from './components/regions/regions.component';
import { SubstationsComponent } from './components/substations/substations.component';
import { PolesComponent } from './components/poles/poles.component';
import { FeedersComponent } from './components/feeders/feeders.component';
import { LayoutComponent } from './components/layout/layout.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  // { path: 'login', component: LoginComponent },
  // { path: 'register', component: RegisterComponent },
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'network-map', component: NetworkMapComponent },
      { path: 'outages', component: OutagesComponent },
      { path: 'crud/regions', component: RegionsComponent },
      { path: 'crud/substations', component: SubstationsComponent },
      { path: 'crud/poles', component: PolesComponent },
      { path: 'crud/feeders', component: FeedersComponent }
    ]
  },
  { path: '**', redirectTo: '/dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }