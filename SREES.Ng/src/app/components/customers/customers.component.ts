import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';
import { BuildingService } from '../../services/building.service';
import { Customer, CreateCustomerRequest, UpdateCustomerRequest } from '../../models/customer.model';
import { BuildingSelectOption } from '../../models/building.model';

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.scss']
})
export class CustomersComponent implements OnInit {
  customers: Customer[] = [];
  buildingOptions: BuildingSelectOption[] = [];
  showModal = false;
  showDeleteModal = false;
  isEdit = false;
  selectedCustomer: Customer | null = null;
  
  customerForm: CreateCustomerRequest = {
    firstName: '',
    lastName: '',
    address: '',
    buildingId: null,
    isActive: true,
    customerType: -1
  };

  constructor(
    private customerService: CustomerService,
    private buildingService: BuildingService
  ) {}

  ngOnInit() {
    this.loadCustomers();
    this.loadBuildingOptions();
  }

  loadCustomers() {
    this.customerService.getAll().subscribe(response => {
      this.customers = response.data;
    });
  }

  loadBuildingOptions() {
    this.buildingService.getAllForSelect().subscribe(response => {
      this.buildingOptions = response.data;
    });
  }

  openCreateModal() {
    this.isEdit = false;
    this.customerForm = {
      firstName: '',
      lastName: '',
      address: '',
      buildingId: null,
      isActive: true,
      customerType: -1
    };
    this.showModal = true;
  }

  openEditModal(customer: Customer) {
    this.isEdit = true;
    this.selectedCustomer = customer;
    this.customerForm = {
      firstName: customer.firstName,
      lastName: customer.lastName,
      address: customer.address,
      buildingId: customer.buildingId,
      isActive: customer.isActive,
      customerType: customer.customerType
    };
    console.log('Edit modal opened with:', this.customerForm);
    this.showModal = true;
  }

  openDeleteModal(customer: Customer) {
    this.selectedCustomer = customer;
    this.showDeleteModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.showDeleteModal = false;
    this.selectedCustomer = null;
  }

  saveCustomer() {
    const customerTypeNumber = +this.customerForm.customerType;
    
    if (customerTypeNumber < 1) {
      alert('Please select valid Customer Type');
      return;
    }

    const requestData: CreateCustomerRequest | UpdateCustomerRequest = {
      firstName: this.customerForm.firstName,
      lastName: this.customerForm.lastName,
      address: this.customerForm.address,
      buildingId: this.customerForm.buildingId || null,
      isActive: this.customerForm.isActive,
      customerType: customerTypeNumber
    };

    if (this.isEdit && this.selectedCustomer) {
      this.customerService.update(this.selectedCustomer.id, requestData).subscribe(() => {
        this.loadCustomers();
        this.closeModal();
      });
    } else {
      this.customerService.create(requestData).subscribe(() => {
        this.loadCustomers();
        this.closeModal();
      });
    }
  }

  deleteCustomer() {
    if (this.selectedCustomer) {
      this.customerService.delete(this.selectedCustomer.id).subscribe(() => {
        this.loadCustomers();
        this.closeModal();
      });
    }
  }

  getBuildingAddress(buildingId: number | null): string {
    if (!buildingId) return 'N/A';
    const building = this.buildingOptions.find(b => b.id === buildingId);
    return building ? building.address : 'N/A';
  }

  getCustomerTypeName(customerType: number): string {
    switch (customerType) {
      case 1: return 'Residential';
      case 2: return 'Commercial';
      case 3: return 'Industrial';
      case 4: return 'Government';
      default: return 'Unknown Type';
    }
  }

  getCustomerTypeClass(customerType: number): string {
    switch (customerType) {
      case 1: return 'bg-residential';
      case 2: return 'bg-commercial';
      case 3: return 'bg-industrial';
      case 4: return 'bg-government';
      default: return 'bg-secondary';
    }
  }
}
