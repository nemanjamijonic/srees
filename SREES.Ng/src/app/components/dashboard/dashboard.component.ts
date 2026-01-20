import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, ChartConfiguration, registerables } from 'chart.js';
import { OutageService } from '../../services/outage.service';
import { CustomerService } from '../../services/customer.service';
import { RegionService } from '../../services/region.service';
import { SubstationService } from '../../services/substation.service';
import { PoleService } from '../../services/pole.service';
import { FeederService } from '../../services/feeder.service';
import { forkJoin, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { BuildingService } from '../../services/building.service';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy, AfterViewInit {
  private destroy$ = new Subject<void>();
  private charts: Chart[] = [];

  // KPI Stats
  stats = {
    totalOutages: 0,
    totalCustomers: 0,
    totalRegions: 0,
    totalSubstations: 0,
    totalPoles: 0,
    totalFeeders: 0,
    totalBuildings: 0
  };

  // Loading state
  isLoading = true;

  constructor(
    private outageService: OutageService,
    private customerService: CustomerService,
    private regionService: RegionService,
    private substationService: SubstationService,
    private poleService: PoleService,
    private feederService: FeederService,
    private buildingService: BuildingService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  ngAfterViewInit(): void {
    // Charts will be created after data is loaded
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.destroyCharts();
  }

  private formatLabel(label: string): string {
    return label
      .replace(/([a-z])([A-Z])/g, '$1 $2') // Add space between lowercase and uppercase
      .replace(/([A-Z])([A-Z][a-z])/g, '$1 $2') // Add space between consecutive capitals followed by lowercase
      .replace(/([a-zA-Z])([0-9])/g, '$1 $2') // Add space between letter and number
      .replace(/([0-9])([a-zA-Z])/g, '$1 $2') // Add space between number and letter
      .trim();
  }

  loadDashboardData(): void {
    this.isLoading = true;

    forkJoin({
      outageStats: this.outageService.getStatistics(),
      customerStats: this.customerService.getStatistics(),
      regionStats: this.regionService.getStatistics(),
      substationStats: this.substationService.getStatistics(),
      poleStats: this.poleService.getStatistics(),
      feederStats: this.feederService.getStatistics(),
      buildingStats: this.buildingService.getStatistics()
    })
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: (data) => {        
        const outageStats = data.outageStats.data || [];
        this.stats.totalOutages = outageStats.reduce((sum, stat) => sum + stat.count, 0);
        
        this.stats.totalCustomers = (data.customerStats.data || []).reduce((sum, stat) => sum + stat.count, 0);
        this.stats.totalRegions = (data.regionStats.data || []).reduce((sum, stat) => sum + stat.count, 0);
        this.stats.totalSubstations = (data.substationStats.data || []).reduce((sum, stat) => sum + stat.count, 0);
        this.stats.totalPoles = (data.poleStats.data || []).reduce((sum, stat) => sum + stat.count, 0);
        this.stats.totalFeeders = (data.feederStats.data || []).reduce((sum, stat) => sum + stat.count, 0);
        this.stats.totalBuildings = (data.buildingStats.data || []).reduce((sum, stat) => sum + stat.count, 0);
        this.isLoading = false;
        
        setTimeout(() => {
          this.createCharts(data);
        }, 100);
      },
      error: (error) => {
        this.isLoading = false;
      }
    });
  }

  createCharts(data: any): void {
    this.destroyCharts();
    this.createOutageStatusChart(data.outageStats.data || []);
    this.createCustomerTypeChart(data.customerStats.data || []);
    this.createPoleTypeChart(data.poleStats.data || []);
    this.createFeederTypeChart(data.feederStats.data || []);
    this.createSubstationTypeChart(data.substationStats.data || []);
  }

  createOutageStatusChart(stats: any[]): void {
    const statusMap: any = {};
    stats.forEach(stat => {
      statusMap[stat.name] = stat.count;
    });

    const canvas = document.getElementById('outageStatusChart') as HTMLCanvasElement;
    if (!canvas) {
      return;
    }

    const config: ChartConfiguration = {
      type: 'doughnut',
      data: {
        labels: ['Reported', 'In Progress', 'Resolved'],
        datasets: [{
          data: [
            statusMap['Reported'] || 0,
            statusMap['In Progress'] || 0,
            statusMap['Resolved'] || 0
          ],
          backgroundColor: [
            'rgba(255, 99, 132, 0.8)',
            'rgba(255, 206, 86, 0.8)',
            'rgba(75, 192, 192, 0.8)'
          ],
          borderColor: [
            'rgba(255, 99, 132, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)'
          ],
          borderWidth: 2
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              padding: 15,
              font: {
                size: 12
              }
            }
          },
          title: {
            display: true,
            text: 'Outage Status',
            font: {
              size: 16,
              weight: 'bold'
            }
          }
        }
      }
    };

    this.charts.push(new Chart(canvas, config));
  }

  createCustomerTypeChart(stats: any[]): void {
    const labels = stats.map(stat => this.formatLabel(stat.name));
    const data = stats.map(stat => stat.count);

    const canvas = document.getElementById('customerTypeChart') as HTMLCanvasElement;
    if (!canvas) {
      return;
    }

    const config: ChartConfiguration = {
      type: 'pie',
      data: {
        labels: labels,
        datasets: [{
          data: data,
          backgroundColor: [
            'rgba(40, 167, 69, 0.8)',      // Residential - Green
            'rgba(253, 126, 20, 0.8)',     // Commercial - Orange
            'rgba(111, 66, 193, 0.8)',     // Industrial - Purple
            'rgba(0, 123, 255, 0.8)'       // Government - Blue
          ],
          borderColor: [
            'rgba(40, 167, 69, 1)',
            'rgba(253, 126, 20, 1)',
            'rgba(111, 66, 193, 1)',
            'rgba(0, 123, 255, 1)'
          ],
          borderWidth: 2
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              padding: 15,
              font: {
                size: 12
              }
            }
          },
          title: {
            display: true,
            text: 'Customers by Type',
            font: {
              size: 16,
              weight: 'bold'
            }
          }
        }
      }
    };

    this.charts.push(new Chart(canvas, config));
  }

  createPoleTypeChart(stats: any[]): void {
    const labels = stats.map(stat => this.formatLabel(stat.name));
    const data = stats.map(stat => stat.count);

    const canvas = document.getElementById('poleTypeChart') as HTMLCanvasElement;
    if (!canvas) {
      return;
    }

    const config: ChartConfiguration = {
      type: 'doughnut',
      data: {
        labels: labels,
        datasets: [{
          data: data,
          backgroundColor: [
            'rgba(220, 53, 69, 0.8)',      // HV - Red
            'rgba(253, 126, 20, 0.8)',     // MV - Orange
            'rgba(255, 193, 7, 0.8)'       // LV - Yellow
          ],
          borderColor: [
            'rgba(220, 53, 69, 1)',
            'rgba(253, 126, 20, 1)',
            'rgba(255, 193, 7, 1)'
          ],
          borderWidth: 2
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              padding: 15,
              font: {
                size: 11
              }
            }
          },
          title: {
            display: true,
            text: 'Poles by Voltage',
            font: {
              size: 16,
              weight: 'bold'
            }
          }
        }
      }
    };

    this.charts.push(new Chart(canvas, config));
  }

  createFeederTypeChart(stats: any[]): void {
    const labels = stats.map(stat => this.formatLabel(stat.name));
    const data = stats.map(stat => stat.count);

    const canvas = document.getElementById('feederTypeChart') as HTMLCanvasElement;
    if (!canvas) {
      return;
    }

    const config: ChartConfiguration = {
      type: 'pie',
      data: {
        labels: labels,
        datasets: [{
          data: data,
          backgroundColor: [
            'rgba(0, 123, 255, 0.8)',      // F11 - Blue
            'rgba(111, 66, 193, 0.8)'      // F33 - Purple
          ],
          borderColor: [
            'rgba(0, 123, 255, 1)',
            'rgba(111, 66, 193, 1)'
          ],
          borderWidth: 2
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              padding: 15,
              font: {
                size: 12
              }
            }
          },
          title: {
            display: true,
            text: 'Feeders by Type',
            font: {
              size: 16,
              weight: 'bold'
            }
          }
        }
      }
    };

    this.charts.push(new Chart(canvas, config));
  }

  createSubstationTypeChart(stats: any[]): void {
    const labels = stats.map(stat => this.formatLabel(stat.name));
    const data = stats.map(stat => stat.count);

    const canvas = document.getElementById('substationTypeChart') as HTMLCanvasElement;
    if (!canvas) {
      return;
    }

    const config: ChartConfiguration = {
      type: 'pie',
      data: {
        labels: labels,
        datasets: [{
          data: data,
          backgroundColor: [
            'rgba(220, 53, 69, 0.8)',      // Transmission - Red
            'rgba(0, 123, 255, 0.8)',      // Injection - Blue
            'rgba(40, 167, 69, 0.8)',      // Distribution - Green
            'rgba(253, 126, 20, 0.8)'      // Bulk Supply - Orange
          ],
          borderColor: [
            'rgba(220, 53, 69, 1)',
            'rgba(0, 123, 255, 1)',
            'rgba(40, 167, 69, 1)',
            'rgba(253, 126, 20, 1)'
          ],
          borderWidth: 2
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              padding: 15,
              font: {
                size: 12
              }
            }
          },
          title: {
            display: true,
            text: 'Substations by Type',
            font: {
              size: 16,
              weight: 'bold'
            }
          }
        }
      }
    };

    this.charts.push(new Chart(canvas, config));
  }

  // Monthly trends zahteva drugačiji format podataka - privremeno onemogućeno
  // Ako želite mesečne trendove, potreban je poseban endpoint na backendu

  destroyCharts(): void {
    this.charts.forEach(chart => chart.destroy());
    this.charts = [];
  }
}
