// Login DTO
export interface LoginDto {
  userName: string;
  password: string;
}

// Register DTO
export interface RegisterDto {
  fullName: string;
  userName: string;
  email: string;
  password: string;
  role?: string;
}

// User model
export interface User {
  id: string;
  userName: string;
  email: string;
  fullName: string | null;
  roles: string[];
}

// Auth response from API
export interface AuthResponse {
  token: string;
  user: User;
}

// Decoded JWT token payload
export interface JwtPayload {
  sub: string;
  nameid: string;
  unique_name: string[];
  role: string;
  nbf: number;
  exp: number;
  iat: number;
  iss: string;
  aud: string;
}
