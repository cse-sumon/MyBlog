// Category model
export interface Category {
  id: number;
  name: string;
  description?: string;
  isActive: boolean;
  createdDate: Date;
}

// Create/Update Category DTO
export interface CreateUpdateCategoryDto {
  name: string;
  description?: string;
  isActive: boolean;
}
