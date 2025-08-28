# 📦 UCM Clean Architecture Backend

Dự án **UCM Backend** được xây dựng theo mô hình **Clean Architecture**, sử dụng **ASP.NET Core Web API**, **Entity Framework Core**, **JWT Authentication**, và hỗ trợ mở rộng dễ dàng theo các best practices.

---

## 🔧 Cấu trúc dự án
Dự án được tổ chức theo các tầng của Clean Architecture để đảm bảo tính phân tách rõ ràng và khả năng bảo trì:

- **Ucm.API**: Project khởi chạy API chính (Startup Project). Đây là điểm vào cho các yêu cầu HTTP.
- **Ucm.Application**: Chứa các logic nghiệp vụ cốt lõi (Use Cases), các đối tượng truyền dữ liệu (DTOs), các giao diện (Interfaces) cho các dịch vụ ứng dụng, v.v.
- **Ucm.Domain**: Định nghĩa các thực thể thuần (Entities), Enum, Value Objects, và các quy tắc nghiệp vụ cốt lõi không phụ thuộc vào bất kỳ công nghệ nào.
- **Ucm.Infrastructure**: Thực thi các giao diện Repository, cài đặt Entity Framework Core DbContext, và cấu hình cơ sở dữ liệu. Tầng này chứa các chi tiết triển khai cụ thể.
- **Ucm.Shared**: Chứa các thư viện và tiện ích dùng chung (Constants, Helpers, Extensions methods, v.v.) được sử dụng xuyên suốt các tầng.
- **Ucm.Tests**: Chứa các dự án kiểm thử (Unit Tests, Integration Tests) cho các tầng của ứng dụng.

> 🔁 **Lưu ý về Dependency Injection**: Mỗi dự án (**Ucm.Application**, **Ucm.Infrastructure**) chịu trách nhiệm đăng ký các dịch vụ và phụ thuộc riêng qua các phương thức mở rộng (extension methods), sau đó được tích hợp vào `Ucm.API`. Điều này đảm bảo tính **modular** và tuân thủ nguyên tắc **tách biệt mối quan tâm (Separation of Concerns)**.

---

## 🚀 Công nghệ sử dụng

Dự án tận dụng các công nghệ và thư viện hiện đại để đảm bảo hiệu suất, bảo mật và khả năng mở rộng:

- ✅ **ASP.NET Core 8**
- ✅ **Entity Framework Core**
- ✅ **SQL Server**
- ✅ **JWT Authentication**
- ✅ **Clean Architecture**
- ✅ **Swagger UI**
- ✅ **AutoMapper**
- ✅ **FluentValidation**

---

## ⚙️ Cài đặt & chạy dự án

### 1. Clone repository:
```bash
https://github.com/VuongDuc12/BaseProject_NetApi
cd BaseProject_NetApi
```

### 2. Cấu hình `appsettings.Development.json`:
Tạo một file mới tên `appsettings.Development.json` trong thư mục `Ucm.API/` (đã được `.gitignore`) với nội dung sau:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=UcmDb;User Id=sa;Password=123456;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "UcmDbServer",
    "Audience": "UcmDbClient"
  }
}
```

### 3. Cài đặt các gói phụ thuộc:
```bash
dotnet restore
```

### 4. Tạo và cập nhật database:
```bash
dotnet ef migrations add InitialCreate -p Ucm.Infrastructure -s Ucm.API
dotnet ef database update -p Ucm.Infrastructure -s Ucm.API
```

### 5. Chạy ứng dụng:
```bash
dotnet run --project Ucm.API
```

---

## 🔑 Tài khoản mặc định (dành cho testing)
```json
{
  "username": "sa",
  "password": "123456"
}
```
> 📝 Bạn có thể thay đổi thông tin đăng nhập này trong database hoặc khi tích hợp hệ thống Identity phức tạp hơn.

---

## 🔐 Swagger UI
Khi ứng dụng chạy thành công, bạn có thể truy cập:

🌐 [https://localhost:5001/swagger](https://localhost:5001/swagger)

Tại đây, bạn có thể kiểm tra và thử nghiệm các API endpoints dễ dàng.

---

## 📁 Ghi chú về bảo mật

- ❌ Không đẩy file `appsettings.Development.json` lên Git.
- ❌ Không public `Jwt:Key` hoặc `ConnectionStrings` thật.
- ✅ Mỗi môi trường (**Development**, **Staging**, **Production**) nên có file cấu hình riêng: `appsettings.{Environment}.json`.

---

## 🛠 Đang phát triển

Các tính năng đang được phát triển:

- [ ] API xác thực (Auth) với Identity + JWT
- [ ] Quản lý người dùng
- [ ] Hệ thống phân quyền (Role & Permissions)
- [ ] CRUD cho các entity chính (Match, Transaction, ...)
- [ ] Viết Unit Test cho các tầng và logic nghiệp vụ

---

## 💡 Đóng góp

Mọi ý kiến đóng góp, đề xuất tính năng (Pull Request) hoặc báo lỗi (Issue) đều được hoan nghênh và khuyến khích!

---

## 📜 License

Dự án này được cấp phép theo **MIT License**.

© 2025 Duc Dev