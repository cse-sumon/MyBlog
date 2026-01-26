import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category, CreateUpdateCategoryDto } from '../models/category-model';
import { environment } from '../../config/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private readonly apiUrl = `${environment.apiUrl}/Category`;

  constructor(private http: HttpClient) {}

  /**
   * Get all categories
   */
  getAll(search?: string): Observable<Category[]> {
    let params = new HttpParams();
    if (search && search.trim().length) {
      params = params.set('search', search.trim());
    }

    return this.http.get<Category[]>(this.apiUrl, { params });
  }

  /**
   * Get category by id
   */
  getById(id: number): Observable<Category> {
    return this.http.get<Category>(`${this.apiUrl}/${id}`);
  }

  /**
   * Create new category
   */
  create(category: CreateUpdateCategoryDto): Observable<Category> {
    return this.http.post<Category>(this.apiUrl, category);
  }

  /**
   * Update category
   */
  update(id: number, category: CreateUpdateCategoryDto): Observable<Category> {
    return this.http.put<Category>(`${this.apiUrl}/${id}`, category);
  }

  /**
   * Delete category
   */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  /**
   * Get active categories
   */
  getActive(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/active`);
  }
}
