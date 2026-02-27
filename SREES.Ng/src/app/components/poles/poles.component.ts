import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PoleService } from '../../services/pole.service';
import { RegionService } from '../../services/region.service';
import { Pole, CreatePoleRequest, UpdatePoleRequest, PoleFilterRequest } from '../../models/pole.model';
import { RegionSelectOption } from '../../models/region-select.model';

@Component({
  selector: 'app-poles',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './poles.component.html',
  styleUrls: ['./poles.component.scss']
})
export class PolesComponent implements OnInit {
  poles: Pole[] = [];
  regionOptions: RegionSelectOption[] = [];
  showModal = false;
  showDeleteModal = false;
  isEdit = false;
  selectedPole: Pole | null = null;
  Math = Math;

  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  pages: number[] = [];

  // Filter
  filterRequest: PoleFilterRequest = {
    pageNumber: 1,
    pageSize: 10
  };

  appliedFilters: { key: string; label: string; value: string }[] = [];

  poleForm: CreatePoleRequest = {
    name: '',
    latitude: 0,
    longitude: 0,
    address: '',
    poleType: -1,
    regionId: 0
  };

  constructor(
    private poleService: PoleService,
    private regionService: RegionService
  ) {}

  ngOnInit() {
    this.loadPolesFiltered();
    this.loadRegionOptions();
  }

  loadPolesFiltered() {
    this.filterRequest.pageNumber = this.currentPage;
    this.filterRequest.pageSize = this.pageSize;

    this.poleService.getFiltered(this.filterRequest).subscribe(response => {
      if (response.data) {
        this.poles = response.data.data;
        this.totalCount = response.data.totalCount;
        this.totalPages = response.data.totalPages;
        this.generatePages();
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
    this.loadPolesFiltered();
  }

  resetFilters() {
    this.filterRequest = { pageNumber: 1, pageSize: this.pageSize };
    this.appliedFilters = [];
    this.currentPage = 1;
    this.loadPolesFiltered();
  }

  removeFilter(key: string) {
    (this.filterRequest as any)[key] = undefined;
    this.currentPage = 1;
    this.updateAppliedFilters();
    this.loadPolesFiltered();
  }

  updateAppliedFilters() {
    this.appliedFilters = [];
    if (this.filterRequest.searchTerm) {
      this.appliedFilters.push({ key: 'searchTerm', label: 'Search', value: this.filterRequest.searchTerm });
    }
    if (this.filterRequest.poleType !== undefined && this.filterRequest.poleType !== null) {
      this.appliedFilters.push({ key: 'poleType', label: 'Type', value: this.getPoleTypeName(this.filterRequest.poleType) });
    }
    if (this.filterRequest.dateFrom) {
      this.appliedFilters.push({ key: 'dateFrom', label: 'From', value: this.filterRequest.dateFrom });
    }
    if (this.filterRequest.dateTo) {
      this.appliedFilters.push({ key: 'dateTo', label: 'To', value: this.filterRequest.dateTo });
    }
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadPolesFiltered();
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadPolesFiltered();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadPolesFiltered();
    }
  }

  generatePages() {
    const maxVisible = 5;
    const half = Math.floor(maxVisible / 2);
    let start = Math.max(1, this.currentPage - half);
    let end = Math.min(this.totalPages, start + maxVisible - 1);
    if (end - start < maxVisible - 1) {
      start = Math.max(1, end - maxVisible + 1);
    }
    this.pages = Array.from({ length: end - start + 1 }, (_, i) => start + i);
  }

  openCreateModal() {
    this.isEdit = false;
    this.poleForm = {
      name: '',
      latitude: 0,
      longitude: 0,
      address: '',
      poleType: -1,
      regionId: 0
    };
    this.showModal = true;
  }

  openEditModal(pole: Pole) {
    this.isEdit = true;
    this.selectedPole = pole;
    this.poleForm = {
      name: pole.name,
      latitude: pole.latitude,
      longitude: pole.longitude,
      address: pole.address,
      poleType: pole.poleType,
      regionId: pole.regionId
    };
    this.showModal = true;
  }

  openDeleteModal(pole: Pole) {
    this.selectedPole = pole;
    this.showDeleteModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.showDeleteModal = false;
    this.selectedPole = null;
  }

  savePole() {
    const poleTypeNumber = +this.poleForm.poleType;
    const regionIdNumber = +this.poleForm.regionId;

    if (poleTypeNumber < 0 || regionIdNumber <= 0) {
      alert('Please select valid Pole Type and Region');
      return;
    }

    const requestData = {
      ...this.poleForm,
      poleType: poleTypeNumber,
      regionId: regionIdNumber
    };

    if (this.isEdit && this.selectedPole) {
      this.poleService.update(this.selectedPole.id, requestData).subscribe(() => {
        this.loadPolesFiltered();
        this.closeModal();
      });
    } else {
      this.poleService.create(requestData).subscribe(() => {
        this.loadPolesFiltered();
        this.closeModal();
      });
    }
  }

  deletePole() {
    if (this.selectedPole) {
      this.poleService.delete(this.selectedPole.id).subscribe(() => {
        this.loadPolesFiltered();
        this.closeModal();
      });
    }
  }

  getRegionName(regionId: number): string {
    const region = this.regionOptions.find(r => r.id === regionId);
    return region ? region.name : 'N/A';
  }

  getPoleTypeName(poleType: number): string {
    switch (poleType) {
      case 0: return 'HV Pole';
      case 1: return 'MV Pole';
      case 2: return 'LV Pole';
      default: return 'Unknown Type';
    }
  }

  getPoleTypeClass(poleType: number): string {
    switch (poleType) {
      case 0: return 'bg-hv';
      case 1: return 'bg-mv';
      case 2: return 'bg-lv';
      default: return 'bg-secondary';
    }
  }
}
