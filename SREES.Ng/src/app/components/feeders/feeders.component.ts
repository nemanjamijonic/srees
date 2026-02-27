import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FeederService } from '../../services/feeder.service';
import { SubstationService } from '../../services/substation.service';
import { RegionService } from '../../services/region.service';
import { Feeder, CreateFeederRequest, UpdateFeederRequest, FeederFilterRequest } from '../../models/feeder.model';
import { SubstationSelectOption } from '../../models/substation.model';
import { RegionSelectOption } from '../../models/region-select.model';

@Component({
  selector: 'app-feeders',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './feeders.component.html',
  styleUrls: ['./feeders.component.scss']
})
export class FeedersComponent implements OnInit {
  feeders: Feeder[] = [];
  substationOptions: SubstationSelectOption[] = [];
  regionOptions: RegionSelectOption[] = [];
  showModal = false;
  showDeleteModal = false;
  isEdit = false;
  selectedFeeder: Feeder | null = null;
  Math = Math;

  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  pages: number[] = [];

  // Filter
  filterRequest: FeederFilterRequest = {
    pageNumber: 1,
    pageSize: 10
  };

  appliedFilters: { key: string; label: string; value: string }[] = [];

  feederForm: CreateFeederRequest = {
    name: '',
    feederType: -1,
    substationId: null,
    suppliedRegions: []
  };

  constructor(
    private feederService: FeederService,
    private substationService: SubstationService,
    private regionService: RegionService
  ) {}

  ngOnInit() {
    this.loadFeedersFiltered();
    this.loadSubstationOptions();
    this.loadRegionOptions();
  }

  loadFeedersFiltered() {
    this.filterRequest.pageNumber = this.currentPage;
    this.filterRequest.pageSize = this.pageSize;

    this.feederService.getFiltered(this.filterRequest).subscribe(response => {
      const paginated = response.data;
      this.feeders = paginated.data;
      this.totalCount = paginated.totalCount;
      this.totalPages = paginated.totalPages;
      this.currentPage = paginated.currentPage;
      this.generatePages();
    });
  }

  generatePages() {
    this.pages = [];
    for (let i = 1; i <= this.totalPages; i++) {
      this.pages.push(i);
    }
  }

  applyFilters() {
    this.currentPage = 1;
    this.updateAppliedFilters();
    this.loadFeedersFiltered();
  }

  resetFilters() {
    this.filterRequest = { pageNumber: 1, pageSize: this.pageSize };
    this.appliedFilters = [];
    this.currentPage = 1;
    this.loadFeedersFiltered();
  }

  removeFilter(key: string) {
    if (key === 'searchTerm') this.filterRequest.searchTerm = undefined;
    if (key === 'feederType') this.filterRequest.feederType = undefined;
    if (key === 'dateFrom') this.filterRequest.dateFrom = undefined;
    if (key === 'dateTo') this.filterRequest.dateTo = undefined;
    this.applyFilters();
  }

  updateAppliedFilters() {
    this.appliedFilters = [];
    if (this.filterRequest.searchTerm) {
      this.appliedFilters.push({ key: 'searchTerm', label: 'Search', value: this.filterRequest.searchTerm });
    }
    if (this.filterRequest.feederType !== undefined && this.filterRequest.feederType !== null) {
      this.appliedFilters.push({ key: 'feederType', label: 'Type', value: this.getFeederTypeName(this.filterRequest.feederType) });
    }
    if (this.filterRequest.dateFrom) {
      this.appliedFilters.push({ key: 'dateFrom', label: 'From', value: this.filterRequest.dateFrom });
    }
    if (this.filterRequest.dateTo) {
      this.appliedFilters.push({ key: 'dateTo', label: 'To', value: this.filterRequest.dateTo });
    }
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadFeedersFiltered();
  }

