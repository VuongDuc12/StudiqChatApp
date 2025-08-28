# Hướng dẫn Deploy TluPlannerApi

## Cấu trúc Project
```
TluPlannerApi/
├── PlannerApp-Client/     # Frontend React
├── Ucm.API/              # Backend .NET
├── docker-compose.yml    # File cấu hình Docker Compose
└── README2.md           # Tài liệu hướng dẫn này
```

## Yêu cầu hệ thống
- Docker và Docker Compose
- Git
- Node.js (phát triển local)
- .NET SDK 8.0 (phát triển local)
- SQL Server (hoặc dùng container)

## Các bước deploy

### 1. Chuẩn bị môi trường

```bash
# Tạo thư mục deploy
mkdir -p /var/www/TluPlannerApi
cd /var/www/TluPlannerApi

# Clone repository
git clone https://github.com/ducduy1102/TluPlannerApi.git .
```

### 2. Cấu hình Docker

#### docker-compose.yml
```yaml
version: '3.8'

services:
  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: plannerapp-db
    ports:
      - "1433:1433"
    environment:
      SA_PASSWORD: "123456@A"
      ACCEPT_EULA: "Y"
    volumes:
      - sqlserver_data:/var/opt/mssql
    networks:
      - plannerapp-net

  backend:
    build:
      context: .
      dockerfile: Ucm.API/Dockerfile
    container_name: plannerapp-backend
    ports:
      - "5000:8080"
    depends_on:
      - db
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=TluStudyPlannerApp_DB2;User Id=sa;Password=123456@A;TrustServerCertificate=True
      - Jwt__Key=f7s8#2jk!4NM56qz!X7MnRw4$D1e2BhBt
      - Jwt__Issuer=PlannerApp
      - Jwt__Audience=PlannerApp
    networks:
      - plannerapp-net

  frontend:
    build:
      context: ./PlannerApp-Client
      dockerfile: Dockerfile
    container_name: plannerapp-frontend
    ports:
      - "3000:80"
    depends_on:
      - backend
    networks:
      - plannerapp-net

networks:
  plannerapp-net:
    driver: bridge

volumes:
  sqlserver_data:
```

#### Ucm.API/Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Ucm.API/Ucm.API.csproj", "Ucm.API/"]
COPY ["Ucm.Application/Ucm.Application.csproj", "Ucm.Application/"]
COPY ["Ucm.Domain/Ucm.Domain.csproj", "Ucm.Domain/"]
COPY ["Ucm.Infrastructure/Ucm.Infrastructure.csproj", "Ucm.Infrastructure/"]
COPY ["Ucm.Shared/Ucm.Shared.csproj", "Ucm.Shared/"]
RUN dotnet restore "Ucm.API/Ucm.API.csproj"
COPY . .
WORKDIR "/src/Ucm.API"
RUN dotnet build "Ucm.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ucm.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ucm.API.dll"]
```

#### PlannerApp-Client/Dockerfile
```dockerfile
# Build stage
FROM node:18-alpine as build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Production stage
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

#### PlannerApp-Client/nginx.conf
```nginx
server {
    listen 80;
    server_name localhost;
    root /usr/share/nginx/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /api {
        proxy_pass http://plannerapp-backend:8080;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

### 3. Deploy thủ công

```bash
# Di chuyển đến thư mục project
cd /var/www/TluPlannerApi

# Pull code mới
git pull

# Dừng các container cũ nếu có
docker compose down

# Build và chạy các container mới
docker compose up -d --build
```

### 4. Kiểm tra hoạt động

```bash
# Kiểm tra trạng thái các container
docker ps

# Xem logs backend
docker logs plannerapp-backend

# Xem logs frontend
docker logs plannerapp-frontend

# Xem logs database
docker logs plannerapp-db
```

### 5. Truy cập ứng dụng

- Frontend: http://your-vps-ip:3000
- Backend API: http://your-vps-ip:5000
- Database: your-vps-ip:1433

### 6. Xử lý lỗi thường gặp

1. **Lỗi kết nối database**
   - Kiểm tra logs container db
   - Verify connection string trong environment của backend
   - Đảm bảo container db đã khởi động hoàn tất

2. **Lỗi frontend không load được API**
   - Kiểm tra nginx.conf
   - Verify backend URL trong frontend config
   - Kiểm tra logs nginx

3. **Lỗi permission**
   - Đảm bảo thư mục /var/www có đủ quyền
   - Kiểm tra quyền truy cập volume của SQL Server

### 7. Backup và Restore

#### Backup database
```bash
# Backup trong container
docker exec plannerapp-db /opt/mssql-tools/bin/sqlcmd -S localhost \
   -U sa -P "123456@A" \
   -Q "BACKUP DATABASE TluStudyPlannerApp_DB2 TO DISK = '/var/opt/mssql/backup/TluStudyPlannerApp_DB2.bak'"
```

#### Restore database
```bash
# Restore trong container
docker exec plannerapp-db /opt/mssql-tools/bin/sqlcmd -S localhost \
   -U sa -P "123456@A" \
   -Q "RESTORE DATABASE TluStudyPlannerApp_DB2 FROM DISK = '/var/opt/mssql/backup/TluStudyPlannerApp_DB2.bak'"
```

## Lưu ý quan trọng

1. **Bảo mật**
   - Thay đổi mật khẩu SQL Server trong production
   - Cập nhật JWT secret key
   - Sử dụng HTTPS trong production
   - Hạn chế quyền truy cập ports không cần thiết

2. **Monitoring**
   - Theo dõi logs thường xuyên
   - Kiểm tra disk usage của SQL Server
   - Monitor memory usage của các containers

3. **Maintenance**
   - Backup database định kỳ
   - Cập nhật security patches
   - Dọn dẹp unused images và containers
   - Kiểm tra và clean docker volumes không sử dụng 