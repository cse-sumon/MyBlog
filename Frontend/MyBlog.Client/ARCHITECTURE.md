# MyBlog - Angular Frontend Application

A professional Angular frontend application with authentication, role-based access control, and category management dashboard.

## 📁 Project Structure

```
src/
├── app/                          # Application root
│   ├── app.config.ts            # Global configuration with providers
│   ├── app.routes.ts            # Application routing
│   ├── app.ts                   # Root component
│   ├── app.html                 # Root template
│   └── app.css                  # Root styles
│
├── core/                        # Core module (singleton services, guards, interceptors)
│   ├── guards/
│   │   └── auth.guard.ts        # Route protection guard
│   ├── interceptors/
│   │   └── auth.interceptor.ts  # HTTP JWT token injection & error handling
│   ├── models/
│   │   ├── auth-models.ts       # Authentication DTOs (LoginDto, RegisterDto, User, AuthResponse)
│   │   └── category-model.ts    # Category model
│   └── services/
│       ├── auth.service.ts      # Authentication & token management
│       └── category.service.ts  # Category CRUD operations
│
├── features/                    # Feature modules
│   ├── auth/                    # Authentication feature
│   │   ├── login/
│   │   │   ├── login.component.ts
│   │   │   ├── login.component.html
│   │   │   └── login.component.css
│   │   └── register/
│   │       ├── register.component.ts
│   │       ├── register.component.html
│   │       └── register.component.css
│   │
│   └── dashboard/               # Dashboard feature
│       ├── layout/
│       │   ├── dashboard-layout.component.ts        # Main dashboard layout with sidebar
│       │   ├── dashboard-layout.component.html
│       │   ├── dashboard-layout.component.css
│       │   ├── dashboard-home.component.ts          # Dashboard home page
│       │   ├── dashboard-home.component.html
│       │   └── dashboard-home.component.css
│       └── category/
│           ├── category.component.ts                # Category CRUD component
│           ├── category.component.html
│           └── category.component.css
│
├── shared/                      # Shared components (to be extended)
│   └── components/
│
├── config/                      # Configuration files
│   ├── environment.ts          # Development environment
│   └── environment.prod.ts     # Production environment
│
└── index.html                   # HTML entry point
```

## 🔒 Authentication Flow

### Login Process
1. User enters credentials (username, password)
2. `LoginComponent` calls `AuthService.login()`
3. API returns JWT token + user data
4. Token stored in localStorage
5. User redirected to dashboard

### Protected Routes
- Routes are protected by `AuthGuard`
- Guard checks for valid JWT token
- Interceptor automatically adds token to HTTP headers
- On 401 response, user is logged out and redirected to login

## 🛡️ Security Features

### Auth Interceptor (`auth.interceptor.ts`)
- ✅ Automatically injects JWT token in Authorization header
- ✅ Handles 401 (Unauthorized) responses
- ✅ Handles 403 (Forbidden) responses
- ✅ Logs user out on token expiration

### Auth Guard (`auth.guard.ts`)
- ✅ Prevents unauthorized access to protected routes
- ✅ Supports role-based access control
- ✅ Redirects to login if no valid token
- ✅ Redirects to dashboard if insufficient permissions

### Token Management (`auth.service.ts`)
- ✅ JWT token parsing and expiration checking
- ✅ Automatic token refresh mechanism ready
- ✅ User state management with RxJS BehaviorSubject
- ✅ Local storage persistence

## 📋 Features Implemented

### 1. Authentication
- ✅ User Login
- ✅ User Registration
- ✅ Token-based JWT authentication
- ✅ Role-based access control

### 2. Dashboard
- ✅ Responsive sidebar navigation
- ✅ User welcome message
- ✅ Dashboard home page
- ✅ Logout functionality

### 3. Category Management (CRUD)
- ✅ Create new categories
- ✅ Read/View all categories
- ✅ Update existing categories
- ✅ Delete categories with confirmation
- ✅ Filter by active/inactive status
- ✅ Form validation

## 🚀 Getting Started

### Prerequisites
```bash
Node.js 18+
npm or yarn
```

