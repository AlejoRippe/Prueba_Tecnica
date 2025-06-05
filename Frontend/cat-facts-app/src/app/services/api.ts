import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface FactGifResponse {
  fact: string;
  queryWords: string;
  gifUrl: string;
}

export interface SearchHistory {
  searchDate: string;
  factText: string;
  queryWords: string;
  gifUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'http://localhost:5000/api';  

  constructor(private http: HttpClient) { }

  getFact(): Observable<FactGifResponse> {
    return this.http.get<FactGifResponse>(`${this.baseUrl}/fact`);
  }

  getGif(queryWords: string, offset: number = 0): Observable<FactGifResponse> {
  return this.http.get<FactGifResponse>(`${this.baseUrl}/gif?query=${encodeURIComponent(queryWords)}&offset=${offset}`);
  }

  getHistory(): Observable<SearchHistory[]> {
    return this.http.get<SearchHistory[]>(`${this.baseUrl}/history`);
  }
}