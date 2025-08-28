# TLU Planner API Documentation

## Base URL
```
http://localhost:5000/api
```

## Authentication
All endpoints require JWT Bearer token authentication except for login and register.

### Headers
```
Authorization: Bearer <your-jwt-token>
Content-Type: application/json
```

---

## 🔐 Authentication Endpoints

### POST /api/auth/login
**Description:** User login
**Access:** Public

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": "user-guid",
      "email": "user@example.com",
      "fullName": "John Doe",
      "role": "User"
    }
  }
}
```

### POST /api/auth/register
**Description:** User registration
**Access:** Public

**Request Body:**
```json
{
  "email": "newuser@example.com",
  "password": "password123",
  "fullName": "John Doe"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": "user-guid",
      "email": "newuser@example.com",
      "fullName": "John Doe",
      "role": "User"
    }
  }
}
```

---

## 👤 User Endpoints

### 📚 Study Plans

#### GET /api/studyplan/user
**Description:** Get all study plans for current user
**Access:** User

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "planName": "Spring 2024",
      "startDate": "2024-01-15",
      "endDate": "2024-05-15",
      "semester": "Spring",
      "academicYear": "2024",
      "weeklyStudyGoalHours": 20,
      "completed": false,
      "courseCount": 5,
      "planCourses": [...]
    }
  ]
}
```

#### GET /api/studyplan/{id}
**Description:** Get study plan by ID
**Access:** User (owner only)

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "planName": "Spring 2024",
    "startDate": "2024-01-15",
    "endDate": "2024-05-15",
    "semester": "Spring",
    "academicYear": "2024",
    "weeklyStudyGoalHours": 20,
    "completed": false,
    "courseCount": 5,
    "planCourses": [...]
  }
}
```

#### POST /api/studyplan
**Description:** Create new study plan
**Access:** User

**Request Body:**
```json
{
  "planName": "Fall 2024",
  "startDate": "2024-09-01",
  "endDate": "2024-12-15",
  "semester": "Fall",
  "academicYear": "2024",
  "weeklyStudyGoalHours": 25
}
```

#### PUT /api/studyplan/{id}
**Description:** Update study plan
**Access:** User (owner only)

**Request Body:**
```json
{
  "id": 1,
  "planName": "Updated Plan Name",
  "startDate": "2024-09-01",
  "endDate": "2024-12-15",
  "semester": "Fall",
  "academicYear": "2024",
  "weeklyStudyGoalHours": 30,
  "completed": false
}
```

#### DELETE /api/studyplan/{id}
**Description:** Delete study plan
**Access:** User (owner only)

#### GET /api/studyplan/user-summary
**Description:** Get user study plan summary with statistics
**Access:** User

**Response:**
```json
{
  "success": true,
  "data": {
    "userId": "user-guid",
    "totalPlans": 3,
    "activePlans": 2,
    "completedPlans": 1,
    "pendingPlans": 0,
    "studyPlans": [...]
  }
}
```

### 📖 Study Plan Courses

#### GET /api/studyplancourse/plan/{studyPlanId}
**Description:** Get all courses for a study plan
**Access:** User (owner only)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "studyPlanId": 1,
      "courseId": 1,
      "course": {
        "id": 1,
        "courseCode": "CS101",
        "courseName": "Introduction to Computer Science",
        "credits": 3,
        "description": "Basic computer science concepts"
      },
      "tasks": [...]
    }
  ]
}
```

#### POST /api/studyplancourse
**Description:** Add course to study plan
**Access:** User (owner only)

**Request Body:**
```json
{
  "studyPlanId": 1,
  "courseId": 2
}
```

#### DELETE /api/studyplancourse/{id}
**Description:** Remove course from study plan
**Access:** User (owner only)

### 📝 Study Tasks

