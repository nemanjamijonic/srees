import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SubstationService } from '../../services/substation.service';
import { RegionService } from '../../services/region.service';
import { Substation, CreateSubstationRequest, UpdateSubstationRequest, SubstationFilterRequest } from '../../models/substation.model';
import { RegionSelectOption } from '../../models/region-select.model';

@Component({
  selector: 'app-substations',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './substations.component.html',
  styleUrls: ['./substations.component.scss']
})
export class SubstationsComponent implements OnInit {
  substations: Substation[] = [];
  regionOptions: RegionSelectOption[] = [];
  showModal = false;
  showDeleteModal = false;
  isEdit = false;
  selectedSubstation: Substation | null = null;
  Math = Math;

  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  pages: number[] = [];

  // Filter
  filterRequest: SubstationFilterRequest = {
    pageNumber: 1,
    pageSize: 10
  };

  appliedFilters: { key: string; label: string; value: string }[] = [];

  substationForm: CreateSubstationRequest = {
    name: '',
    substationType: -1,
    latitude: 0,
    longitude: 0,
    regionId: 0
  };

  constructor(
    private substationService: SubstationService,
    private regionService: RegionService
  ) {}

  ngOnInit() {
    this.loadSubstationsFiltered();
    this.loadRegionOptions();
  }

  loadSubstationsFiltered() {
    this.filterRequest.pageNumber = this.currentPage;
    this.filterRequest.pageSize = this.pageSize;

    this.substationService.getFiltered(this.filterRequest).subscribe(response => {
      if (response.data) {
        this.substations = response.data.data;
        this.totalCount = response.data.totalCount;
        this.totalPages = response.data.totalPages;
        this.pages = this.generatePages();
      }
    });
  }

  loadRegionOptions() {
    this.regionService.getAllForSelect().subscribe(response => {
      this.regionOptions = response.data;
    });
  }

  applyFilters() {
    this.currentPage = 1;
    this.updateAppliedFilters();
    this.loadSubstationsFiltered();
  }

  resetFilters() {
    this.filterRequest = { pageNumber: 1, pageSize: this.pageSize };
    this.appliedFilters = [];
    this.currentPage = 1;
    this.loadSubstationsFiltered();
  }

  removeFilter(key: string) {
    switch (key) {
      case 'searchTerm': this.filterRequest.searchTerm = undefined; break;
      case 'substationType': this.filterRequest.substationType = undefined; break;
      case 'dateFrom': this.filterRequest.dateFrom = undefined; break;
      case 'dateTo': this.filterRequest.dateTo = undefined; break;
    }
    this.applyFilters();
  }

  updateAppliedFilters() {
    this.appliedFilters = [];
    if (this.filterRequest.searchTerm)
      this.appliedFilters.push({ key: 'searchTerm', label: 'Search', value: this.filterRequest.searchTerm });
    if (this.filterRequest.substationType !== undefined && this.filterRequest.substationType !== null)
      this.appliedFilters.push({ key: 'substationType', label: 'Type', value: this.getSubstationTypeName(this.filterRequest.substationType) });
    if (this.filterRequest.dateFrom)
      this.appliedFilters.push({ key: 'dateFrom', label: 'From', value: this.filterRequest.dateFrom });
    if (this.filterRequest.dateTo)
      this.appliedFilters.push({ key: 'dateTo', label: 'To', value: this.filterRequest.dateTo });
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadSubstationsFiltered();
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadSubstationsFiltered();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadSubstationsFiltered();
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
    this.substationForm = { name: '', substationType: -1, latitude: 0, longitude: 0, regionId: 0 };
    this.showModal = true;
  }

  openEditModal(substation: Substation) {
    this.isEdit = true;
    this.selectedSubstation = substation;
    this.substationForm = {
      name: substation.name,
      substationType: substation.substationType,
      latitude: substation.latitude,
      longitude: substation.longitude,
      regionId: substation.regionId
    };
    this.showModal = true;
  }

  openDeleteModal(substation: Substation) {
    this.selectedSubstation = substation;
    this.showDeleteModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.showDeleteModal = false;
    this.selectedSubstation = null;
  }

  saveSubstation() {
    const substationTypeNumber = +this.substationForm.substationType;
    const regionIdNumber = +this.substationForm.regionId;

    if (substationTypeNumber < 0 || regionIdNumber <= 0) {
      alert('Please select valid Substation Type and Region');
      return;
    }

    const requestData = { ...this.substationForm, substationType: substationTypeNumber, regionId: regionIdNumber };

    if (this.isEdit && this.selectedSubstation) {
      this.substationService.update(this.selectedSubstation.id, requestData).subscribe(() => {
        this.loadSubstationsFiltered();
        this.closeModal();
      });
    } else {
      this.substationService.create(requestData).subscribe(() => {
        this.loadSubstationsFiltered();
        this.closeModal();
      });
    }
  }

  deleteSubstation() {
    if (this.selectedSubstation) {
      this.substationService.delete(this.selectedSubstation.id).subscribe(() => {
        this.loadSubstationsFiltered();
        this.closeModal();
      });
    }
  }

  getRegionName(regionId: number): string {
    const region = this.regionOptions.find(r => r.id === regionId);
    return region ? region.name : 'N/A';
  }

  getSubstationTypeName(substationType: number): string {
    switch (substationType) {
      case 0: return 'Transmission';
      case 1: return 'Injection';
      case 2: return 'Distribution';
      case 3: return 'Bulk Supply';
      default: return 'Unknown';
    }
  }

  getSubstationTypeClass(substationType: number): string {
    switch (substationType) {
      case 0: return 'bg-transmission';
      case 1: return 'bg-injection';
      case 2: return 'bg-distribution';
      case 3: return 'bg-bulk';
      default: return 'bg-secondary';
    }
  }
}
