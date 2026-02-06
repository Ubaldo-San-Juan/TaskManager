using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Context;
using TaskManager.Data.Entities;
using TaskManager.Data.Interfaces;

namespace TaskManager.Data.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly ApplicationDbContext _context;
        public TodoTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoTask>> GetAllTasksAsync()
        {
            var tasks = await _context.Tasks.Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            return tasks;
        }

        public async Task<IEnumerable<TodoTask>> GetAllTasksByUserAsync(int userId)
        {
            var tasks = await _context.Tasks.Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            return tasks;
        }

        public async Task<TodoTask?> GetTaskByIdAsync(int idTask)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == idTask && !t.IsDeleted);
            return task;
        }
        public async Task CreateTaskAsync(TodoTask task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public Task UpdateTaskAsync(TodoTask task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            _context.Tasks.Update(task);
            return _context.SaveChangesAsync();
        }
        public Task DeleteTaskAsync(TodoTask task)
        {
            task.IsDeleted = true;
            task.DeletedAt = DateTime.UtcNow;
            _context.Tasks.Update(task);
            return _context.SaveChangesAsync();
        }
    }
}
