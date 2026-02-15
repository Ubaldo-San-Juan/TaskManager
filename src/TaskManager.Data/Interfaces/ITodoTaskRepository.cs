using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Interfaces
{
    public interface ITodoTaskRepository
    {
        Task<IEnumerable<TodoTask>> GetAllTasksAsync();
        Task<IEnumerable<TodoTask>> GetAllTasksByUserAsync(int userId);
        Task<TodoTask?> GetTaskByIdAsync(int idTask);
        Task CreateTaskAsync(TodoTask task);
        Task UpdateTaskAsync(TodoTask task);
        Task DeleteTaskAsync(TodoTask task);
    }
}
