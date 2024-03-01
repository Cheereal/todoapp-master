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
    public class AddListCommand : CommandBase
    {
        private MainWindowViewModel _mwvm;

        public AddListCommand(MainWindowViewModel mwvm)
        {
            _mwvm = mwvm;
        }
        public override void Execute(object parameter)
        {
            // grab all values
            string ListName = _mwvm.FieldName;
            Action<string> CreateNewListUIPanel = _mwvm.CreateNewListUIPanel;
            // check if list already exists
            if(TodoListStore.TodoLists.Any(list => list.ListName == ListName)) 
            {
                MessageBox.Show("List already exists.\nPlease choose a different name for the to-do list.");
                return;
            }

            // find todolist from store
            TodoListStore.TodoLists.Add(new TodoList(ListName));
            _mwvm.TodoLists = TodoListStore.TodoLists;
            TodoListStore.SerialiseToDoLists();

            // add task to ui
            CreateNewListUIPanel.Invoke(ListName);

            // reset fields
            _mwvm.FieldName = "";
        }
    }
}
