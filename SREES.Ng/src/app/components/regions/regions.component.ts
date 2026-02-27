import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RegionService } from '../../services/region.service';
import { Region, CreateRegionRequest, UpdateRegionRequest, RegionFilterRequest } from '../../models/region.model';
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
  Math = Math;

  // Pagination state
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  pages: number[] = [];

  // Filter state
  filterRequest: RegionFilterRequest = {
    pageNumber: 1,
    pageSize: 10
  };

  appliedFilters: { key: string; label: string; value: string }[] = [];

  regionForm: CreateRegionRequest = {
    name: '',
    latitude: 0,
    longitude: 0
  };

  constructor(private regionService: RegionService) {}

  ngOnInit() {
    this.loadRegionsFiltered();
  }

  loadRegionsFiltered() {
    this.filterRequest.pageNumber = this.currentPage;
    this.filterRequest.pageSize = this.pageSize;

    this.regionService.getFiltered(this.filterRequest).subscribe(response => {
      if (response.data) {
        this.regions = response.data.data;
        this.totalCount = response.data.totalCount;
        this.totalPages = response.data.totalPages;
        this.pages = this.generatePages();
      }
    });
  }

  applyFilters() {
    this.currentPage = 1;
    this.updateAppliedFilters();
    this.loadRegionsFiltered();
  }

  resetFilters() {
    this.filterRequest = { pageNumber: 1, pageSize: this.pageSize };
    this.appliedFilters = [];
    this.currentPage = 1;
    this.loadRegionsFiltered();
  }

  removeFilter(key: string) {
    switch (key) {
      case 'searchTerm': this.filterRequest.searchTerm = undefined; break;
      case 'dateFrom': this.filterRequest.dateFrom = undefined; break;
      case 'dateTo': this.filterRequest.dateTo = undefined; break;
    }
    this.applyFilters();
  }

  updateAppliedFilters() {
    this.appliedFilters = [];
    if (this.filterRequest.searchTerm)
      this.appliedFilters.push({ key: 'searchTerm', label: 'Search', value: this.filterRequest.searchTerm });
    if (this.filterRequest.dateFrom)
      this.appliedFilters.push({ key: 'dateFrom', label: 'From', value: this.filterRequest.dateFrom });
    if (this.filterRequest.dateTo)
      this.appliedFilters.push({ key: 'dateTo', label: 'To', value: this.filterRequest.dateTo });
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadRegionsFiltered();
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadRegionsFiltered();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadRegionsFiltered();
    }
  }

  generatePages(): number[] {
    const pages: number[] = [];
    const delta = 2;
    const start = Math.max(1, this.currentPage - delta);
    const end = Math.min(this.totalPages, this.currentPage + delta);
    for (let i = start; i <= end; i++) pages.push(i);
    return pages;
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
        this.loadRegionsFiltered();
        this.closeModal();
      });
    } else {
      this.regionService.create(this.regionForm).subscribe(() => {
        this.loadRegionsFiltered();
        this.closeModal();
      });
    }
  }

  deleteRegion() {
    if (this.selectedRegion) {
      this.regionService.delete(this.selectedRegion.id).subscribe(() => {
        this.loadRegionsFiltered();
        this.closeModal();
      });
    }
  }
}

