import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OutageService } from '../../services/outage.service';
import { Outage, CreateOutageRequest, OutageStatus } from '../../models/outage.model';

@Component({
  selector: 'app-outages',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './outages.component.html',
  styleUrls: ['./outages.component.scss']
})
export class OutagesComponent implements OnInit {
  outages: Outage[] = [];
  loading = false;
  showCreateForm = false;
  
  newOutage: CreateOutageRequest = {
    userId: 0,
    regionId: 0,
    description: ''
  };

  statuses: OutageStatus[] = ['Reported', 'Assigned', 'InProgress', 'Resolved'];

  constructor(private outageService: OutageService) { }

  ngOnInit(): void {
    this.loadOutages();
  }

  loadOutages(): void {
    this.loading = true;
    this.outageService.getAllOutages().subscribe({
      next: (response) => {
        console.log('Outages loaded:', response);
        this.outages = response.data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading outages:', error);
        this.loading = false;
      }
    });
  }

  createOutage(): void {
    this.outageService.createOutage(this.newOutage).subscribe({
      next: (response) => {
        this.outages.push(response.data);
        this.resetForm();
      },
      error: (error) => console.error('Error creating outage:', error)
    });
  }

  updateStatus(id: number, event: Event): void {
    const target = event.target as HTMLSelectElement;
    const newStatus = target.value as OutageStatus;
    this.outageService.updateOutageStatus(id, { newStatus }).subscribe({
      next: (response) => {
        const index = this.outages.findIndex(o => o.id === id);
        if (index !== -1) {
          this.outages[index] = response.data;
        }
      },
      error: (error) => console.error('Error updating status:', error)
    });
  }

  deleteOutage(id: number): void {
    if (confirm('Da li ste sigurni da želite da obrišete ovaj prekid?')) {
      this.outageService.deleteOutage(id).subscribe({
        next: () => {
          this.outages = this.outages.filter(o => o.id !== id);
        },
        error: (error) => console.error('Error deleting outage:', error)
      });
    }
  }

  resetForm(): void {
    this.newOutage = { userId: 0, regionId: 0, description: '' };
    this.showCreateForm = false;
  }
}