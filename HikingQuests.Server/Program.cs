using HikingQuests.Server.Application;
using HikingQuests.Server.Domain;
using HikingQuests.Server.Domain.Entities;
using HikingQuests.Server.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

var questLogInstance = new QuestLog();
//TODO: remove when database seeding is implemented.
questLogInstance.AddQuest(new QuestItem("Walk 5 km", "Walk 5 km or more on a forest path."));
questLogInstance.AddQuest(new QuestItem("Catch a trout", "Catch a trout with a fishing rod."));
questLogInstance.AddQuest(new QuestItem("Sleep in a tent", "Spend at least 1 night in the forest sleeping in a tent."));
questLogInstance.AddQuest(new QuestItem("Build a campfire", "Build a campfire using materials you find around the camp."));
questLogInstance.AddQuest(new QuestItem("Find a porcini mushroom", "Locate a porcini mushroom in the forest."));

builder.Services.AddSingleton<IQuestManagementService>(questLogInstance);
builder.Services.AddSingleton<IQuestQueryService>(questLogInstance);
builder.Services.AddSingleton<IQuestWorkflowService>(questLogInstance);

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