import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SubstationService } from '../../services/substation.service';
import { RegionService } from '../../services/region.service';
import { Substation, CreateSubstationRequest, UpdateSubstationRequest } from '../../models/substation.model';
import { RegionSelectOption } from '../../models/region-select.model';
import { Region, ApiResponse } from '../../models/region.model';

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
    this.loadSubstations();
    this.loadRegionOptions();
  }

  loadSubstations() {
    this.substationService.getAll().subscribe(response => {
      this.substations = response.data;
    });
  }

  loadRegionOptions() {
    this.regionService.getAllForSelect().subscribe(response => {
      this.regionOptions = response.data;
    });
  }

  openCreateModal() {
    this.isEdit = false;
    this.substationForm = {
      name: '',
      substationType: -1,
      latitude: 0,
      longitude: 0,
      regionId: 0
    };
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
    console.log('Edit modal opened with:', this.substationForm);
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
    // Convert string values to numbers for substationType and regionId
    const substationTypeNumber = +this.substationForm.substationType;
    const regionIdNumber = +this.substationForm.regionId;
    
    // Validate that valid options are selected
    if (substationTypeNumber < 0 || regionIdNumber <= 0) {
      alert('Please select valid Substation Type and Region');
      return;
    }

    const requestData = {
      ...this.substationForm,
      substationType: substationTypeNumber,
      regionId: regionIdNumber
    };

    if (this.isEdit && this.selectedSubstation) {
      this.substationService.update(this.selectedSubstation.id, requestData).subscribe(() => {
        this.loadSubstations();
        this.closeModal();
      });
    } else {
      this.substationService.create(requestData).subscribe(() => {
        this.loadSubstations();
        this.closeModal();
      });
    }
  }

  deleteSubstation() {
    if (this.selectedSubstation) {
      this.substationService.delete(this.selectedSubstation.id).subscribe(() => {
        this.loadSubstations();
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
      case 0: return 'Transmission Substation';
      case 1: return 'Injection Substation';
      case 2: return 'Distribution Substation';
      case 3: return 'Bulk Supply';
      default: return 'Unknown Type';
    }
  }
}