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
  showModal = false;
  showDeleteModal = false;
  isEdit = false;
  selectedOutage: Outage | null = null;
  selectedStatus: OutageStatus = 'Reported';
  
  outageForm: CreateOutageRequest = {
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
    this.outageService.getAllOutages().subscribe({
      next: (response) => {
        this.outages = response.data;
      },
      error: (error) => {
        console.error('Error loading outages:', error);
      }
    });
  }

  openModal(): void {
    this.isEdit = false;
    this.outageForm = {
      userId: 0,
      regionId: 0,
      description: ''
    };
    this.showModal = true;
  }

  openEditModal(outage: Outage): void {
    this.isEdit = true;
    this.selectedOutage = outage;
    this.selectedStatus = outage.outageStatus;
    this.outageForm = {
      userId: outage.userId,
      regionId: outage.regionId,
      description: outage.description
    };
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.selectedOutage = null;
  }

  saveOutage(): void {
    if (this.isEdit && this.selectedOutage) {
      // Update status if changed
      if (this.selectedStatus !== this.selectedOutage.outageStatus) {
        this.outageService.updateOutageStatus(this.selectedOutage.id, { newStatus: this.selectedStatus }).subscribe({
          next: () => {
            this.loadOutages();
          },
          error: (error) => console.error('Error updating status:', error)
        });
      }
      this.closeModal();
    } else {
      // Create new outage
      this.outageService.createOutage(this.outageForm).subscribe({
        next: () => {
          this.loadOutages();
          this.closeModal();
        },
        error: (error) => console.error('Error creating outage:', error)
      });
    }
  }

  openDeleteModal(outage: Outage): void {
    this.selectedOutage = outage;
    this.showDeleteModal = true;
  }

  closeDeleteModal(): void {
    this.showDeleteModal = false;
    this.selectedOutage = null;
  }

  confirmDelete(): void {
    if (this.selectedOutage) {
      this.outageService.deleteOutage(this.selectedOutage.id).subscribe({
        next: () => {
          this.loadOutages();
          this.closeDeleteModal();
        },
        error: (error) => console.error('Error deleting outage:', error)
      });
    }
  }
}