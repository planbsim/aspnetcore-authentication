using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecurityIdentity.Services;
using SecurityIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace SecurityIdentity.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService todoItemService;
        private readonly UserManager<ApplicationUser> userManager;

        public TodoController(
            ITodoItemService todoItemService,
            UserManager<ApplicationUser> userManager)
        {
            this.todoItemService = todoItemService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            IEnumerable<TodoItem> todoItems = await todoItemService.GetIncompleteItemsAsync(currentUser);

            TodoViewModel viewModel = new TodoViewModel
            {
                Items = todoItems
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AddItem(NewTodoItem newItem)
        {
            ApplicationUser currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool added = await todoItemService.AddItemAsync(newItem, currentUser);

            if (!added)
            {
                return BadRequest(new { error = "Could not add item to the database" });
            }

            return Ok();
        }

        public async Task<IActionResult> MarkDone(Guid id)
        {
            ApplicationUser currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            if (id == Guid.Empty) return BadRequest();

            var successful = await todoItemService.MarkDoneAsync(id, currentUser);

            if (!successful) return BadRequest();

            return Ok();
        }
    }
}