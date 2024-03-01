using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using todoapp.Commands;

namespace todoapp.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        // smth
        public List<TodoList> TodoLists { get; set; }
        public DateTime MinimumDate { get; set; }
        public Action<string> CreateNewListUIPanel;
        public Action<string, string, DateTime, TodoTask.TaskStates> CreateNewTaskUIPanel;
        public string SelectedListName;
        private string _fieldName;
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                _fieldName = value;
                OnPropertyChanged(nameof(FieldName));
            }
        }
        private DateTime _dueDate;
        public DateTime DueDate
        {
            get
            {
                return _dueDate;
            }
            set
            {
                if(value is DateTime)
                {
                    _dueDate = value;
                    OnPropertyChanged(nameof(DueDate));
                }
            }
        }




        public bool IsCreateListUIEnabled { get; set; }
        public bool IsCreateTaskUIEnabled {  get; set; }
        public bool IsUIElementEnabled => IsCreateListUIEnabled || IsCreateTaskUIEnabled;
        public RelayCommand EnabledCreateListUI { get; }
        public RelayCommand TurnOffUIElements { get; }
        public ICommand AddTaskCommand { get; }
        public ICommand AddListCommand { get; }
        public MainWindowViewModel(List<TodoList> _todoLists)
        {
            EnabledCreateListUI = new RelayCommand(ToggleCreateListUICommand);
            TurnOffUIElements = new RelayCommand(TurnOffUIElementsCommand);
            AddTaskCommand = new AddTaskCommand(this);
            AddListCommand = new AddListCommand(this);

            TodoLists = _todoLists;
            SelectedListName = string.Empty;
            DueDate = DateTime.Now;
            MinimumDate = DateTime.Now;
        }


        public void ToggleCreateListUICommand(object param = null)
        {
            IsCreateListUIEnabled = !IsCreateListUIEnabled;
            UpdateProperties();
        }
        public void ToggleCreateTaskUICommand(object param = null)
        {
            IsCreateTaskUIEnabled = !IsCreateTaskUIEnabled;
            UpdateProperties();
        }
        private void TurnOffUIElementsCommand(object param)
        {
            IsCreateListUIEnabled = false;
            IsCreateTaskUIEnabled = false;
            UpdateProperties();
        }

        private void UpdateProperties()
        {
            OnPropertyChanged(nameof(IsCreateListUIEnabled));
            OnPropertyChanged(nameof(IsCreateTaskUIEnabled));
            OnPropertyChanged(nameof(IsUIElementEnabled));
        }
    }
}
