# MyBlog Angular - Commands Reference

## 🚀 Quick Start Commands

```bash
# Navigate to project directory
cd MyBlog.Client

# Install all dependencies
npm install

# Start development server
npm start

# Application will run at http://localhost:4200
```

---

## 📦 NPM Commands

### **Development**
```bash
# Start development server with live reload
npm start

# Start with specific port
ng serve --port 4300

# Start with open browser
ng serve --open
```

### **Build**
```bash
# Build for development
npm run build

# Build for production
ng build --configuration production

# Build with source maps
ng build --source-map
```

### **Testing**
```bash
# Run unit tests
npm test

# Run tests with coverage
ng test --code-coverage

# Run end-to-end tests
npm run e2e
```

### **Code Quality**
```bash
# Lint TypeScript files
npm run lint

# Format code
npx prettier --write "src/**/*.{ts,html,css}"
```

---

## 🔧 Angular CLI Commands

### **Generate Components**
```bash
# Generate new component
ng generate component features/blog/post

# Generate component with inline template
ng generate component features/comment --inline-template

# Generate component with module
ng generate component features/tags --module=app
```

### **Generate Services**
```bash
# Generate service
ng generate service core/services/blog

# Generate service with skip tests
ng generate service core/services/comment --skip-tests
```

### **Generate Other Files**
```bash
# Generate guard
ng generate guard core/guards/admin

# Generate interceptor
ng generate interceptor core/interceptors/logging

# Generate pipe
ng generate pipe shared/pipes/truncate

# Generate directive
ng generate directive shared/directives/highlight
```

---

## 🗂️ Project Commands

### **Navigate to Directories**
```bash
# Go to project root
cd D:/FullStack/MyBlog/Frontend/MyBlog.Client

# Go to src directory
cd src

# Go to components
cd src/features/dashboard/category
```

### **View Files**
```bash
# List all TypeScript files
dir /s /b *.ts

# List all components
dir /s /b *component.ts

# List all services
dir /s /b *service.ts
```

---

## 🔍 Debugging Commands

### **Check for Errors**
```bash
# Compile and check for errors
ng build --configuration development

# TypeScript check
npx tsc --noEmit
```

### **View Logs**
```bash
# View Angular CLI version
ng version

# View npm version
npm --version

# View Node version
node --version
```

---

## 🌐 API Testing Commands

### **Using PowerShell**
```powershell
# Test Login endpoint
$body = @{
    userName = "user"
    password = "your_password"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:44390/api/Auth/Login" `
    -Method POST `
    -Body $body `
    -ContentType "application/json"

# Test Categories endpoint with token
$headers = @{
    Authorization = "Bearer YOUR_TOKEN_HERE"
}

Invoke-RestMethod -Uri "https://localhost:44390/api/Category" `
    -Method GET `
    -Headers $headers
```

### **Using curl**
```bash
# Test Login
curl -X POST https://localhost:44390/api/Auth/Login \
  -H "Content-Type: application/json" \
  -d '{"userName":"user","password":"your_password"}'

# Test Categories
curl -X GET https://localhost:44390/api/Category \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## 📝 Git Commands (if using version control)

```bash
# Initialize git repository
git init

# Add all files
git add .

# Commit changes
git commit -m "Initial commit: Complete Angular application with auth & CRUD"

# Create .gitignore
echo "node_modules/" > .gitignore
echo "dist/" >> .gitignore
echo ".angular/" >> .gitignore
echo "*.log" >> .gitignore

# Push to remote
git remote add origin YOUR_REPO_URL
git push -u origin main
```

---

## 🔐 Environment Commands

### **Switch Between Environments**
```bash
# Build for development
ng build --configuration development

# Build for production
ng build --configuration production

# Serve with production config
ng serve --configuration production
```

### **Environment File Locations**
- Development: `src/config/environment.ts`
- Production: `src/config/environment.prod.ts`

---

## 🧹 Cleanup Commands

```bash
# Remove node_modules
rm -rf node_modules

# Clear npm cache
npm cache clean --force

# Reinstall dependencies
npm install

# Remove dist folder
rm -rf dist

# Remove .angular cache
rm -rf .angular
```

---

## 📊 Useful Analysis Commands

