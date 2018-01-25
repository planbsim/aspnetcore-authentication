using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecurityIdentity.Models;
using SecurityIdentity.Data;
using Microsoft.EntityFrameworkCore;

namespace SecurityIdentity.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext dbContext;

        public TodoItemService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync()
        {
            return await dbContext.Items
                .Where((item) => item.IsDone == false)
                .ToArrayAsync();
        }

        public async Task<bool> AddItemAsync(NewTodoItem newItem, ApplicationUser user)
        {
            var entity = dbContext.Items.Add(
                new TodoItem {
                    Id = Guid.NewGuid(),
                    Title = newItem.Title,
                    OwnerId = user.Id,
                    IsDone = false,
                    DueAt = DateTimeOffset.Now.AddDays(3)
                });

            bool added = await dbContext.SaveChangesAsync() == 1;

            return added;
        }

        public async Task<bool> MarkDoneAsync(Guid id, ApplicationUser user)
        {
            var singleItem = await dbContext.Items
                .Where(item => item.Id == id && item.OwnerId == user.Id)
                .SingleOrDefaultAsync();

            if (singleItem == null) return false;

            singleItem.IsDone = true;

            var saveResult = await dbContext.SaveChangesAsync();
            return saveResult == 1; // One entity should have been updated
        }

        public async Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync(ApplicationUser user)
        {
            return await dbContext.Items
                .Where((item) => item.IsDone == false && item.OwnerId == user.Id)
                .ToArrayAsync();
        }
    }
}
