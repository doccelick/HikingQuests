using HikingQuests.Server.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In-memory singleton
builder.Services.AddSingleton<IQuestLog>(sp =>
{
    var questLog = new QuestLog();

    questLog.AddQuest(new QuestItem("Walk 5 km", "Walk 5 km or more on a forest path."));
    questLog.AddQuest(new QuestItem("Catch a trout", "Catch a trout with a fishing rod."));
    questLog.AddQuest(new QuestItem("Sleep in a tent", "Spend at least 1 night in the forest sleeping in a tent."));

    return questLog;
});

var app = builder.Build();

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