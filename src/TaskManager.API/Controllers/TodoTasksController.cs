using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Business.Common;
using TaskManager.Business.DTOs.Tasks;
using TaskManager.Business.Interfaces;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoTasksController : ControllerBase
    {
        private readonly ITodoTaskService _todoTaskService;
        public TodoTasksController(ITodoTaskService todoTaskService)
        {
            _todoTaskService = todoTaskService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TodoTaskDto>>>> GetAllTasks()
        {
            var tasks = await _todoTaskService.GetAllTasksAsync();
            return Ok(new ApiResponse<IEnumerable<TodoTaskDto>>(tasks, "List of Tasks"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TodoTaskDto>>> GetTaskById(int id)
        {
            var tasks = await _todoTaskService.GetTaskByIdAsync(id);
            return Ok(new ApiResponse<TodoTaskDto>(tasks));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TodoTaskDto>>> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            var task = await _todoTaskService.CreateTaskAsync(createTaskDto);
            return Ok(new ApiResponse<TodoTaskDto>(task, "Task created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TodoTaskDto>>> UpdateTask(int id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            var task = await _todoTaskService.UpdateTaskAsync(id, updateTaskDto);
            return Ok(new ApiResponse<TodoTaskDto>(task, "Task updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteTask(int id)
        {
            await _todoTaskService.DeleteTaskAsync(id);
            return Ok(new ApiResponse<string>(null, "Task deleted successfully"));
        }
    }
}
