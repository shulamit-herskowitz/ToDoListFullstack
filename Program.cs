using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- 1. הוספת שירותים למכולה (Container) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. הגדרת CORS - פותר את השגיאה שראינו בדפדפן ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // אישור גישה ספציפי לכתובת של ה-React שלך ב-Render
        policy.WithOrigins("https://todo-list-app-hubt.onrender.com") 
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// --- 3. הגדרת מסד הנתונים ---
if (builder.Environment.IsDevelopment())
{
    // סביבת פיתוח: שימוש ב-SQLite
    builder.Services.AddDbContext<ToDoDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    // סביבת ייצור (Production): שימוש ב-MySQL
    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
        ?? throw new InvalidOperationException("CONNECTION_STRING environment variable is not set.");
    
    // הגדרת גרסת השרת ידנית ל-8.0.0 (הגרסה של CleverCloud) כדי למנוע שגיאה 500
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 0)); 
    
    builder.Services.AddDbContext<ToDoDbContext>(options =>
        options.UseMySql(connectionString, serverVersion, options => 
            options.EnableRetryOnFailure())); // הוספת מנגנון ניסיון חוזר במקרה של ניתוק
}

var app = builder.Build();

// --- 4. הגדרת Swagger - פתוח גם ב-Production לצרכי ניפוי שגיאות ---
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    c.RoutePrefix = string.Empty; // ה-Swagger ייפתח ישירות בכתובת השרת
});

// --- 5. הפעלת CORS ---
app.UseCors("AllowAll");

// ============================================
// API ENDPOINTS - ניהול המשימות
// ============================================

// שליפת כל המשימות
app.MapGet("/items", async (ToDoDbContext db) =>
{
    return await db.Items.ToListAsync();
})
.WithName("GetAllItems")
.WithTags("Items");

// שליפת משימה לפי מזהה (ID)
app.MapGet("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
})
.WithName("GetItemById")
.WithTags("Items");

// יצירת משימה חדשה
app.MapPost("/items", async (Item item, ToDoDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
})
.WithName("CreateItem")
.WithTags("Items");

// עדכון משימה קיימת
app.MapPut("/items/{id}", async (int id, Item inputItem, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    item.Name = inputItem.Name;
    item.IsComplete = inputItem.IsComplete;
    await db.SaveChangesAsync();

    return Results.Ok(item);
})
.WithName("UpdateItem")
.WithTags("Items");

// מחיקת משימה
app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteItem")
.WithTags("Items");

app.Run();
