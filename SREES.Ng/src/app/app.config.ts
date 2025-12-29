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
import { LayoutComponent } from './components/layout/layout.component';

const routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' as const },
  // { path: 'login', component: LoginComponent },
  // { path: 'register', component: RegisterComponent },
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

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    provideHttpClient()
  ]
};