### **Bundle Analysis**
```bash
# Install analyzer
npm install --save-dev webpack-bundle-analyzer

# Build with stats
ng build --stats-json

# Analyze bundle
npx webpack-bundle-analyzer dist/my-blog-client/stats.json
```

### **Code Statistics**
```bash
# Count TypeScript lines
find src -name "*.ts" | xargs wc -l

# Count component files
find src -name "*component.ts" | wc -l

# Count all files
find src -type f | wc -l
```

---

## 🚢 Deployment Commands

### **Build for Production**
```bash
# Production build with optimization
ng build --configuration production --optimization

# Build with AOT compilation
ng build --aot

# Build with base href
ng build --base-href /myblog/
```

### **Deploy to Server**
```bash
# Build
ng build --configuration production

# Files will be in dist/my-blog-client/browser/
# Upload these files to your web server
```

---

## 🛠️ Troubleshooting Commands

### **Fix Common Issues**
```bash
# Clear cache and reinstall
rm -rf node_modules package-lock.json
npm cache clean --force
npm install

# Update Angular CLI
npm install -g @angular/cli@latest

# Update project dependencies
ng update @angular/core @angular/cli

# Fix peer dependency issues
npm install --legacy-peer-deps
```

### **Check Configuration**
```bash
# View Angular configuration
cat angular.json

# View TypeScript configuration
cat tsconfig.json

# View package.json
cat package.json
```

---

## 🎯 Testing Workflow

```bash
# 1. Start development server
npm start

# 2. In another terminal, run tests
npm test

# 3. Check coverage
ng test --code-coverage

# 4. View coverage report
start coverage/index.html  # Windows
open coverage/index.html   # Mac
```

---

## 📱 Mobile Testing

```bash
# Serve on specific IP for mobile testing
ng serve --host 0.0.0.0 --port 4200

# Your mobile device can access at:
# http://YOUR_COMPUTER_IP:4200
```

---

## 🔄 Update Commands

```bash
# Check for outdated packages
npm outdated

# Update all packages
npm update

# Update Angular
ng update @angular/core @angular/cli

# Update specific package
npm update package-name
```

---

## 📚 Documentation Commands

```bash
# Generate documentation with Compodoc
npm install -g @compodoc/compodoc

# Generate docs
compodoc -p tsconfig.json

# Serve docs
compodoc -s
```

---

## ⚡ Performance Commands

```bash
# Build with performance budget
ng build --stats-json

# Analyze bundle size
ng build --stats-json
npx webpack-bundle-analyzer dist/stats.json

# Run performance audit
npm audit

# Fix vulnerabilities
npm audit fix
```

---

## 🎨 Style Commands

```bash
# Add CSS framework (if needed)
npm install bootstrap
# OR
npm install tailwindcss

# Add Angular Material (optional)
ng add @angular/material
```

---

## 📦 Package Management

```bash
# Install specific package
npm install package-name

# Install dev dependency
npm install --save-dev package-name

# Uninstall package
npm uninstall package-name

# List installed packages
npm list --depth=0

# Check package version
npm view package-name version
```

---

## 🔍 Search Commands

```bash
# Search in files (Windows PowerShell)
Select-String -Path "src/**/*.ts" -Pattern "AuthService"

# Find files by name
Get-ChildItem -Path src -Recurse -Filter "*component.ts"

# Count lines of code
(Get-Content src/**/*.ts | Measure-Object -Line).Lines
```

---

## ✅ Verification Commands

```bash
# Verify installation
node --version
npm --version
ng version

# Check project health
npm doctor

# Verify build
ng build --configuration development

# Check for errors
ng lint
```

---

## 🎯 Quick Reference

| Task | Command |
|------|---------|
| Start dev server | `npm start` |
| Build for prod | `npm run build` |
| Run tests | `npm test` |
| Check errors | `ng build` |
| Generate component | `ng g c component-name` |
| Generate service | `ng g s service-name` |
| Update dependencies | `npm update` |
| Clear cache | `npm cache clean --force` |

---

**💡 Pro Tip**: Create aliases for frequently used commands in your terminal configuration!

---

*For more details, see [QUICKSTART.md](./QUICKSTART.md) and [ARCHITECTURE.md](./ARCHITECTURE.md)*
