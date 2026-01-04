using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy for React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add DbContext with SQLite for Development and MySQL for Production
if (builder.Environment.IsDevelopment())
{
    // Development: Use SQLite
    builder.Services.AddDbContext<ToDoDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    // Production: Use MySQL from environment variable
    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
        ?? throw new InvalidOperationException("CONNECTION_STRING environment variable is not set.");
    
    builder.Services.AddDbContext<ToDoDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}

var app = builder.Build();

// --- עדכון כאן: אפשור Swagger גם ב-Production כדי שנוכל לדבג ב-Render ---
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    c.RoutePrefix = string.Empty; // זה יגרום ל-Swagger להיפתח ישר כשנכנסים לכתובת האתר
});

// Enable CORS
app.UseCors("AllowAll");

// ============================================
// API ENDPOINTS
// ============================================

// GET: /items - Fetch all items
app.MapGet("/items", async (ToDoDbContext db) =>
{
    return await db.Items.ToListAsync();
})
.WithName("GetAllItems")
.WithTags("Items")
.Produces<List<Item>>(StatusCodes.Status200OK);

// GET: /items/{id} - Fetch item by ID
app.MapGet("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
})
.WithName("GetItemById")
.WithTags("Items")
.Produces<Item>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// POST: /items - Create a new item
app.MapPost("/items", async (Item item, ToDoDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
})
.WithName("CreateItem")
.WithTags("Items")
.Produces<Item>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

// PUT: /items/{id} - Update an item's status
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
.WithTags("Items")
.Produces<Item>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// DELETE: /items/{id} - Remove an item
app.MapDelete("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();

    db.Items.Remove(item);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithName("DeleteItem")
.WithTags("Items")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.Run();
