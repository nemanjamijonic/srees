import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PoleService } from '../../services/pole.service';
import { RegionService } from '../../services/region.service';
import { Pole, CreatePoleRequest, UpdatePoleRequest } from '../../models/pole.model';
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
    this.loadPoles();
    this.loadRegionOptions();
  }

  loadPoles() {
    this.poleService.getAll().subscribe(response => {
      this.poles = response.data;
    });
  }

  loadRegionOptions() {
    this.regionService.getAllForSelect().subscribe(response => {
      this.regionOptions = response.data;
    });
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
    console.log('Edit modal opened with:', this.poleForm);
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
    // Convert string values to numbers for poleType and regionId
    const poleTypeNumber = +this.poleForm.poleType;
    const regionIdNumber = +this.poleForm.regionId;
    
    // Validate that valid options are selected
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
        this.loadPoles();
        this.closeModal();
      });
    } else {
      this.poleService.create(requestData).subscribe(() => {
        this.loadPoles();
        this.closeModal();
      });
    }
  }

  deletePole() {
    if (this.selectedPole) {
      this.poleService.delete(this.selectedPole.id).subscribe(() => {
        this.loadPoles();
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
}