#### GET /api/studytask/date/{date}
**Description:** Get tasks for specific date
**Access:** User

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "taskName": "Complete Assignment 1",
      "description": "Finish the programming assignment",
      "scheduledDate": "2024-01-15",
      "dueDate": "2024-01-20",
      "status": "Pending",
      "priority": "High",
      "planCourse": {
        "id": 1,
        "course": {
          "courseCode": "CS101",
          "courseName": "Introduction to Computer Science"
        }
      },
      "courseTopic": {
        "id": 1,
        "topicName": "Variables and Data Types"
      },
      "logs": [...],
      "resources": [...]
    }
  ]
}
```

#### GET /api/studytask/week/{weekStart}
**Description:** Get tasks for a week
**Access:** User

#### GET /api/studytask/month/{year}/{month}
**Description:** Get tasks for a month
**Access:** User

#### GET /api/studytask/plan/{studyPlanId}
**Description:** Get all tasks for a study plan
**Access:** User (owner only)

#### GET /api/studytask/course/{planCourseId}
**Description:** Get all tasks for a specific course in study plan
**Access:** User (owner only)

#### GET /api/studytask/upcoming/{days}
**Description:** Get upcoming tasks (default 7 days)
**Access:** User

#### GET /api/studytask/overdue
**Description:** Get overdue tasks
**Access:** User

#### GET /api/studytask/status/{status}
**Description:** Get tasks by status (Pending, InProgress, Completed)
**Access:** User

#### POST /api/studytask
**Description:** Create new task
**Access:** User (owner only)

**Request Body:**
```json
{
  "planCourseId": 1,
  "taskName": "New Task",
  "description": "Task description",
  "scheduledDate": "2024-01-15",
  "dueDate": "2024-01-20",
  "priority": "High",
  "courseTopicId": 1
}
```

#### PUT /api/studytask/{id}
**Description:** Update task
**Access:** User (owner only)

**Request Body:**
```json
{
  "id": 1,
  "planCourseId": 1,
  "taskName": "Updated Task Name",
  "description": "Updated description",
  "scheduledDate": "2024-01-15",
  "dueDate": "2024-01-20",
  "priority": "Medium",
  "status": "InProgress",
  "courseTopicId": 1
}
```

#### DELETE /api/studytask/{id}
**Description:** Delete task
**Access:** User (owner only)

### 📊 Study Logs

#### GET /api/studylog/task/{taskId}
**Description:** Get study logs for a task
**Access:** User (owner only)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "taskId": 1,
      "startTime": "2024-01-15T09:00:00",
      "endTime": "2024-01-15T11:00:00",
      "durationMinutes": 120,
      "notes": "Focused on understanding variables"
    }
  ]
}
```

#### POST /api/studylog
**Description:** Create study log
**Access:** User (owner only)

**Request Body:**
```json
{
  "taskId": 1,
  "startTime": "2024-01-15T09:00:00",
  "endTime": "2024-01-15T11:00:00",
  "notes": "Studied variables and data types"
}
```

#### PUT /api/studylog/{id}
**Description:** Update study log
**Access:** User (owner only)

#### DELETE /api/studylog/{id}
**Description:** Delete study log
**Access:** User (owner only)

### 📎 Task Resources

#### GET /api/taskresource/task/{taskId}
**Description:** Get resources for a task
**Access:** User (owner only)

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "taskId": 1,
      "resourceName": "Lecture Notes",
      "resourceType": "Document",
      "resourceUrl": "https://example.com/notes.pdf",
      "description": "Important lecture notes"
    }
  ]
}
```

#### POST /api/taskresource
**Description:** Add resource to task
**Access:** User (owner only)

**Request Body:**
```json
{
  "taskId": 1,
  "resourceName": "Video Tutorial",
  "resourceType": "Video",
  "resourceUrl": "https://youtube.com/watch?v=example",
  "description": "Helpful video tutorial"
}
```

#### PUT /api/taskresource/{id}
**Description:** Update task resource
**Access:** User (owner only)

#### DELETE /api/taskresource/{id}
**Description:** Delete task resource
**Access:** User (owner only)

### 📚 Courses & Topics

#### GET /api/course
**Description:** Get all available courses
**Access:** User

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "courseCode": "CS101",
      "courseName": "Introduction to Computer Science",
      "credits": 3,
      "description": "Basic computer science concepts",
      "topics": [...]
    }
  ]
}
```

#### GET /api/course/{id}
**Description:** Get course by ID
**Access:** User

