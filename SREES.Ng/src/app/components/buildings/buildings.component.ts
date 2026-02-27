import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BuildingService } from '../../services/building.service';
import { RegionService } from '../../services/region.service';
import { PoleService } from '../../services/pole.service';
import { Building, CreateBuildingRequest, UpdateBuildingRequest, BuildingFilterRequest, PaginatedResponse } from '../../models/building.model';
import { RegionSelectOption } from '../../models/region-select.model';
import { PoleSelectOption } from '../../models/pole.model';

@Component({
  selector: 'app-buildings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './buildings.component.html',
  styleUrls: ['./buildings.component.scss']
})
export class BuildingsComponent implements OnInit {
  buildings: Building[] = [];
  regionOptions: RegionSelectOption[] = [];
  poleOptions: PoleSelectOption[] = [];
  showModal = false;
  showDeleteModal = false;
  isEdit = false;
  selectedBuilding: Building | null = null;
  
  // Make Math available in template
  Math = Math;
  
  // Pagination properties
  totalCount = 0;
  totalPages = 0;
  currentPage = 1;
  pageSize = 10;
  pageSizes = [5, 10, 20, 50];
  pages: number[] = [];
  
  // Filter properties
  filterRequest: BuildingFilterRequest = {
    searchTerm: '',
    poleType: undefined,
    dateFrom: '',
    dateTo: '',
    pageNumber: 1,
    pageSize: 10
  };

  // Applied filters for display
  appliedFilters: Array<{label: string, value: string, key: string}> = [];
  
  // Pole type options for filter
  poleTypeOptions = [
    { value: undefined, label: 'All Pole Types' },
    { value: 0, label: 'HV Pole' },
    { value: 1, label: 'MV Pole' },
    { value: 2, label: 'LV Pole' }
  ];
  
  buildingForm: CreateBuildingRequest = {
    latitude: 0,
    longitude: 0,
    ownerName: '',
    address: '',
    regionId: 0,
    poleId: 0
  };

  constructor(
    private buildingService: BuildingService,
    private regionService: RegionService,
    private poleService: PoleService
  ) {}

  ngOnInit() {
    this.loadBuildings();
    this.loadRegionOptions();
    this.loadPoleOptions();
  }

  loadBuildings() {
    this.filterRequest.pageNumber = this.currentPage;
    this.filterRequest.pageSize = this.pageSize;
    
    this.buildingService.getFiltered(this.filterRequest).subscribe(response => {
      this.buildings = response.data.data;
      this.totalCount = response.data.totalCount;
      this.totalPages = response.data.totalPages;
      this.currentPage = response.data.currentPage;
      this.updatePagination();
    });
  }

  loadRegionOptions() {
    this.regionService.getAllForSelect().subscribe(response => {
      this.regionOptions = response.data;
    });
  }

  loadPoleOptions() {
    this.poleService.getAllForSelect().subscribe(response => {
      this.poleOptions = response.data;
    });
  }

  openCreateModal() {
    this.isEdit = false;
    this.buildingForm = {
      latitude: 0,
      longitude: 0,
      ownerName: '',
      address: '',
      regionId: 0,
      poleId: 0
    };
    this.showModal = true;
  }

  openEditModal(building: Building) {
    this.isEdit = true;
    this.selectedBuilding = building;
    this.buildingForm = {
      latitude: building.latitude,
      longitude: building.longitude,
      ownerName: building.ownerName,
      address: building.address,
      regionId: building.regionId,
      poleId: building.poleId
    };
    this.showModal = true;
  }

  openDeleteModal(building: Building) {
    this.selectedBuilding = building;
    this.showDeleteModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.showDeleteModal = false;
    this.selectedBuilding = null;
  }

  saveBuilding() {
    // Convert string values to numbers for regionId and poleId
    const regionIdNumber = +this.buildingForm.regionId;
    const poleIdNumber = +this.buildingForm.poleId;
    
    // Validate that valid options are selected
    if (regionIdNumber <= 0 || poleIdNumber <= 0) {
      alert('Please select valid Region and Pole');
      return;
    }

    const requestData = {
      ...this.buildingForm,
      regionId: regionIdNumber,
      poleId: poleIdNumber
    };

    if (this.isEdit && this.selectedBuilding) {
      this.buildingService.update(this.selectedBuilding.id, requestData).subscribe(() => {
        this.loadBuildings();
        this.closeModal();
      });
    } else {
      this.buildingService.create(requestData).subscribe(() => {
        this.loadBuildings();
        this.closeModal();
      });
    }
  }

  deleteBuilding() {
    if (this.selectedBuilding) {
      this.buildingService.delete(this.selectedBuilding.id).subscribe(() => {
        this.loadBuildings();
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

  getPoleAddress(poleId: number): string {
    const pole = this.poleOptions.find(p => p.id === poleId);
    return pole ? pole.name : 'N/A';
  }

  // Filter methods
  applyFilters() {
    this.currentPage = 1;
    this.filterRequest.pageNumber = 1;
    this.loadBuildings();
    this.updateAppliedFilters();
  }

  resetFilters() {
    this.filterRequest = {
      searchTerm: '',
      poleType: undefined,
      dateFrom: '',
      dateTo: '',
      pageNumber: 1,
      pageSize: this.pageSize
    };
    this.appliedFilters = [];
    this.currentPage = 1;
    this.loadBuildings();
  }

  removeFilter(key: string) {
    switch(key) {
      case 'searchTerm':
        this.filterRequest.searchTerm = '';
        break;
      case 'poleType':
        this.filterRequest.poleType = undefined;
        break;
      case 'dateFrom':
        this.filterRequest.dateFrom = '';
        break;
      case 'dateTo':
        this.filterRequest.dateTo = '';
        break;
    }
    this.applyFilters();
  }

  updateAppliedFilters() {
    this.appliedFilters = [];
    
    if (this.filterRequest.searchTerm) {
      this.appliedFilters.push({
        label: 'Search',
        value: this.filterRequest.searchTerm,
        key: 'searchTerm'
      });
    }

    if (this.filterRequest.poleType !== undefined && this.filterRequest.poleType !== null) {
      this.appliedFilters.push({
        label: 'Pole Type',
        value: this.getPoleTypeName(this.filterRequest.poleType),
        key: 'poleType'
      });
    }

    if (this.filterRequest.dateFrom) {
      this.appliedFilters.push({
        label: 'From',
        value: new Date(this.filterRequest.dateFrom).toLocaleDateString(),
        key: 'dateFrom'
      });
    }

    if (this.filterRequest.dateTo) {
      this.appliedFilters.push({
        label: 'To',
        value: new Date(this.filterRequest.dateTo).toLocaleDateString(),
        key: 'dateTo'
      });
    }
  }

  // Pagination methods
  updatePagination() {
    this.pages = [];
    const maxPagesToShow = 5;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(this.totalPages, startPage + maxPagesToShow - 1);
    
    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      this.pages.push(i);
    }
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.loadBuildings();
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.goToPage(this.currentPage - 1);
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.goToPage(this.currentPage + 1);
    }
  }

  onPageSizeChange() {
    this.currentPage = 1;
    this.loadBuildings();
  }
}
