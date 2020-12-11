using Blazorise;
using Domain.ViewModel;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using WebClient.Abstractions;
using WebClient.Components;

namespace WebClient.Pages
{
    public class TasksBase : ComponentBase
    {
        protected List<TaskViewModel> tasks = new List<TaskViewModel>();
        protected TaskViewModel createTaskViewModel;
        public TaskListerComponent taskListerComponent;

        protected bool isLoaded;
        protected bool showLister;
        protected Validations validations;

        [Inject]
        public IMemberDataService MemberDataService { get; set; }
        [Inject]
        public ITaskDataService TaskDataService { get; set; }

        protected override void OnInitialized()
        {

            TaskDataService.TaskCreated += MemberDataService_MembersChanged;

            createTaskViewModel = new TaskViewModel()
            {
                Id = Guid.NewGuid(),
                IsComplete = false,
                Subject = "",
            };

            showLister = true;
            isLoaded = true;

            UpdateTasks();

            base.OnInitialized();
        }
                
        private void MemberDataService_MembersChanged(object sender, EventArgs e)
        {
            createTaskViewModel = TaskDataService.SelectedTask ?? new TaskViewModel();
            StateHasChanged();
        }

        protected void UpdateTasks()
        {
            var result = TaskDataService.Tasks;

            if (result.Any())
            {
                tasks = result.ToList();
            }
        }

        protected void OnAddItem()
        {
            if (validations.ValidateAll())
            {
                validations.ClearAll();

                if (MemberDataService.SelectedMember != null && MemberDataService.SelectedMember.Id != Guid.Empty)
                {
                    createTaskViewModel.AssignedToId = MemberDataService.SelectedMember.Id;
                }
                TaskDataService.CreateTask(createTaskViewModel);
            }
        }

    }
}
