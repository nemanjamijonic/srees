import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BuildingService } from '../../services/building.service';
import { RegionService } from '../../services/region.service';
import { PoleService } from '../../services/pole.service';
import { Building, CreateBuildingRequest, UpdateBuildingRequest } from '../../models/building.model';
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
    this.buildingService.getAll().subscribe(response => {
      this.buildings = response.data;
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
    console.log('Edit modal opened with:', this.buildingForm);
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
    return pole ? pole.address : 'N/A';
  }
}
