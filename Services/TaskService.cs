using AutoMapper;
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.DataModels;
using Domain.Queries;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Services
{
    using Domain.DataModels;
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(IMapper mapper, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }
        public async Task<CreateTaskCommandResult> CreateTaskCommandHandler(CreateTaskCommand command)
        {
            var task = _mapper.Map<Domain.DataModels.Task>(command);
            var persistedTask = await _taskRepository.CreateRecordAsync(task);

            TaskViewModel taskViewModel = _mapper.Map<TaskViewModel>(persistedTask);

            return new CreateTaskCommandResult()
            {
                Payload = taskViewModel
            };
        }

        public async Task<GetAllTasksQueryResult> GetAllTasksQueryHandler()
        {
            IEnumerable<TaskViewModel> lstTaskViewModel = new List<TaskViewModel>();

            var tasks = await _taskRepository.Reset().ToListAsync();

            if (tasks != null && tasks.Any())
                lstTaskViewModel = _mapper.Map<IEnumerable<TaskViewModel>>(tasks);

            return new GetAllTasksQueryResult()
            {
                Payload = lstTaskViewModel
            };
        }

        public async Task<UpdateTaskCommandResult> UpdateTaskCommandHandler(UpdateTaskCommand command)
        {
            var isSucceed = true;
            Task member = await _taskRepository.ByIdAsync(command.Id);

            _mapper.Map<UpdateTaskCommand, Task>(command, member);

            var affectedRecordsCount = await _taskRepository.UpdateRecordAsync(member);

            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new UpdateTaskCommandResult()
            {
                Succeed = isSucceed
            };
        }
    }
}
