using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static todoapp.TodoTask;

namespace todoapp
{
    public class TodoList
    {
        public string ListName { get; set; }
        public List<TodoTask> Tasks { get; set; }
        public bool IsFinished => Tasks.All(t => t.TaskStatus == TodoTask.TaskStates.Finished);

        // for creating a new todo list
        public TodoList(string listName)
        {
            ListName = listName;
            Tasks = new List<TodoTask>();
        }
        // for serialisation
        public TodoList()
        { }
        public bool ContainsTask(string taskName)
        {
            return Tasks.Any(task => task.TaskName == taskName);
        }
        public void AddTask(string taskName, DateTime dueDate)
        {
            dueDate = FormatDateTime.Format(dueDate);
            if(dueDate < FormatDateTime.Format(DateTime.Now)) 
            {
                MessageBox.Show("due date is before current date");
                return;
            }
            if(ContainsTask(taskName))
            {
                MessageBox.Show("Task already exists");
                return;
            }
            Tasks.Add(new TodoTask(taskName, dueDate));
        }
        public void RemoveTask(string taskName) 
        {
            if(!ContainsTask(taskName))
            {
                MessageBox.Show($"{taskName} doesnt exist in tasks");
                return;
            }
            Tasks.Remove(Tasks
                .First(task => task.TaskName == taskName)
                );
        }

        // repeat code. Clean up later.
        // bruh this code isnt even used once not gonna clean this up
        public void SetTaskAsFinished(string taskName)
        {
            if(!ContainsTask(taskName))
            {
                Console.WriteLine($"{taskName} does not exist");
                return;
            }
            if(Tasks.Find(task => task.TaskName == taskName).TaskStatus == TaskStates.Finished)
            {
                Console.WriteLine("Task is already finished");
                return;
            }
            Tasks.Find(task => task.TaskName == taskName).TaskStatus = TaskStates.Finished;
        }
        public void SetTaskAsNotFinished(string taskName)
        {
            if (!ContainsTask(taskName))
            {
                Console.WriteLine($"{taskName} does not exist");
                return;
            }
            if (Tasks.Find(task => task.TaskName == taskName).TaskStatus == TaskStates.Unfinished)
            {
                Console.WriteLine("Task is already not finished");
                return;
            }
            Tasks.Find(task => task.TaskName == taskName).TaskStatus = TaskStates.Unfinished;
        }
    }
}
