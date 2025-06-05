import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ApiService, FactGifResponse, SearchHistory } from './services/api';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
})
export class AppComponent implements OnInit {
  title = 'Cat Facts & Giphy App';
  activeTab: 'current' | 'history' = 'current';

  // Datos de la pestaña actual
  currentFact: any = {};
  isLoadingFact = false;
  isLoadingGif = false;
  error: string | null = null;

  // NUEVA PROPIEDAD PARA TRACKEAR OFFSET
  private currentOffset = 0;

  // Datos de la pestaña de historial
  searchHistory: SearchHistory[] = [];
  isLoadingHistory = false;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.loadNewFact();
  }

  setActiveTab(tab: 'current' | 'history'): void {
    this.activeTab = tab;
    if (tab === 'history') {
      this.loadHistory();
    }
  }

  loadNewFact(): void {
    this.isLoadingFact = true;
    this.error = null;

    this.apiService.getFact().subscribe({
      next: (response) => {
        this.currentFact = response;
        this.isLoadingFact = false;
      },
      error: (error) => {
        this.error = 'Error loading cat fact. Please try again.';
        this.isLoadingFact = false;
        console.error('Error:', error);
      },
    });
  }

  refreshGif(): void {
    this.isLoadingGif = true;

    // Incrementar offset para obtener el siguiente GIF
    this.currentOffset += 1;

    // Si llegamos muy alto, reiniciar (Giphy tiene límites)
    if (this.currentOffset > 100) {
      this.currentOffset = 0;
    }

    this.apiService
      .getGif(this.currentFact.queryWords, this.currentOffset)
      .subscribe({
        next: (response) => {
          this.currentFact.gifUrl = response.gifUrl;
          this.isLoadingGif = false;
        },
        error: (error) => {
          console.error('Error al cambiar GIF:', error);
          this.isLoadingGif = false;

          // Si hay error, probar con offset aleatorio
          const randomOffset = Math.floor(Math.random() * 50);
          this.currentOffset = randomOffset;
        },
      });
  }

  loadHistory(): void {
    this.isLoadingHistory = true;

    this.apiService.getHistory().subscribe({
      next: (response) => {
        this.searchHistory = response;
        this.isLoadingHistory = false;
      },
      error: (error) => {
        console.error('Error loading history:', error);
        this.isLoadingHistory = false;
      },
    });
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleString();
  }
}
