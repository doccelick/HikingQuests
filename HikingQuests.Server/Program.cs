using HikingQuests.Server.Application;
using HikingQuests.Server.Application.Interfaces;
using HikingQuests.Server.Infrastructure;
using HikingQuests.Server.Infrastructure.Persistence;
using HikingQuests.Server.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -- SERVICE REGISTRATION --
// 1. API Exception filter
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

// 2. Database Context
builder.Services.AddDbContext<QuestDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Infrastructure
builder.Services.AddScoped<IQuestRepository, QuestRepository>();

// 4. Application Services and Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IQuestManagementService, QuestManagementService>();
builder.Services.AddScoped<IQuestQueryService, QuestQueryService>();
builder.Services.AddScoped<IQuestWorkflowService, QuestWorkflowService>();
builder.Services.AddScoped<DatabaseSeeder>();

var app = builder.Build();

// -- DATABASE INITIALIZATION --

// Ensure the database exists and migrations are applied on startup
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var context = serviceProvider.GetRequiredService<QuestDbContext>();

    // 1. This command will use the connection string from appsettings.json 
    // to create the 'hikingquests.db' file and run the migrations.
    context.Database.Migrate();

    // 2. The Seeder adds data to the database, if no data exists.
    var seeder = serviceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedDataAsync();

}


// -- HTTP PIPELINE --

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();