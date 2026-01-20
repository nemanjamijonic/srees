import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';
import { BuildingService } from '../../services/building.service';
import { Customer, CreateCustomerRequest, UpdateCustomerRequest, CustomerFilterRequest } from '../../models/customer.model';
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
  
  // Expose Math to template
  Math = Math;
  
  // Pagination properties
  totalCount = 0;
  totalPages = 0;
  currentPage = 1;
  pageSize = 1;
  pages: number[] = [];

  // Filter properties
  filterRequest: CustomerFilterRequest = {
    searchTerm: '',
    customerType: undefined,
    dateFrom: '',
    dateTo: '',
    pageNumber: 1,
    pageSize: 10
  };

  // Applied filters for display
  appliedFilters: Array<{label: string, value: string, key: string}> = [];
  
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
    this.customerService.getFiltered(this.filterRequest).subscribe(response => {
      this.customers = response.data.data;
      this.totalCount = response.data.totalCount;
      this.totalPages = response.data.totalPages;
      this.currentPage = response.data.currentPage;
      this.pageSize = response.data.pageSize;
      this.updatePagination();
    });
  }

  applyFilters() {
    this.filterRequest.pageNumber = 1;
    this.currentPage = 1;
    this.loadCustomers();
    this.updateAppliedFilters();
  }

  clearFilters() {
    this.filterRequest = {
      searchTerm: '',
      customerType: undefined,
      dateFrom: '',
      dateTo: '',
      pageNumber: 1,
      pageSize: 10
    };
    this.appliedFilters = [];
    this.loadCustomers();
  }

  removeFilter(key: string) {
    switch(key) {
      case 'searchTerm':
        this.filterRequest.searchTerm = '';
        break;
      case 'customerType':
        this.filterRequest.customerType = undefined;
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

    if (this.filterRequest.customerType !== undefined && this.filterRequest.customerType !== null) {
      this.appliedFilters.push({
        label: 'Type',
        value: this.getCustomerTypeName(this.filterRequest.customerType),
        key: 'customerType'
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
      this.filterRequest.pageNumber = page;
      this.loadCustomers();
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
