import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
// import { LoginComponent } from './components/auth/login/login.component';
// import { RegisterComponent } from './components/auth/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { NetworkMapComponent } from './components/network-map/network-map.component';
import { OutagesComponent } from './components/outages/outages.component';
import { RegionsComponent } from './components/regions/regions.component';
import { SubstationsComponent } from './components/substations/substations.component';
import { PolesComponent } from './components/poles/poles.component';
import { BuildingsComponent } from './components/buildings/buildings.component';
import { FeedersComponent } from './components/feeders/feeders.component';
import { CustomersComponent } from './components/customers/customers.component';
import { LayoutComponent } from './components/layout/layout.component';

const routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' as const },
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
      { path: 'crud/buildings', component: BuildingsComponent },
      { path: 'crud/feeders', component: FeedersComponent },
      { path: 'crud/customers', component: CustomersComponent }
    ]
  },
  { path: '**', redirectTo: '/dashboard' }
];

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    provideHttpClient()
  ]
};
