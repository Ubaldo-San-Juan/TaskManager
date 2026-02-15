using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.DTOs.Tasks;
using TaskManager.Data.Entities;

namespace TaskManager.Business.Interfaces
{
    public interface ITodoTaskService
    {
        Task<IEnumerable<TodoTaskDto>> GetAllTasksAsync();
        Task<TodoTaskDto> GetTaskByIdAsync(int id);
        Task<TodoTaskDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<TodoTaskDto> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
        Task DeleteTaskAsync(int id);
    }
}
