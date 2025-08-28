# 📚 StudyIQ - Hệ thống Quản lý Học tập Thông minh

## 🎯 Tổng quan dự án

StudyIQ là một hệ thống quản lý học tập toàn diện được xây dựng với kiến trúc microservices, bao gồm:

- **Backend API**: ASP.NET Core 8.0 với Entity Framework Core
- **Frontend Web**: Next.js 15 với TypeScript và Tailwind CSS
- **Database**: SQL Server với Entity Framework migrations

## 🏗️ Kiến trúc hệ thống

```
Studyiq/
├── backend/                 # Backend API (.NET 8)
│   ├── Ucm.API/            # Web API Controllers
│   ├── Ucm.Application/    # Business Logic & Services
│   ├── Ucm.Domain/         # Domain Entities & Interfaces
│   ├── Ucm.Infrastructure/ # Data Access & External Services
│   └── Ucm.Shared/         # Shared DTOs & Utilities
└── frontend/               # Next.js Web Application
```

## 🚀 Hướng dẫn cài đặt và chạy dự án

### Yêu cầu hệ thống

- **.NET 8.0 SDK** hoặc cao hơn
- **Node.js 18+** và npm/yarn
- **SQL Server** (LocalDB, Express, hoặc Full)
- **Visual Studio 2022** hoặc **VS Code**
- **Git**

### 1. Clone và cài đặt Backend

```bash
# Clone repository
git clone <repository-url>
cd Studyiq

# Di chuyển vào thư mục backend
cd backend

# Restore NuGet packages
dotnet restore

# Cấu hình database connection
# Mở file backend/Ucm.API/appsettings.json và cập nhật:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudyiqDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "your-super-secret-key-here-minimum-16-characters",
    "Issuer": "https://localhost:7001",
    "Audience": "https://localhost:7001"
  }
}

# Chạy migrations để tạo database
cd Ucm.API
dotnet ef database update

# Chạy API
dotnet run
```

API sẽ chạy tại: `https://localhost:7001`
Swagger UI: `https://localhost:7001/swagger`

### 2. Cài đặt Frontend (Next.js)

```bash
# Mở terminal mới, di chuyển vào thư mục frontend
cd frontend

# Cài đặt dependencies
npm install

# Chạy development server
npm run dev
```

Frontend sẽ chạy tại: `http://localhost:3000`



## 📖 Hướng dẫn phát triển cho người mới

### 🎯 Hiểu về kiến trúc Backend

Dự án sử dụng **Clean Architecture** với 4 layer chính:

#### 1. **Ucm.Domain** - Domain Layer
- Chứa các **Entities** (model gốc của business)
- Chứa các **Interfaces** (contracts)
- Chứa các **Enums** và **Value Objects**

**Ví dụ Entity:**
```csharp
// Ucm.Domain/Entities/StudyPlan.cs
public class StudyPlan
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; }
    public List<StudyPlanCourse> StudyPlanCourses { get; set; }
}
```

#### 2. **Ucm.Application** - Application Layer
- Chứa **Services** (business logic)
- Chứa **DTOs** (Data Transfer Objects)
- Chứa **Interfaces** của services

**Ví dụ Service:**
```csharp
// Ucm.Application/Services/StudyPlanService.cs
public class StudyPlanService : IStudyPlanService
{
    private readonly IStudyPlanRepository _studyPlanRepository;
    
    public StudyPlanService(IStudyPlanRepository studyPlanRepository)
    {
        _studyPlanRepository = studyPlanRepository;
    }
    
    public async Task<StudyPlanDto> CreateAsync(StudyPlanCreateRequest request)
    {
        // Business logic here
        var studyPlan = new StudyPlan
        {
            Title = request.Title,
            Description = request.Description,
            // ...
        };
        
        await _studyPlanRepository.AddAsync(studyPlan);
        return _mapper.Map<StudyPlanDto>(studyPlan);
    }
}
```

#### 3. **Ucm.Infrastructure** - Infrastructure Layer
- Chứa **DbContext** và **EF Models**
- Chứa **Repositories** (implementation)
- Chứa **Mappers** (Entity ↔ EF Model)

