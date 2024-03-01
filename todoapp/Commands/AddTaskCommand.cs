using BookInventoryAdminProgram.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using todoapp.ViewModel;

namespace todoapp.Commands
{
    public class AddTaskCommand : CommandBase
    {
        private MainWindowViewModel _mwvm;

        public AddTaskCommand(MainWindowViewModel mwvm)
        {
            _mwvm = mwvm;
        }

        public override void Execute(object parameter)
        {
            // grab all values
            string ListName = _mwvm.SelectedListName;
            string TaskName = _mwvm.FieldName;
            DateTime DueDate = _mwvm.DueDate;
            Action<string, string, DateTime, TodoTask.TaskStates> CreateNewTaskUIPanel = _mwvm.CreateNewTaskUIPanel;

            // find todolist from store
            TodoList todoList = TodoListStore.TodoLists.Find(list => list.ListName == ListName);
            if(todoList.ContainsTask(TaskName))
            {
                MessageBox.Show("Task already exists.\nPlease use a different task name");
                return;
            }
            // add task to list
            todoList.AddTask(TaskName, DueDate);
            // update todoList in vm and serialise the new data change
            _mwvm.TodoLists = TodoListStore.TodoLists;
            TodoListStore.SerialiseToDoLists();

            // add task to ui
            CreateNewTaskUIPanel.Invoke(ListName, TaskName, DueDate, TodoTask.TaskStates.Unfinished);

            // reset fields
            _mwvm.FieldName = "";
            _mwvm.DueDate = DateTime.Now;
        }
    }
}
