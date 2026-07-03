import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

interface ExtractedMaterial {
  id: string;
  originalText: string;
  mappedCategory: string;
  quantity: number;
  unit: string;
  estimatedTotal: number;
  status: 'Matched' | 'Review Needed';
}

@Component({
  selector: 'app-bom-upload',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './bom-upload.component.html',
  styleUrl: './bom-upload.component.scss'
})
export class BomUploadComponent {
  selectedFile = signal<File | null>(null);
  isAnalyzing = signal(false);
  extractedData = signal<ExtractedMaterial[] | null>(null);

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      this.selectedFile.set(input.files[0]);
    }
  }

  analyzeBOM() {
    if (!this.selectedFile()) return;
    
    this.isAnalyzing.set(true);

    // Simulate Agentic AI processing delay (2 seconds) to look realistic in the demo
    setTimeout(() => {
      this.isAnalyzing.set(false);
      
      // Mock data representing what the AI would extract from a contractor's PDF
      this.extractedData.set([
        { id: '1', originalText: 'OPC 43 Grade Cement - 100 bags', mappedCategory: 'Cement', quantity: 100, unit: 'bags', estimatedTotal: 38000, status: 'Matched' },
        { id: '2', originalText: '12mm TMT Rebar (Tata)', mappedCategory: 'Steel', quantity: 500, unit: 'kg', estimatedTotal: 32500, status: 'Matched' },
        { id: '3', originalText: 'River sand coarse 2 trucks', mappedCategory: 'Aggregates', quantity: 2000, unit: 'cft', estimatedTotal: 90000, status: 'Review Needed' },
        { id: '4', originalText: 'Red bricks wire cut class A', mappedCategory: 'Bricks', quantity: 10000, unit: 'pieces', estimatedTotal: 80000, status: 'Matched' }
      ]);
    }, 2000);
  }

  getTotalEstimate(): number {
    return this.extractedData()?.reduce((sum, item) => sum + item.estimatedTotal, 0) || 0;
  }
}