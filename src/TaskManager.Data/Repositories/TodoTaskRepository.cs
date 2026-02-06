using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Entities;
using TaskManager.Data.Interfaces;

namespace TaskManager.Data.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        public Task CreateTaskAsync(TodoTask task)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTaskAsync(TodoTask task)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TodoTask>> GetAllTasksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TodoTask>> GetAllTasksByUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<TodoTask?> GetTaskByIdAsync(int idTask)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTaskAsync(TodoTask task)
        {
            throw new NotImplementedException();
        }
    }
}