  previousPage() {
    if (this.currentPage > 1) this.goToPage(this.currentPage - 1);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) this.goToPage(this.currentPage + 1);
  }

  loadSubstationOptions() {
    this.substationService.getAllForSelect().subscribe(response => {
      this.substationOptions = response.data;
    });
  }

  loadRegionOptions() {
    this.regionService.getAllForSelect().subscribe(response => {
      this.regionOptions = response.data;
    });
  }

  openCreateModal() {
    this.isEdit = false;
    this.feederForm = {
      name: '',
      feederType: -1,
      substationId: null,
      suppliedRegions: []
    };
    this.showModal = true;
  }

  openEditModal(feeder: Feeder) {
    this.isEdit = true;
    this.selectedFeeder = feeder;
    this.feederForm = {
      name: feeder.name,
      feederType: feeder.feederType,
      substationId: feeder.substationId,
      suppliedRegions: feeder.suppliedRegions || []
    };
    this.showModal = true;
  }

  openDeleteModal(feeder: Feeder) {
    this.selectedFeeder = feeder;
    this.showDeleteModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.showDeleteModal = false;
    this.selectedFeeder = null;
  }

  saveFeeder() {
    const feederTypeNumber = +this.feederForm.feederType;

    if (feederTypeNumber < 0) {
      alert('Please select valid Feeder Type');
      return;
    }

    const requestData: CreateFeederRequest | UpdateFeederRequest = {
      name: this.feederForm.name || '',
      feederType: feederTypeNumber,
      substationId: this.feederForm.substationId || null,
      suppliedRegions: this.feederForm.suppliedRegions && this.feederForm.suppliedRegions.length > 0
        ? this.feederForm.suppliedRegions
        : null
    };

    if (this.isEdit && this.selectedFeeder) {
      this.feederService.update(this.selectedFeeder.id, requestData).subscribe(() => {
        this.loadFeedersFiltered();
        this.closeModal();
      });
    } else {
      this.feederService.create(requestData).subscribe(() => {
        this.loadFeedersFiltered();
        this.closeModal();
      });
    }
  }

  deleteFeeder() {
    if (this.selectedFeeder) {
      this.feederService.delete(this.selectedFeeder.id).subscribe(() => {
        this.loadFeedersFiltered();
        this.closeModal();
      });
    }
  }

  getSubstationName(substationId: number | null): string {
    if (!substationId) return 'N/A';
    const substation = this.substationOptions.find(s => s.id === substationId);
    return substation ? substation.name : 'N/A';
  }

  getFeederTypeName(feederType: number): string {
    switch (feederType) {
      case 0: return 'Feeder 11kV';
      case 1: return 'Feeder 33kV';
      default: return 'Unknown Type';
    }
  }

  getFeederTypeClass(feederType: number): string {
    switch (feederType) {
      case 0: return 'bg-feeder11';
      case 1: return 'bg-feeder33';
      default: return 'bg-secondary';
    }
  }

  getSuppliedRegionsNames(regionIds: number[] | null): string {
    if (!regionIds || regionIds.length === 0) return 'N/A';
    const regionNames = regionIds
      .map(id => {
        const region = this.regionOptions.find(r => r.id === id);
        return region ? region.name : null;
      })
      .filter(name => name !== null);
    return regionNames.length > 0 ? regionNames.join(', ') : 'N/A';
  }

  toggleRegion(regionId: number) {
    if (!this.feederForm.suppliedRegions) {
      this.feederForm.suppliedRegions = [];
    }
    const index = this.feederForm.suppliedRegions.indexOf(regionId);
    if (index > -1) {
      this.feederForm.suppliedRegions.splice(index, 1);
    } else {
      this.feederForm.suppliedRegions.push(regionId);
    }
  }

  isRegionSelected(regionId: number): boolean {
    return this.feederForm.suppliedRegions?.includes(regionId) || false;
  }
}


// @Component({
//   selector: 'app-feeders',
//   standalone: true,
//   imports: [CommonModule, FormsModule],
//   templateUrl: './feeders.component.html',
//   styleUrls: ['./feeders.component.scss']
// })
// export class FeedersComponent implements OnInit {
//   feeders: Feeder[] = [];
//   substationOptions: SubstationSelectOption[] = [];
//   regionOptions: RegionSelectOption[] = [];
//   showModal = false;
//   showDeleteModal = false;
//   isEdit = false;
//   selectedFeeder: Feeder | null = null;
  
//   feederForm: CreateFeederRequest = {
//     name: '',
//     feederType: -1,
//     substationId: null,
//     suppliedRegions: []
//   };