### Installation
```bash
cd MyBlog.Client
npm install
```

### Development Server
```bash
npm start
```
Application will be available at `http://localhost:4200`

### API Configuration
Update the API URL in `src/config/environment.ts`:

**Development:**
```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:44390/api'
};
```

**Production:**
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.yourdomain.com/api'
};
```

## 🔄 API Endpoints Used

### Authentication
- `POST /api/Auth/Login` - User login
- `POST /api/Auth/Register` - User registration

### Categories
- `GET /api/Category` - Get all categories
- `GET /api/Category/{id}` - Get category by ID
- `POST /api/Category` - Create new category
- `PUT /api/Category/{id}` - Update category
- `DELETE /api/Category/{id}` - Delete category
- `GET /api/Category/active` - Get active categories

## 📦 Data Models

### Login Request
```typescript
{
  userName: string;
  password: string;
}
```

### Register Request
```typescript
{
  fullName: string;
  userName: string;
  email: string;
  password: string;
  role?: string;
}
```

### Login Response
```typescript
{
  token: string;  // JWT Token
  user: {
    id: string;
    userName: string;
    email: string;
    fullName: string | null;
    roles: string[];
  }
}
```

### Category Model
```typescript
{
  id: number;
  name: string;
  description?: string;
  isActive: boolean;
  createdDate: Date;
}
```

## 🎨 UI Components

### Login Component
- Username input with validation
- Password input with validation
- Submit button with loading state
- Error message display
- Link to register page

### Register Component
- Full name input
- Username input
- Email input with validation
- Password input
- Confirm password input
- Submit button with loading state
- Link to login page

### Dashboard Layout
- Responsive sidebar navigation
- Header with user info
- Toggle sidebar button
- Logout button
- Router outlet for child components

### Category Component
- Create new category form
- Categories table with all CRUD operations
- Edit functionality
- Delete with confirmation modal
- Active/Inactive status badge
- Form validation
- Error/Success messages

## 🔧 Available Scripts

```bash
# Development server
npm start

# Build for production
npm run build

# Run tests
npm test

# Lint code
npm lint
```

## 📝 Form Validation

### Login Form
- Username: Required, minimum 3 characters
- Password: Required, minimum 6 characters

### Register Form
- Full Name: Required, minimum 3 characters
- Username: Required, minimum 3 characters
- Email: Required, valid email format
- Password: Required, minimum 6 characters
- Confirm Password: Required, must match password

### Category Form
- Name: Required, minimum 3 characters
- Description: Optional
- Is Active: Boolean toggle

## 🎯 Routing Configuration

```
/login                 → Login page
/register             → Registration page
/dashboard            → Dashboard home (Protected)
/dashboard/category   → Category management (Protected)
```

## 🔐 Role-Based Access Control

Routes can be protected by role. Example:
```typescript
{
  path: 'admin',
  component: AdminComponent,
  canActivate: [AuthGuard],
  data: { roles: ['Admin'] }
}
```

## 📱 Responsive Design

- Mobile-first approach
- Breakpoints for tablets (768px) and desktops
- Hamburger menu on mobile devices
- Responsive tables with horizontal scroll on mobile

## 🛠️ Development Tips

### Adding New Protected Routes
1. Create component in `features/` folder
2. Add route to `app.routes.ts` with `canActivate: [AuthGuard]`
3. Access `AuthService` to get current user and roles

### Adding New Services
1. Create service in `core/services/`
2. Inject in component or another service
3. Use with RxJS observables

### Token Expiration
- Tokens are automatically validated on app initialization
- Expired tokens trigger logout and redirect to login

## 🐛 Troubleshooting

### 401 Unauthorized Errors
- Check if token is stored in localStorage
- Verify token hasn't expired
- Check API CORS settings

### CORS Issues
- Ensure API is configured to accept requests from frontend origin
- Check API CORS headers

### Token Not Being Sent
- Check AuthInterceptor is registered in app.config.ts
- Verify token is stored in localStorage with correct key

## 📄 License

This project is part of MyBlog application.

## 👨‍💻 Author

Created as a professional Angular starter template with JWT authentication and role-based access control.
