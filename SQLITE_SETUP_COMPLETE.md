# SQLite Migration Complete! ✅

## What Was Done

### 1. ✅ Installed SQLite Package
```powershell
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.0
```

### 2. ✅ Updated appsettings.json
Changed connection string from MySQL to SQLite:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=Todo.db"
}
```

### 3. ✅ Created Item Model
**File:** `Models/Item.cs`
- Id (int, Auto-increment Primary Key)
- Name (string, Required, Max 255 chars)
- IsComplete (bool)

### 4. ✅ Created ToDoDbContext
**File:** `Data/ToDoDbContext.cs`
- Inherits from DbContext
- DbSet<Item> Items
- Configured entity mappings and indexes
- Includes seed data (3 sample items)

### 5. ✅ Updated Program.cs
Added DbContext registration:
```csharp
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### 6. ✅ Created & Applied Migration
```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Result:** `Todo.db` SQLite database file created successfully!

---

## Database Schema Created

**Table:** Items
- Id (INTEGER, PRIMARY KEY, AUTOINCREMENT)
- Name (TEXT, NOT NULL)
- IsComplete (INTEGER, NOT NULL)
- Index on Name
- Index on IsComplete

**Sample Data Inserted:**
1. Learn ASP.NET Core (incomplete)
2. Build TodoAPI (incomplete)
3. Deploy to production (incomplete)

---

## Next Steps - Add API Endpoints

Now you can add endpoints to interact with your data. Here's an example:

```csharp
// In Program.cs, before app.Run();

// GET: Get all items
app.MapGet("/api/items", async (ToDoDbContext db) =>
{
    return await db.Items.ToListAsync();
})
.WithName("GetAllItems");

// GET: Get item by ID
app.MapGet("/api/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
})
.WithName("GetItemById");

// POST: Create new item
app.MapPost("/api/items", async (Item item, ToDoDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/api/items/{item.Id}", item);
})
.WithName("CreateItem");

// PUT: Update item
app.MapPut("/api/items/{id}", async (int id, Item inputItem, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();
    
    item.Name = inputItem.Name;
    item.IsComplete = inputItem.IsComplete;
    await db.SaveChangesAsync();
    
    return Results.NoContent();
})
.WithName("UpdateItem");

// DELETE: Delete item
app.MapDelete("/api/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();
    
    db.Items.Remove(item);
    await db.SaveChangesAsync();
    
    return Results.NoContent();
})
.WithName("DeleteItem");
```

---

## Run Your API

```powershell
dotnet run
```

Then test with:
- Browser: `https://localhost:xxxx/api/items`
- Or use Postman/Thunder Client to test all endpoints

---

## Useful Commands

**View migrations:**
```powershell
dotnet ef migrations list
```

**Remove last migration (if needed):**
```powershell
dotnet ef migrations remove
```

**Update database after model changes:**
```powershell
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

**Drop database and recreate:**
```powershell
dotnet ef database drop
dotnet ef database update
```
