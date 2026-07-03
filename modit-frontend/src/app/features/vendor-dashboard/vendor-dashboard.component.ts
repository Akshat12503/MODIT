import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

interface Lead {
  id: string;
  contractorName: string;
  projectLocation: string;
  material: string;
  quantity: number;
  unit: string;
  status: 'Pending' | 'Quoted' | 'Negotiating';
  aiSuggestedPrice: number;
}

@Component({
  selector: 'app-vendor-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './vendor-dashboard.component.html',
  styleUrl: './vendor-dashboard.component.scss'
})
export class VendorDashboardComponent {
  vendorName = signal('Delhi NCR Builders Hub');
  
  // Mocking incoming RFQs (Leads) from the buyer side
  activeLeads = signal<Lead[]>([
    { id: 'RFQ-001', contractorName: 'Apex Constructions', projectLocation: 'Noida Sector 62', material: 'OPC 43 Grade Cement', quantity: 100, unit: 'bags', status: 'Pending', aiSuggestedPrice: 375 },
    { id: 'RFQ-002', contractorName: 'Sharma Builders', projectLocation: 'Gurugram Phase 2', material: '12mm TMT Rebar', quantity: 500, unit: 'kg', status: 'Negotiating', aiSuggestedPrice: 62 },
    { id: 'RFQ-003', contractorName: 'Urban Nest Projects', projectLocation: 'South Extension', material: 'River Sand', quantity: 2000, unit: 'cft', status: 'Quoted', aiSuggestedPrice: 42 }
  ]);

  approveAiQuote(leadId: string) {
    const updatedLeads = this.activeLeads().map(lead => {
      if (lead.id === leadId) {
        return { ...lead, status: 'Quoted' as const };
      }
      return lead;
    });
    this.activeLeads.set(updatedLeads);
    alert('Agentic AI has successfully submitted the optimized quote to the buyer.');
  }
}