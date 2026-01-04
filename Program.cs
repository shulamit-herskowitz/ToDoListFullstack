using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- Services Configuration ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("https://todo-list-app-hubt.onrender.com") //
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Database Configuration
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<ToDoDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
        ?? throw new InvalidOperationException("CONNECTION_STRING environment variable is not set.");
    
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 0)); //
    
    builder.Services.AddDbContext<ToDoDbContext>(options =>
        options.UseMySql(connectionString, serverVersion, options => 
            options.EnableRetryOnFailure()));
}

var app = builder.Build();

// --- Middleware Pipeline ---
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    c.RoutePrefix = string.Empty; 
});

app.UseCors("AllowAll");

// --- API Endpoints ---

app.MapGet("/items", async (ToDoDbContext db) =>
{
    return await db.Items.ToListAsync();
})
.WithName("GetAllItems")
.WithTags("Items");

app.MapGet("/items/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
})
.WithName("GetItemById")
.WithTags("Items");

app.MapPost("/items", async (Item item, ToDoDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
})
.WithName("CreateItem")
.WithTags("Items");

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