**Ví dụ Repository:**
```csharp
// Ucm.Infrastructure/Repositories/StudyPlanRepository.cs
public class StudyPlanRepository : RepositoryBase<StudyPlan>, IStudyPlanRepository
{
    public StudyPlanRepository(AppDbContext context) : base(context)
    {
    }
    
    public async Task<List<StudyPlan>> GetByUserIdAsync(Guid userId)
    {
        return await _context.StudyPlans
            .Include(sp => sp.StudyPlanCourses)
            .Where(sp => sp.UserId == userId)
            .ToListAsync();
    }
}
```

#### 4. **Ucm.API** - Presentation Layer
- Chứa **Controllers** (API endpoints)
- Cấu hình **Dependency Injection**
- Cấu hình **Authentication/Authorization**

**Ví dụ Controller:**
```csharp
// Ucm.API/Controllers/StudyPlanController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudyPlanController : ControllerBase
{
    private readonly IStudyPlanService _studyPlanService;
    
    public StudyPlanController(IStudyPlanService studyPlanService)
    {
        _studyPlanService = studyPlanService;
    }
    
    [HttpPost]
    public async Task<ActionResult<StudyPlanDto>> Create(StudyPlanCreateRequest request)
    {
        var result = await _studyPlanService.CreateAsync(request);
        return Ok(result);
    }
}
```

### 🔄 Quy trình phát triển Backend

#### 1. **Tạo Entity mới**
```bash
# 1. Tạo Entity trong Ucm.Domain/Entities/
# 2. Tạo Interface trong Ucm.Domain/IRepositories/
# 3. Tạo EF Model trong Ucm.Infrastructure/Data/Models/
# 4. Tạo Mapper trong Ucm.Infrastructure/Common/Mappers/
# 5. Tạo Repository trong Ucm.Infrastructure/Repositories/
# 6. Tạo DTOs trong Ucm.Application/DTOs/
# 7. Tạo Service Interface và Implementation
# 8. Tạo Controller
# 9. Tạo Migration
dotnet ef migrations add AddNewEntity
dotnet ef database update
```

#### 2. **Ví dụ tạo Entity "Course"**

**Bước 1: Tạo Entity**
```csharp
// Ucm.Domain/Entities/Course.cs
public class Course
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; } // minutes
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

**Bước 2: Tạo Repository Interface**
```csharp
// Ucm.Domain/IRepositories/ICourseRepository.cs
public interface ICourseRepository : IRepositoryBase<Course>
{
    Task<List<Course>> GetByNameAsync(string name);
}
```

**Bước 3: Tạo EF Model**
```csharp
// Ucm.Infrastructure/Data/Models/CourseEf.cs
public class CourseEf
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

**Bước 4: Tạo Mapper**
```csharp
// Ucm.Infrastructure/Common/Mappers/CourseEntityEfMapper.cs
public class CourseEntityEfMapper : IEntityEfMapper<Course, CourseEf>
{
    public Course MapToEntity(CourseEf ef)
    {
        return new Course
        {
            Id = ef.Id,
            Name = ef.Name,
            Description = ef.Description,
            Duration = ef.Duration,
            CreatedAt = ef.CreatedAt,
            UpdatedAt = ef.UpdatedAt
        };
    }

    public CourseEf MapToEf(Course entity)
    {
        return new CourseEf
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Duration = entity.Duration,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
```

### 🎨 Hướng dẫn phát triển Frontend

#### 1. **Cấu trúc Next.js App**
```
frontend/src/
├── app/                    # App Router (Next.js 13+)
│   ├── layout.tsx         # Root layout
│   ├── page.tsx           # Home page
│   └── globals.css        # Global styles
├── components/            # Reusable components
├── features/              # Feature-based components
├── hooks/                 # Custom React hooks
├── lib/                   # Utility functions
├── styles/                # Additional styles
└── types/                 # TypeScript type definitions
```

