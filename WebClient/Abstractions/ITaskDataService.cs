using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Shared.Models;

namespace WebClient.Abstractions
{
    /// <summary>
    /// This Service is currently using the TaskModel Class, and will need to use a shared view
    /// model after the model has been created.  For the moment, this pattern facilitates a client
    /// side storage mechanism to view functionality.  See work completed for the MemberDataService
    /// for an example of expectations.
    /// </summary>
    public interface ITaskDataService
    {
        IEnumerable<TaskViewModel> Tasks { get; }
        TaskViewModel SelectedTask { get; }

        event EventHandler TaskUpdated;
        event EventHandler TaskCreated;

        event EventHandler<string> UpdateTaskFailed;
        event EventHandler<string> CreateTaskFailed;

        Task UpdateTask(TaskViewModel taskViewModel);
        Task CreateTask(TaskViewModel taskViewModel);
        void SelectTask(Guid id);
        void CallbackTaskList();
        void SetTask(TaskViewModel taskViewModel);
    }
}