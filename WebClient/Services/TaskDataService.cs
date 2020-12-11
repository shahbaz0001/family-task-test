using Domain.Commands;
using Domain.Queries;
using Domain.ViewModel;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebClient.Abstractions;
using WebClient.Shared.Models;
using Core.Extensions.ModelConversion;

namespace WebClient.Services
{
    public class TaskDataService : ITaskDataService
    {
        private readonly HttpClient _httpClient;
        private IEnumerable<TaskViewModel> _tasks;
        public IEnumerable<TaskViewModel> Tasks => _tasks;
        public TaskViewModel SelectedTask { get; private set; }

        public event EventHandler TaskUpdated;
        public event EventHandler TaskCreated;
        public event EventHandler<string> UpdateTaskFailed;
        public event EventHandler<string> CreateTaskFailed;

        public TaskDataService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            _tasks = new List<TaskViewModel>();
            LoadTasks();
        }

        public async void LoadTasks()
        {
            _tasks = (await GetAllTasks()).Payload;
            TaskUpdated?.Invoke(this, null);
        }

        public void CallbackTaskList()
        {
            TaskUpdated?.Invoke(this, null);
        }

        private async Task<CreateTaskCommandResult> Create(CreateTaskCommand command)
        {
            return await _httpClient.PostJsonAsync<CreateTaskCommandResult>("tasks", command);
        }

        private async Task<GetAllTasksQueryResult> GetAllTasks()
        {
            return await _httpClient.GetJsonAsync<GetAllTasksQueryResult>("tasks");
        }

        private async Task<UpdateMemberCommandResult> Update(UpdateTaskCommand command)
        {
            return await _httpClient.PutJsonAsync<UpdateMemberCommandResult>($"tasks/{command.Id}", command);
        }

        public async Task UpdateTask(TaskViewModel taskViewModel)
        {
            var result = await Update(taskViewModel.ToUpdateTaskCommand());

            Console.WriteLine(JsonSerializer.Serialize(result));

            if (result != null)
            {
                var updatedList = (await GetAllTasks()).Payload;

                if (updatedList != null)
                {
                    _tasks = updatedList;
                    TaskUpdated?.Invoke(this, null);
                    return;
                }
                UpdateTaskFailed?.Invoke(this, "The save was successful, but we can no longer get an updated list of members from the server.");
            }

            UpdateTaskFailed?.Invoke(this, "Unable to save changes.");
        }

        public async Task CreateTask(TaskViewModel taskViewModel)
        {
            var result = await Create(taskViewModel.ToCreateTaskCommand());
            if (result != null)
            {
                var updatedList = (await GetAllTasks()).Payload;

                if (updatedList != null)
                {
                    _tasks = updatedList;
                     SelectNullTask();
                    TaskCreated?.Invoke(this, null);
                    return;
                }
                CreateTaskFailed?.Invoke(this, "The creation was successful, but we can no longer get an updated list of members from the server.");
            }

            CreateTaskFailed?.Invoke(this, "Unable to create record.");
        }

        public void SelectTask(Guid id)
        {
            if (_tasks.All(memberVm => memberVm.Id != id)) return;
            {
                SelectedTask = _tasks.SingleOrDefault(memberVm => memberVm.Id == id);
                TaskUpdated?.Invoke(this, null);
            }
        }

        public void SelectNullTask()
        {
            SelectedTask = null;
            TaskUpdated?.Invoke(this, null);
        }

        public void SetTask(TaskViewModel taskViewModel)
        {
            SelectedTask = taskViewModel;
        }
    }
}