//   constructor(
//     private feederService: FeederService,
//     private substationService: SubstationService,
//     private regionService: RegionService
//   ) {}

//   ngOnInit() {
//     this.loadFeeders();
//     this.loadSubstationOptions();
//     this.loadRegionOptions();
//   }

//   loadFeeders() {
//     this.feederService.getAll().subscribe(response => {
//       this.feeders = response.data;
//     });
//   }

//   loadSubstationOptions() {
//     this.substationService.getAllForSelect().subscribe(response => {
//       this.substationOptions = response.data;
//     });
//   }

//   loadRegionOptions() {
//     this.regionService.getAllForSelect().subscribe(response => {
//       this.regionOptions = response.data;
//     });
//   }

//   openCreateModal() {
//     this.isEdit = false;
//     this.feederForm = {
//       name: '',
//       feederType: -1,
//       substationId: null,
//       suppliedRegions: []
//     };
//     this.showModal = true;
//   }

//   openEditModal(feeder: Feeder) {
//     this.isEdit = true;
//     this.selectedFeeder = feeder;
//     this.feederForm = {
//       name: feeder.name,
//       feederType: feeder.feederType,
//       substationId: feeder.substationId,
//       suppliedRegions: feeder.suppliedRegions || []
//     };
//     this.showModal = true;
//   }

//   openDeleteModal(feeder: Feeder) {
//     this.selectedFeeder = feeder;
//     this.showDeleteModal = true;
//   }

//   closeModal() {
//     this.showModal = false;
//     this.showDeleteModal = false;
//     this.selectedFeeder = null;
//   }

//   saveFeeder() {
//     const feederTypeNumber = +this.feederForm.feederType;
    
//     if (feederTypeNumber < 0) {
//       alert('Please select valid Feeder Type');
//       return;
//     }

//     const requestData: CreateFeederRequest | UpdateFeederRequest = {
//       name: this.feederForm.name || '',
//       feederType: feederTypeNumber,
//       substationId: this.feederForm.substationId || null,
//       suppliedRegions: this.feederForm.suppliedRegions && this.feederForm.suppliedRegions.length > 0 
//         ? this.feederForm.suppliedRegions 
//         : null
//     };

//     if (this.isEdit && this.selectedFeeder) {
//       this.feederService.update(this.selectedFeeder.id, requestData).subscribe(() => {
//         this.loadFeeders();
//         this.closeModal();
//       });
//     } else {
//       this.feederService.create(requestData).subscribe(() => {
//         this.loadFeeders();
//         this.closeModal();
//       });
//     }
//   }

//   deleteFeeder() {
//     if (this.selectedFeeder) {
//       this.feederService.delete(this.selectedFeeder.id).subscribe(() => {
//         this.loadFeeders();
//         this.closeModal();
//       });
//     }
//   }

//   getSubstationName(substationId: number | null): string {
//     if (!substationId) return 'N/A';
//     const substation = this.substationOptions.find(s => s.id === substationId);
//     return substation ? substation.name : 'N/A';
//   }

//   getFeederTypeName(feederType: number): string {
//     switch (feederType) {
//       case 0: return 'Feeder 11kV';
//       case 1: return 'Feeder 33kV';
//       default: return 'Unknown Type';
//     }
//   }

//   getSuppliedRegionsNames(regionIds: number[] | null): string {
//     if (!regionIds || regionIds.length === 0) return 'N/A';
//     const regionNames = regionIds
//       .map(id => {
//         const region = this.regionOptions.find(r => r.id === id);
//         return region ? region.name : null;
//       })
//       .filter(name => name !== null);
//     return regionNames.length > 0 ? regionNames.join(', ') : 'N/A';
//   }

//   toggleRegion(regionId: number) {
//     if (!this.feederForm.suppliedRegions) {
//       this.feederForm.suppliedRegions = [];
//     }
//     const index = this.feederForm.suppliedRegions.indexOf(regionId);
//     if (index > -1) {
//       this.feederForm.suppliedRegions.splice(index, 1);
//     } else {
//       this.feederForm.suppliedRegions.push(regionId);
//     }
//   }

//   isRegionSelected(regionId: number): boolean {
//     return this.feederForm.suppliedRegions?.includes(regionId) || false;
//   }
// }
