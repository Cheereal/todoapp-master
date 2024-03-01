using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todoapp
{
    public class TodoTask
    {
        public enum TaskStates
        {
            Unfinished,
            Finished,
            Failed
        }
        public string TaskName { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStates TaskStatus { get; set; }

        public TodoTask(string taskName, DateTime dueDate)
        {
            TaskName = taskName;
            DueDate = dueDate;
            TaskStatus = TaskStates.Unfinished;
        }
        public bool IsTaskPastDueDate()
        {
            if(DueDate < FormatDateTime.Format(DateTime.Now)) 
            {
                return true;
            }
            return false;
        }
    }
}
