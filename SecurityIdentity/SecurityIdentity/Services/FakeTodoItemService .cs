﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecurityIdentity.Models;

namespace SecurityIdentity.Services
{
    public class FakeTodoItemService : ITodoItemService
    {
        public Task<bool> AddItemAsync(NewTodoItem newItem)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddItemAsync(NewTodoItem newItem, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync()
        {
            // Return an array of TodoItems
            IEnumerable<TodoItem> items = new[]
            {
                new TodoItem
                {
                    Title = "Learn ASP.NET Core",
                    DueAt = DateTimeOffset.Now.AddDays(1)
                },
                new TodoItem
                {
                    Title = "Build awesome apps",
                    DueAt = DateTimeOffset.Now.AddDays(2)
                }
            };

            return Task.FromResult(items);
        }

        public Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MarkDoneAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> MarkDoneAsync(Guid id, ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
