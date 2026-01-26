# MyBlog Angular Application - Quick Start Guide

## 📚 Complete Professional Structure Created

Your Angular application now has a complete, production-ready structure with:

### ✅ Core Features
1. **Authentication System**
   - Login & Register components with form validation
   - JWT token management
   - Automatic token injection in HTTP requests
   - Token expiration handling
   - User state management with RxJS

2. **Security**
   - Auth Guard for route protection
   - HTTP Interceptor for JWT token injection
   - Role-based access control ready
   - Automatic logout on token expiration

3. **Dashboard**
   - Responsive layout with sidebar navigation
   - User welcome message
   - Mobile-friendly hamburger menu

4. **Category Management (CRUD)**
   - Create new categories
   - View all categories
   - Edit existing categories
   - Delete categories with confirmation
   - Form validation
   - Active/Inactive status

## 🚀 How to Run

1. **Install Dependencies**
   ```bash
   cd MyBlog.Client
   npm install
   ```

2. **Start Development Server**
   ```bash
   npm start
   ```
   Application will run at `http://localhost:4200`

3. **Your API should be running at:**
   `https://localhost:44390/api`

## 🔧 Environment Configuration

The API URL is configured in:
- Development: `src/config/environment.ts`
- Production: `src/config/environment.prod.ts`

Current configuration:
```typescript
apiUrl: 'https://localhost:44390/api'
```

## 📱 Available Routes

| Route | Description | Protected |
|-------|-------------|-----------|
| `/login` | User login page | No |
| `/register` | User registration page | No |
| `/dashboard` | Dashboard home | Yes ✅ |
| `/dashboard/category` | Category management | Yes ✅ |

## 🔑 Test Login

Use your existing API credentials:
```typescript
POST https://localhost:44390/api/Auth/Login
{
  "userName": "user",
  "password": "your_password"
}
```

Response includes:
- JWT token
- User data (id, userName, email, fullName, roles)

## 📁 Project Structure

```
src/
├── core/                       # Core functionality
│   ├── guards/                 # Auth guard
│   ├── interceptors/           # HTTP interceptor
│   ├── models/                 # TypeScript models
│   └── services/               # Services (Auth, Category)
│
├── features/                   # Feature modules
│   ├── auth/                   # Login & Register
│   └── dashboard/              # Dashboard & Category
│
├── config/                     # Environment config
│   ├── environment.ts
│   └── environment.prod.ts
│
└── app/                        # App root
    ├── app.config.ts           # Providers configuration
    ├── app.routes.ts           # Routing configuration
    └── app.ts                  # Root component
```

## 🎯 Key Features Implemented

### Authentication Service (`core/services/auth.service.ts`)
- `login(credentials)` - User login
- `register(userData)` - User registration
- `logout()` - Clear session
- `getToken()` - Get JWT token
- `getCurrentUser()` - Get logged-in user
- `hasRole(role)` - Check user role
- `hasValidToken()` - Validate token

### Category Service (`core/services/category.service.ts`)
- `getAll()` - Get all categories
- `getById(id)` - Get single category
- `create(category)` - Create new category
- `update(id, category)` - Update category
- `delete(id)` - Delete category
- `getActive()` - Get only active categories

### Auth Guard (`core/guards/auth.guard.ts`)
- Protects routes from unauthorized access
- Redirects to login if no valid token
- Supports role-based access (data: { roles: ['Admin'] })

### Auth Interceptor (`core/interceptors/auth.interceptor.ts`)
- Automatically adds JWT token to all HTTP requests
- Handles 401 (Unauthorized) errors
- Handles 403 (Forbidden) errors
- Auto-logout on token expiration

## 🎨 UI Components

All components are fully styled and responsive:

### Login Component
- Username & password fields
- Form validation with error messages
- Loading state on submit
- Link to register page

### Register Component
- Full name, username, email, password fields
- Password confirmation
- Email validation
- Link to login page

### Dashboard Layout
- Sidebar with navigation menu
- Header with user info
- Logout button
- Responsive design (mobile hamburger menu)

### Category Component
- Create/Edit form with validation
- Data table with all categories
- Edit & Delete actions
- Delete confirmation modal
- Active/Inactive status badges

## ⚙️ Form Validation

### Login Form
- Username: Required, min 3 characters
- Password: Required, min 6 characters

### Register Form
- Full Name: Required, min 3 characters
- Username: Required, min 3 characters
- Email: Required, valid email format
- Password: Required, min 6 characters
- Confirm Password: Must match password

### Category Form
- Name: Required, min 3 characters
- Description: Optional
- Is Active: Boolean toggle

## 🔐 Security Features

1. **JWT Token Storage**: Stored in localStorage
2. **Automatic Token Injection**: Added to all API requests
3. **Token Expiration Check**: Validated on app initialization
4. **Protected Routes**: Auth guard prevents unauthorized access
5. **Error Handling**: Automatic logout on 401 responses
6. **Role-Based Access**: Ready for role restrictions

## 📊 Data Flow

### Login Flow
1. User enters credentials → LoginComponent
2. LoginComponent calls AuthService.login()
3. AuthService sends POST to /api/Auth/Login
4. API returns token + user data
5. Token saved to localStorage
6. User data saved to BehaviorSubject
7. Redirect to /dashboard

### Protected Route Access
1. User navigates to protected route
2. AuthGuard checks for valid token
3. If valid → allow access
4. If invalid → redirect to /login
5. Token automatically added to all API calls

### Category CRUD
1. Component calls CategoryService
2. Service sends HTTP request with token
3. Interceptor adds Authorization header
4. API processes request
5. Response returned to component
6. UI updated

## 🛠️ Customization

### Add New Menu Item
Edit `src/features/dashboard/layout/dashboard-layout.component.html`:
```html
<li>
  <a routerLink="/dashboard/your-feature" routerLinkActive="active">
    <i class="icon">🎨</i>
    Your Feature
  </a>
</li>
```

### Add New Protected Route
Edit `src/app/app.routes.ts`:
```typescript
{
  path: 'dashboard',
  component: DashboardLayoutComponent,
  canActivate: [AuthGuard],
  children: [
    {
      path: 'your-feature',
      component: YourFeatureComponent,
      canActivate: [AuthGuard]
    }
  ]
}
```

## 📝 Next Steps

1. **Run the application**: `npm start`
2. **Test login** with your existing API credentials
3. **Navigate to categories** and test CRUD operations
4. **Customize styles** to match your brand
5. **Add more features** to the dashboard

## 🐛 Troubleshooting

### CORS Errors
Ensure your API allows requests from `http://localhost:4200`:
```csharp
services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
```

### 401 Errors
- Check if token is valid and not expired
- Verify API is running at `https://localhost:44390`
- Check browser console for detailed errors

### Cannot POST to API
- Verify API URL in environment.ts
- Check if API is accepting HTTPS
- Ensure CORS is properly configured

## 📖 Documentation

See [ARCHITECTURE.md](./ARCHITECTURE.md) for detailed documentation about:
- Complete project structure
- All components and services
- API endpoints
- Security implementation
- Development tips

## 🎉 You're All Set!

Your professional Angular application is ready with:
- ✅ Login & Register
- ✅ JWT Authentication
- ✅ Protected Routes
- ✅ Dashboard with Sidebar
- ✅ Category CRUD
- ✅ Responsive Design
- ✅ Form Validation
- ✅ Error Handling

Start the dev server and begin building your blog! 🚀
