using AutoMapper;
using Domain.ViewModel;
using Domain.Commands;
using Domain.DataModels;

namespace WebApi.AutoMapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskCommand, Task>();
            CreateMap<UpdateTaskCommand, Task>();
            CreateMap<Task, TaskViewModel>();
        }
    }
}