#### 2. **Tạo Component mới**
```typescript
// src/components/StudyPlanCard.tsx
import React from 'react';

interface StudyPlanCardProps {
  title: string;
  description: string;
  startDate: string;
  endDate: string;
  onEdit?: () => void;
  onDelete?: () => void;
}

export const StudyPlanCard: React.FC<StudyPlanCardProps> = ({
  title,
  description,
  startDate,
  endDate,
  onEdit,
  onDelete
}) => {
  return (
    <div className="bg-white rounded-lg shadow-md p-6">
      <h3 className="text-xl font-semibold mb-2">{title}</h3>
      <p className="text-gray-600 mb-4">{description}</p>
      <div className="flex justify-between text-sm text-gray-500">
        <span>Start: {startDate}</span>
        <span>End: {endDate}</span>
      </div>
      <div className="flex gap-2 mt-4">
        {onEdit && (
          <button 
            onClick={onEdit}
            className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
          >
            Edit
          </button>
        )}
        {onDelete && (
          <button 
            onClick={onDelete}
            className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600"
          >
            Delete
          </button>
        )}
      </div>
    </div>
  );
};
```

#### 3. **Tạo API Service**
```typescript
// src/lib/api.ts
const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'https://localhost:7001/api';

export class ApiService {
  private static async request<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<T> {
    const url = `${API_BASE_URL}${endpoint}`;
    const token = localStorage.getItem('token');
    
    const config: RequestInit = {
      headers: {
        'Content-Type': 'application/json',
        ...(token && { Authorization: `Bearer ${token}` }),
        ...options.headers,
      },
      ...options,
    };

    const response = await fetch(url, config);
    
    if (!response.ok) {
      throw new Error(`API Error: ${response.statusText}`);
    }
    
    return response.json();
  }

  // Study Plans
  static async getStudyPlans(): Promise<StudyPlan[]> {
    return this.request<StudyPlan[]>('/studyplan');
  }

  static async createStudyPlan(data: CreateStudyPlanRequest): Promise<StudyPlan> {
    return this.request<StudyPlan>('/studyplan', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }

  static async updateStudyPlan(id: string, data: UpdateStudyPlanRequest): Promise<StudyPlan> {
    return this.request<StudyPlan>(`/studyplan/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  }

  static async deleteStudyPlan(id: string): Promise<void> {
    return this.request<void>(`/studyplan/${id}`, {
      method: 'DELETE',
    });
  }
}
```

### 🔐 Authentication & Authorization

#### 1. **JWT Authentication**
Dự án sử dụng JWT Bearer tokens:

```csharp
// Đăng ký JWT trong Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
```

#### 2. **Sử dụng trong Controller**
```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize] // Yêu cầu authentication
public class StudyPlanController : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")] // Yêu cầu role Admin
    public async Task<ActionResult<List<StudyPlanDto>>> GetAll()
    {
        // Implementation
    }
}
```

### 🗄️ Database Management

#### 1. **Tạo Migration**
```bash
cd backend/Ucm.API
dotnet ef migrations add MigrationName
```

#### 2. **Cập nhật Database**
```bash
dotnet ef database update
```

#### 3. **Rollback Migration**
```bash
dotnet ef database update PreviousMigrationName
```

### 🧪 Testing

#### 1. **Unit Tests**
```bash
cd backend/Ucm.Tests
dotnet test
```

#### 2. **API Testing với Swagger**
- Truy cập: `https://localhost:7001/swagger`
- Test các endpoints trực tiếp
- Authorize với JWT token

### 🚀 Deployment

#### 1. **Build Backend**
```bash
cd backend/Ucm.API
dotnet publish -c Release -o ./publish
```

#### 2. **Build Frontend**
```bash
cd frontend
npm run build
```

#### 3. **Docker (nếu có)**
```bash
# Backend
cd backend
docker build -t studyiq-api .

# Frontend
cd frontend
docker build -t studyiq-frontend .
```

### 📝 Coding Standards

#### 1. **C# Conventions**
- Sử dụng **PascalCase** cho classes, methods, properties
- Sử dụng **camelCase** cho variables, parameters
- Sử dụng **UPPER_CASE** cho constants
- Thêm **XML documentation** cho public APIs

#### 2. **TypeScript/JavaScript Conventions**
- Sử dụng **camelCase** cho variables, functions
- Sử dụng **PascalCase** cho components, classes
- Sử dụng **UPPER_CASE** cho constants
- Sử dụng **TypeScript** strict mode

