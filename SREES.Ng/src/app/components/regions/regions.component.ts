import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RegionService } from '../../services/region.service';
import { Region, CreateRegionRequest, UpdateRegionRequest } from '../../models/region.model';
import { ApiResponse } from '../../models/region.model';

@Component({
  selector: 'app-regions',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './regions.component.html',
  styleUrls: ['./regions.component.scss']
})
export class RegionsComponent implements OnInit {
  regions: Region[] = [];
  showModal = false;
  showDeleteModal = false;
  isEdit = false;
  selectedRegion: Region | null = null;
  
  regionForm: CreateRegionRequest = {
    name: '',
    latitude: 0,
    longitude: 0
  };

  constructor(private regionService: RegionService) {}

  ngOnInit() {
    this.loadRegions();
  }

  loadRegions() {
    this.regionService.getAll().subscribe(response => {
      this.regions = response.data;
    });
  }

  openCreateModal() {
    this.isEdit = false;
    this.regionForm = { name: '', latitude: 0, longitude: 0 };
    this.showModal = true;
  }

  openEditModal(region: Region) {
    this.isEdit = true;
    this.selectedRegion = region;
    this.regionForm = {
      name: region.name,
      latitude: region.latitude,
      longitude: region.longitude
    };
    this.showModal = true;
  }

  openDeleteModal(region: Region) {
    this.selectedRegion = region;
    this.showDeleteModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.showDeleteModal = false;
    this.selectedRegion = null;
  }

  saveRegion() {
    if (this.isEdit && this.selectedRegion) {
      this.regionService.update(this.selectedRegion.id, this.regionForm).subscribe(() => {
        this.loadRegions();
        this.closeModal();
      });
    } else {
      this.regionService.create(this.regionForm).subscribe(() => {
        this.loadRegions();
        this.closeModal();
      });
    }
  }

  deleteRegion() {
    if (this.selectedRegion) {
      this.regionService.delete(this.selectedRegion.id).subscribe(() => {
        this.loadRegions();
        this.closeModal();
      });
    }
  }
}