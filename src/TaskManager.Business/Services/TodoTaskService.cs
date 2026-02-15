using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Business.DTOs.Tasks;
using TaskManager.Business.Interfaces;
using TaskManager.Data.Entities;
using TaskManager.Data.Interfaces;

namespace TaskManager.Business.Services
{
    public class TodoTaskService : ITodoTaskService
    {
        private readonly ITodoTaskRepository _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTaskDto> _createValidator;
        private readonly IValidator<UpdateTaskDto> _updateValidator;
        private readonly ICurrentUserService _currentUser;

        public TodoTaskService(ITodoTaskRepository repository, IMapper mapper, IValidator<CreateTaskDto> createValidator, IValidator<UpdateTaskDto> updateValidator, ICurrentUserService currentUser)
        {
            _repository = repository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<TodoTaskDto>> GetAllTasksAsync()
        {
            IEnumerable<TodoTask> tasks;
            if (_currentUser.IsAdmin())
            {
                tasks = await _repository.GetAllTasksAsync();
            }
            else
            {
                var userId = _currentUser.GetUserId();
                tasks = await _repository.GetAllTasksByUserAsync(userId);
            }
            return _mapper.Map<IEnumerable<TodoTaskDto>>(tasks);
        }

        public async Task<TodoTaskDto> GetTaskByIdAsync(int id)
        {
            var tasks = await _repository.GetTaskByIdAsync(id);
            ValidateTask(tasks);
            return _mapper.Map<TodoTaskDto>(tasks);
        }
        public async Task<TodoTaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            var validationResult = _createValidator.Validate(createTaskDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var taskEntity = _mapper.Map<TodoTask>(createTaskDto);

            // Asignar el UserId de la tarea al ID del usuario actual
            taskEntity.UserId = _currentUser.GetUserId();
            taskEntity.CreatedAt = DateTime.UtcNow;

            await _repository.CreateTaskAsync(taskEntity);
            return _mapper.Map<TodoTaskDto>(taskEntity);
        }

        public async Task<TodoTaskDto> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
        {
            var validationResult = _updateValidator.Validate(updateTaskDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingTask = await _repository.GetTaskByIdAsync(id);
            ValidateTask(existingTask);

            _mapper.Map(updateTaskDto, existingTask);
            
            await _repository.UpdateTaskAsync(existingTask!);
            return _mapper.Map<TodoTaskDto>(existingTask);
        }

        public async Task DeleteTaskAsync(int id)
        {
            var TaskToDelete = await _repository.GetTaskByIdAsync(id);
            ValidateTask(TaskToDelete);
            await _repository.DeleteTaskAsync(TaskToDelete!);
        }

        private void ValidateTask(TodoTask? task)
        {
            // Si no existe, error 404
            if (task == null)
            {
                throw new KeyNotFoundException("La tarea no existe.");
            }

            // Si NO soy Admin Y la tarea NO es mía.
            // Simulamos que no existe (404) por seguridad/privacidad
            if (!_currentUser.IsAdmin() && task.UserId != _currentUser.GetUserId())
            {
                throw new KeyNotFoundException("La tarea no existe.");
            }
        }
    }
}