#### 3. **Git Conventions**
```
feat: add new feature
fix: bug fix
docs: documentation changes
style: formatting changes
refactor: code refactoring
test: add tests
chore: maintenance tasks
```

### 🆘 Troubleshooting

#### 1. **Database Connection Issues**
- Kiểm tra connection string trong `appsettings.json`
- Đảm bảo SQL Server đang chạy
- Kiểm tra firewall settings

#### 2. **CORS Issues**
- Kiểm tra CORS policy trong `Program.cs`
- Đảm bảo frontend URL được allow

#### 3. **JWT Issues**
- Kiểm tra JWT configuration trong `appsettings.json`
- Đảm bảo key có đủ 16 ký tự
- Kiểm tra token expiration

#### 4. **Build Issues**
```bash
# Clean và rebuild
dotnet clean
dotnet restore
dotnet build

# Frontend
npm clean-install
npm run build
```

### 📚 Tài liệu tham khảo

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Next.js Documentation](https://nextjs.org/docs)
- [React Documentation](https://react.dev/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [Tailwind CSS](https://tailwindcss.com/docs)

### 👥 Team Development

#### 1. **Branch Strategy**
```
main          # Production code
develop       # Development branch
feature/*     # Feature branches
hotfix/*      # Hotfix branches
```

#### 2. **Pull Request Process**
1. Tạo feature branch từ `develop`
2. Code và test locally
3. Push và tạo Pull Request
4. Code review
5. Merge vào `develop`

## 🚀 Hướng dẫn Deploy và CI/CD cho người mới

### 🎯 Tổng quan về CI/CD

**CI/CD** (Continuous Integration/Continuous Deployment) là quá trình tự động hóa việc build, test và deploy code lên server. Với StudyIQ, khi bạn push code lên GitHub, hệ thống sẽ tự động:

1. **Chạy tests** để kiểm tra code
2. **Build** ứng dụng thành Docker images
3. **Deploy** lên VPS (Virtual Private Server)
4. **Kiểm tra** xem ứng dụng có hoạt động không

### 📋 Chuẩn bị trước khi deploy

#### 1. **Mua VPS (Virtual Private Server)**

**Các nhà cung cấp VPS phổ biến:**
- **DigitalOcean** (Droplet $5-10/tháng)
- **Vultr** (VPS $5-10/tháng)
- **Linode** (Nanode $5/tháng)
- **AWS EC2** (t2.micro free tier)
- **Google Cloud** (f1-micro free tier)

**Yêu cầu VPS tối thiểu:**
- **RAM**: 2GB trở lên
- **CPU**: 1 core trở lên
- **Storage**: 20GB trở lên
- **OS**: Ubuntu 20.04 hoặc 22.04

#### 2. **Mua Domain (tùy chọn)**

**Các nhà cung cấp domain:**
- **Namecheap** ($10-15/năm)
- **GoDaddy** ($10-15/năm)
- **Google Domains** ($12/năm)

### 🔧 Bước 1: Cấu hình GitHub Repository

#### 1. **Tạo SSH Key cho GitHub Actions**

```bash
# Tạo SSH key pair
ssh-keygen -t rsa -b 4096 -C "github-actions@studyiq.com"

# Xem public key (copy key này)
cat ~/.ssh/id_rsa.pub

# Xem private key (copy toàn bộ nội dung này)
cat ~/.ssh/id_rsa
```

#### 2. **Thêm SSH Key vào VPS**

```bash
# SSH vào VPS
ssh root@your-vps-ip

# Tạo user mới (không dùng root)
adduser studyiq
usermod -aG sudo studyiq

# Copy public key lên VPS
# (Paste public key vào file ~/.ssh/authorized_keys)
nano ~/.ssh/authorized_keys
```

#### 3. **Cấu hình GitHub Secrets**

Vào **GitHub Repository > Settings > Secrets and variables > Actions** và thêm:

```bash
# VPS Production
VPS_HOST=your-vps-ip
VPS_USERNAME=studyiq
VPS_SSH_KEY=your-private-ssh-key-content
VPS_PORT=22
```

### 🖥️ Bước 2: Cài đặt VPS

#### 1. **SSH vào VPS**

```bash
ssh studyiq@your-vps-ip
```

#### 2. **Chạy Installation Script**

```bash
# Clone repository
git clone https://github.com/your-username/studyiq.git /opt/studyiq

# Chạy installation script
chmod +x /opt/studyiq/deploy/install-vps.sh
/opt/studyiq/deploy/install-vps.sh
```

**Script này sẽ tự động:**
- Cài đặt Docker và Docker Compose
- Cấu hình Nginx web server
- Cài đặt firewall và security
- Tạo monitoring scripts
- Thiết lập backup system

#### 3. **Cấu hình Environment Variables**

```bash
# Copy environment template
cp /opt/studyiq/env.example /opt/studyiq/.env

# Edit environment variables
nano /opt/studyiq/.env
```

**Nội dung file `.env`:**
```bash
# Database Configuration
DB_PASSWORD=YourStrong@Passw0rd123

# JWT Configuration
JWT_KEY=your-super-secret-key-here-minimum-32-characters-for-production
JWT_ISSUER=https://yourdomain.com
JWT_AUDIENCE=https://yourdomain.com

# API Configuration
API_URL=https://yourdomain.com/api

# Redis Configuration
REDIS_PASSWORD=your-redis-password-here

# Environment
NODE_ENV=production
ASPNETCORE_ENVIRONMENT=Production
```

### 🌐 Bước 3: Cấu hình Domain và SSL (tùy chọn)

#### 1. **Cấu hình DNS**

Nếu bạn có domain, thêm DNS record:
```
Type: A
Name: @
Value: your-vps-ip
TTL: 300
```

#### 2. **Cấu hình SSL Certificate**

```bash
# Chạy SSL setup script
chmod +x /opt/studyiq/deploy/setup-ssl.sh
/opt/studyiq/deploy/setup-ssl.sh yourdomain.com admin@yourdomain.com
```

### 🚀 Bước 4: Deploy lần đầu

#### 1. **Clone Repository và Deploy**

```bash
# Clone repository
cd /opt/studyiq
git clone https://github.com/your-username/studyiq.git .

# Deploy application
docker-compose -f docker-compose.prod.yml --env-file .env up -d
```

#### 2. **Kiểm tra Deployment**

```bash
# Kiểm tra containers
docker-compose -f docker-compose.prod.yml ps

# Xem logs
docker-compose -f docker-compose.prod.yml logs -f

# Kiểm tra health
curl http://localhost/health
```

### 🔄 Bước 5: Tự động Deploy

#### 1. **Push Code lên GitHub**

```bash
# Commit và push code
git add .
git commit -m "feat: add new feature"
git push origin main
```

#### 2. **Theo dõi Deployment**

Vào **GitHub Repository > Actions** để xem:
- ✅ Tests đang chạy
- 🐳 Docker images đang build
- 🚀 Deployment đang diễn ra

#### 3. **Kiểm tra kết quả**

Sau khi deployment hoàn tất:
- Truy cập: `https://yourdomain.com` (nếu có domain)
- Hoặc: `http://your-vps-ip` (nếu không có domain)

### 📊 Monitoring và Maintenance

#### 1. **Kiểm tra trạng thái**

```bash
# SSH vào VPS
ssh studyiq@your-vps-ip

# Kiểm tra containers
docker-compose -f /opt/studyiq/docker-compose.prod.yml ps

# Xem logs
docker-compose -f /opt/studyiq/docker-compose.prod.yml logs -f api
```

#### 2. **Backup Database**

```bash
# Manual backup
/opt/studyiq/backup.sh

# Kiểm tra backups
ls -la /opt/backups/studyiq/
```

#### 3. **Restart Services**

```bash
# Restart toàn bộ ứng dụng
sudo systemctl restart studyiq

# Hoặc restart từng service
docker-compose -f /opt/studyiq/docker-compose.prod.yml restart api
```

### 🐛 Troubleshooting cho người mới

#### 1. **Deployment thất bại**

**Kiểm tra GitHub Actions:**
1. Vào **GitHub Repository > Actions**
2. Click vào workflow bị lỗi
3. Xem logs để tìm nguyên nhân

**Kiểm tra VPS:**
```bash
# SSH vào VPS
ssh studyiq@your-vps-ip

# Kiểm tra containers
docker-compose -f /opt/studyiq/docker-compose.prod.yml ps

# Xem logs lỗi
docker-compose -f /opt/studyiq/docker-compose.prod.yml logs -f
```

#### 2. **Website không load được**

```bash
# Kiểm tra Nginx
sudo systemctl status nginx

# Kiểm tra firewall
sudo ufw status

# Kiểm tra ports
sudo netstat -tlnp | grep :80
sudo netstat -tlnp | grep :443
```

#### 3. **Database lỗi**

```bash
# Kiểm tra database container
docker-compose -f /opt/studyiq/docker-compose.prod.yml logs sqlserver

# Kiểm tra kết nối database
docker-compose -f /opt/studyiq/docker-compose.prod.yml exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $DB_PASSWORD -Q "SELECT 1"
```

#### 4. **SSL Certificate lỗi**

```bash
# Kiểm tra SSL certificate
sudo certbot certificates

# Renew SSL certificate
sudo certbot renew

# Kiểm tra Nginx config
sudo nginx -t
```

### 📚 Lệnh hữu ích cho người mới

#### **Kiểm tra trạng thái:**
```bash
# Kiểm tra containers
docker ps

# Kiểm tra logs
docker-compose -f /opt/studyiq/docker-compose.prod.yml logs -f

# Kiểm tra disk space
df -h

# Kiểm tra memory
free -h
```

#### **Restart services:**
```bash
# Restart toàn bộ
sudo systemctl restart studyiq

# Restart từng service
docker-compose -f /opt/studyiq/docker-compose.prod.yml restart api
docker-compose -f /opt/studyiq/docker-compose.prod.yml restart frontend
```

#### **Update code:**
```bash
# Pull latest code
cd /opt/studyiq
git pull origin main

# Pull latest images
docker-compose -f docker-compose.prod.yml pull

# Restart với code mới
docker-compose -f docker-compose.prod.yml up -d
```

### 🎯 Workflow cho người mới

#### **Quy trình làm việc hàng ngày:**

1. **Code locally** → Test trên máy local
2. **Commit code** → `git add . && git commit -m "message"`
3. **Push lên GitHub** → `git push origin main`
4. **GitHub Actions** → Tự động test và deploy
5. **Kiểm tra kết quả** → Truy cập website

#### **Khi có lỗi:**

1. **Kiểm tra GitHub Actions** → Xem logs lỗi
2. **SSH vào VPS** → Kiểm tra containers
3. **Xem logs** → Tìm nguyên nhân
4. **Fix lỗi** → Commit và push lại
5. **Redeploy** → Tự động hoặc manual

### 💡 Tips cho người mới

#### **1. Luôn test trước khi push:**
```bash
# Test backend
cd backend
dotnet test

# Test frontend
cd frontend
npm test
```

#### **2. Commit message rõ ràng:**
```bash
git commit -m "feat: add user authentication"
git commit -m "fix: resolve database connection issue"
git commit -m "docs: update README"
```

#### **3. Backup thường xuyên:**
```bash
# Manual backup
/opt/studyiq/backup.sh

# Kiểm tra backups
ls -la /opt/backups/studyiq/
```

#### **4. Monitor resources:**
```bash
# Kiểm tra disk space
df -h

# Kiểm tra memory
free -h

# Kiểm tra CPU
htop
```

### 📞 Hỗ trợ

Nếu gặp vấn đề:

1. **Kiểm tra logs** trước
2. **Google search** lỗi
3. **Tạo issue** trên GitHub
4. **Liên hệ team lead**

### 📚 Tài liệu tham khảo

- [Docker Documentation](https://docs.docker.com/)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Nginx Documentation](https://nginx.org/en/docs/)
- [Let's Encrypt Documentation](https://letsencrypt.org/docs/)

---

**Chúc bạn deploy thành công! 🚀** #   S t u d i q P r o j e c t F u l l  
 #   S t u d i q C h a t A p p  
 