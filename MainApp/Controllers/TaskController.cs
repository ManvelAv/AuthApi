using MainApp.Models;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using MainApp.AuthService;

namespace MainApp.Controllers
{
   
    public class TaskController : Controller
    {

        private readonly TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        public IActionResult Index()
        { return View(); }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var token = HttpContext.Request.Cookies["AuthToken"];
            var tasks = await _taskService.GetTasksAsync(token);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask( TaskItem taskItem)
        {
            var token = HttpContext.Request.Cookies["AuthToken"];
            var createdTask = await _taskService.CreateTaskAsync(taskItem, token);
            if (createdTask != null)
            {
                return CreatedAtAction(nameof(GetTasks), new { id = createdTask.Id }, createdTask);
            }

            return BadRequest("Failed to create task.");
        }

        // Метод для обновления задачи
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem taskItem)
        {
            var token = HttpContext.Request.Cookies["AuthToken"];
            var updatedTask = await _taskService.UpdateTaskAsync(id, taskItem, token);
            if (updatedTask != null)
            {
                return NoContent();
            }

            return BadRequest($"Failed to update task with id {id}");
        }

        // Метод для удаления задачи
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var token = HttpContext.Request.Cookies["AuthToken"];
            var result = await _taskService.DeleteTaskAsync(id, token);
            if (result)
            {
                return NoContent();
            }

            return BadRequest($"Failed to delete task with id {id}");
        }

    }
}