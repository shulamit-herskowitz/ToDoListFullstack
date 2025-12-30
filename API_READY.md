# 🎉 TodoAPI is Live!

## ✅ API is Running at: http://localhost:5011

---

## 📊 Swagger UI (Test Your API Visually)

**Open in your browser:**
```
http://localhost:5011/swagger
```

Swagger provides an interactive UI where you can:
- View all available endpoints
- Test each endpoint directly
- See request/response examples
- View data models

---

## 🔌 API Endpoints

### Base URL: `http://localhost:5011`

| Method | Endpoint | Description | Request Body |
|--------|----------|-------------|--------------|
| **GET** | `/items` | Get all todo items | - |
| **GET** | `/items/{id}` | Get a specific item by ID | - |
| **POST** | `/items` | Create a new item | `{ "name": "string", "isComplete": false }` |
| **PUT** | `/items/{id}` | Update an existing item | `{ "name": "string", "isComplete": true }` |
| **DELETE** | `/items/{id}` | Delete an item | - |

---

## 🧪 Test Endpoints (Using PowerShell)

### 1. Get All Items
```powershell
Invoke-RestMethod -Uri "http://localhost:5011/items" -Method GET | ConvertTo-Json
```

### 2. Get Item by ID
```powershell
Invoke-RestMethod -Uri "http://localhost:5011/items/1" -Method GET | ConvertTo-Json
```

### 3. Create a New Item
```powershell
$body = @{
    name = "Learn Minimal APIs"
    isComplete = $false
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5011/items" -Method POST -Body $body -ContentType "application/json" | ConvertTo-Json
```

### 4. Update an Item (Mark as Complete)
```powershell
$body = @{
    name = "Learn ASP.NET Core"
    isComplete = $true
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5011/items/1" -Method PUT -Body $body -ContentType "application/json" | ConvertTo-Json
```

### 5. Delete an Item
```powershell
Invoke-RestMethod -Uri "http://localhost:5011/items/1" -Method DELETE
```

---

## 🌐 CORS Configuration

✅ **CORS is enabled** with the following policy:
- **AllowAnyOrigin**: Any website can call your API
- **AllowAnyMethod**: GET, POST, PUT, DELETE, etc.
- **AllowAnyHeader**: All request headers are allowed

This means your **React app** (or any frontend) can connect to this API without CORS issues!

---

## 📦 Sample Data

Your database already has 3 items:
1. Learn ASP.NET Core (Not Complete)
2. Build TodoAPI (Not Complete)
3. Deploy to production (Not Complete)

---

## 🚀 For Your React App

Use these endpoints in your React app:

```javascript
// Base URL
const API_URL = 'http://localhost:5011';

// Get all items
const getItems = async () => {
  const response = await fetch(`${API_URL}/items`);
  return await response.json();
};

// Create item
const createItem = async (name) => {
  const response = await fetch(`${API_URL}/items`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name, isComplete: false })
  });
  return await response.json();
};

// Update item
const updateItem = async (id, name, isComplete) => {
  const response = await fetch(`${API_URL}/items/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name, isComplete })
  });
  return await response.json();
};

// Delete item
const deleteItem = async (id) => {
  await fetch(`${API_URL}/items/${id}`, {
    method: 'DELETE'
  });
};
```

---

## 🛠️ What Was Implemented

### 1. ✅ CORS Policy
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

### 2. ✅ Swagger/OpenAPI
```csharp
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In middleware:
app.UseSwagger();
app.UseSwaggerUI();
```

### 3. ✅ RESTful CRUD Endpoints
- **GET /items** - Fetch all items
- **GET /items/{id}** - Fetch single item
- **POST /items** - Create new item
- **PUT /items/{id}** - Update item
- **DELETE /items/{id}** - Delete item

All endpoints use your `ToDoDbContext` and are properly tagged for Swagger.

---

## 📝 Files Updated

- ✅ [Program.cs](Program.cs) - Added CORS, Swagger, and all API endpoints
- ✅ Installed `Swashbuckle.AspNetCore` package

---

## 🎯 Next Steps

1. **Open Swagger UI**: http://localhost:5011/swagger
2. **Test each endpoint** using the interactive Swagger interface
3. **Connect your React app** using the JavaScript code above
4. **Build your frontend** to consume these APIs

---

## 💡 Tips

- The API automatically returns JSON responses
- All endpoints include proper HTTP status codes (200, 201, 204, 404)
- Swagger documentation is auto-generated from your code
- Database file `Todo.db` stores all data persistently

---

## 🛑 To Stop the Server

Press `Ctrl+C` in the terminal where the app is running.
