using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CreateTaskCommandResult
    {
        public TaskViewModel Payload { get; set; }
    }
}
