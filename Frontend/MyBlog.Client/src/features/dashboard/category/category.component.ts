import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { CategoryService } from '../../../core/services/category.service';
import { Category, CreateUpdateCategoryDto } from '../../../core/models/category-model';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent implements OnInit, OnDestroy {
  allCategories: Category[] = [];
  categories: Category[] = [];
  searchQuery = '';
  categoryForm!: FormGroup;
  loading = false;
  submitting = false;
  error: string | null = null;
  success: string | null = null;
  showForm = false;
  editingId: number | null = null;
  deleteConfirmId: number | null = null;
  private destroy$ = new Subject<void>();

  constructor(
    private categoryService: CategoryService,
    private formBuilder: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.initializeForm();
  }

  ngOnInit(): void {
    this.loadCategories();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeForm(): void {
    this.categoryForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: [''],
      isActive: [true]
    });
  }

  get f() {
    return this.categoryForm.controls;
  }

  loadCategories(): void {
    this.loading = true;
    this.error = null;
    this.success = null;
    
    console.log('Loading categories...');
    
    this.categoryService
      .getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          console.log('Categories loaded successfully:', data);
          this.allCategories = data;
          this.applyFilter();
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          this.loading = false;
          console.error('Full error details:', err);
          console.error('Error status:', err.status);
          console.error('Error message:', err.message);
          this.error = err.error?.message || err.message || 'Failed to load categories';
          this.cdr.detectChanges();
        }
      });
  }

  applyFilter(): void {
    const query = this.searchQuery.toLowerCase().trim();
    if (!query) {
      this.categories = this.allCategories;
    } else {
      this.categories = this.allCategories.filter((cat) =>
        cat.name.toLowerCase().includes(query) ||
        (cat.description && cat.description.toLowerCase().includes(query))
      );
    }
    this.cdr.detectChanges();
  }

  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchQuery = input.value;
    this.applyFilter();
  }

  clearSearch(): void {
    this.searchQuery = '';
    this.applyFilter();
  }

  openForm(category?: Category): void {
    this.error = null;
    this.success = null;

    if (category) {
      this.editingId = category.id;
      this.categoryForm.patchValue({
        name: category.name,
        description: category.description,
        isActive: category.isActive
      });
    } else {
      this.editingId = null;
      this.categoryForm.reset({ isActive: true });
    }

    this.showForm = true;
  }

  closeForm(): void {
    this.showForm = false;
    this.categoryForm.reset();
    this.editingId = null;
    this.error = null;
  }

  onSubmit(): void {
    this.error = null;
    this.success = null;

    if (this.categoryForm.invalid) {
      return;
    }

    this.submitting = true;
    const formValue: CreateUpdateCategoryDto = this.categoryForm.value;

    if (this.editingId) {
      // Update
      this.categoryService
        .update(this.editingId, formValue)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.success = 'Category updated successfully';
            this.submitting = false;
            this.closeForm();
            this.cdr.detectChanges();
            setTimeout(() => {
              this.loadCategories();
              this.cdr.detectChanges();
            }, 100);
          },
          error: (err) => {
            this.submitting = false;
            this.error = err.error?.message || 'Failed to update category';
            console.error('Update error:', err);
            this.cdr.detectChanges();
          }
        });
    } else {
      // Create
      this.categoryService
        .create(formValue)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.success = 'Category created successfully';
            this.submitting = false;
            this.closeForm();
            this.cdr.detectChanges();
            setTimeout(() => {
              this.loadCategories();
              this.cdr.detectChanges();
            }, 100);
          },
          error: (err) => {
            this.submitting = false;
            this.error = err.error?.message || 'Failed to create category';
            console.error('Create error:', err);
            this.cdr.detectChanges();
          }
        });
    }
  }

  confirmDelete(id: number): void {
    this.deleteConfirmId = id;
  }

  cancelDelete(): void {
    this.deleteConfirmId = null;
  }

  deleteCategory(id: number): void {
    this.categoryService
      .delete(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.success = 'Category deleted successfully';
          this.deleteConfirmId = null;
          this.cdr.detectChanges();
          setTimeout(() => {
            this.loadCategories();
            this.cdr.detectChanges();
          }, 100);
        },
        error: (err) => {
          this.error = err.error?.message || 'Failed to delete category';
          this.deleteConfirmId = null;
          console.error('Delete error:', err);
          this.cdr.detectChanges();
        }
      });
  }

  getStatusBadge(isActive: boolean): string {
    return isActive ? 'Active' : 'Inactive';
  }

  getStatusClass(isActive: boolean): string {
    return isActive ? 'badge-success' : 'badge-danger';
  }
}