#### GET /api/coursetopic/course/{courseId}
**Description:** Get topics for a course
**Access:** User

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "courseId": 1,
      "topicName": "Variables and Data Types",
      "description": "Understanding variables and different data types",
      "orderIndex": 1
    }
  ]
}
```

---

## 👨‍💼 Admin Endpoints

### 👥 User Management

#### GET /api/adminuser
**Description:** Get all users with study plan statistics
**Access:** Admin

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "userId": "user-guid",
      "email": "user@example.com",
      "fullName": "John Doe",
      "role": "User",
      "totalPlans": 3,
      "activePlans": 2,
      "completedPlans": 1,
      "pendingPlans": 0,
      "studyPlans": [
        {
          "id": 1,
          "planName": "Spring 2024",
          "startDate": "2024-01-15",
          "endDate": "2024-05-15",
          "completed": false,
          "courseCount": 5
        }
      ]
    }
  ]
}
```

#### GET /api/adminuser/{userId}
**Description:** Get specific user with study plan details
**Access:** Admin

#### PUT /api/adminuser/{userId}/role
**Description:** Update user role
**Access:** Admin

**Request Body:**
```json
{
  "role": "Admin"
}
```

### 📊 Study Plan Management

#### GET /api/studyplan/admin-summary
**Description:** Get admin summary of all study plans
**Access:** Admin

**Response:**
```json
{
  "success": true,
  "data": {
    "totalUsers": 50,
    "totalPlans": 120,
    "activePlans": 85,
    "completedPlans": 25,
    "pendingPlans": 10,
    "averagePlansPerUser": 2.4,
    "completionRate": 20.8
  }
}
```

#### GET /api/studyplan
**Description:** Get all study plans (admin view)
**Access:** Admin

#### GET /api/studyplan/with-courses
**Description:** Get all study plans with course details
**Access:** Admin

### 📚 Course Management

#### POST /api/course
**Description:** Create new course
**Access:** Admin

**Request Body:**
```json
{
  "courseCode": "CS102",
  "courseName": "Data Structures",
  "credits": 4,
  "description": "Advanced data structures and algorithms"
}
```

#### PUT /api/course/{id}
**Description:** Update course
**Access:** Admin

#### DELETE /api/course/{id}
**Description:** Delete course
**Access:** Admin

### 📖 Topic Management

#### POST /api/coursetopic
**Description:** Create new course topic
**Access:** Admin

**Request Body:**
```json
{
  "courseId": 1,
  "topicName": "Arrays and Lists",
  "description": "Understanding arrays and linked lists",
  "orderIndex": 2
}
```

#### PUT /api/coursetopic/{id}
**Description:** Update course topic
**Access:** Admin

#### DELETE /api/coursetopic/{id}
**Description:** Delete course topic
**Access:** Admin

---

## 📋 Status Codes

- **200 OK** - Request successful
- **201 Created** - Resource created successfully
- **400 Bad Request** - Invalid request data
- **401 Unauthorized** - Authentication required
- **403 Forbidden** - Insufficient permissions
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server error

## 🔒 Error Response Format

```json
{
  "success": false,
  "message": "Error description",
  "errors": [
    {
      "field": "email",
      "message": "Email is required"
    }
  ]
}
```

## 📝 Notes

1. **Date Format:** All dates are in ISO 8601 format (YYYY-MM-DD)
2. **DateTime Format:** All date times are in ISO 8601 format (YYYY-MM-DDTHH:mm:ss)
3. **Pagination:** Some endpoints support pagination with `page` and `pageSize` query parameters
4. **Filtering:** Many endpoints support filtering by various criteria
5. **Sorting:** Results are typically sorted by date or relevance
6. **User Ownership:** Users can only access their own data unless they are admins
7. **Admin Access:** Admin endpoints require the "Admin" role in the JWT token

## 🚀 Getting Started

1. **Register/Login:** Use `/api/auth/register` or `/api/auth/login` to get a JWT token
2. **Set Authorization Header:** Include the token in all subsequent requests
3. **Create Study Plan:** Use `/api/studyplan` to create your first study plan
4. **Add Courses:** Use `/api/studyplancourse` to add courses to your plan
5. **Create Tasks:** Use `/api/studytask` to create study tasks
6. **Track Progress:** Use `/api/studylog` to log your study sessions 