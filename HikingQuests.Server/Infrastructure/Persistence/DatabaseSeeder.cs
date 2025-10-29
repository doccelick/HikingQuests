namespace HikingQuests.Server.Infrastructure.Persistence
{
    using HikingQuests.Server.Application.Dtos;
    using HikingQuests.Server.Application.Interfaces;
    using HikingQuests.Server.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseSeeder
    {
        private readonly IQuestManagementService _questManagementService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly QuestDbContext _context;

        public DatabaseSeeder(IQuestManagementService managementService, IUnitOfWork unitOfWork, QuestDbContext questDbContext)
        {
            _questManagementService = managementService;
            _unitOfWork = unitOfWork;
            _context = questDbContext;
        }

        public async Task SeedDataAsync()
        {
            var questsExist = await _context.QuestItems.AnyAsync();
            if (questsExist)
            {
                // Exit if data already exists
                return;
            }

            var quests = new List<AddQuestDto>
            {
                new AddQuestDto(){Title = "Walk 5 km", Description = "Walk 5 km or more on a forest path." },
                new AddQuestDto(){Title = "Catch a trout", Description = "Spend at least 1 night in the forest sleeping in a tent." },
                new AddQuestDto(){Title = "Build a campfire", Description = "Build a campfire using materials you find around the camp." },
                new AddQuestDto(){Title = "Find a porcini mushroom", Description = "Locate a porcini mushroom in the forest." },
                new AddQuestDto(){Title = "Sleep in a tent", Description = "Spend at least 1 night in the forest sleeping in a tent." }
            };

            foreach (var quest in quests)
            {
                await _questManagementService.AddQuestAsync(quest);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
