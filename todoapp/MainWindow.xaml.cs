using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using todoapp.ViewModel;
using static todoapp.TodoTask;

namespace todoapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, Border> TodoListsUIElementsDict = new Dictionary<string, Border>();
        private Dictionary<string, Border> TaskToTaskUIElement = new Dictionary<string, Border>();
        private Dictionary<Border, StackPanel> TodoListToTasksStackPanel = new Dictionary<Border, StackPanel>();
        private Dictionary<string, Border> TaskToTaskStatusIndicatorUIElementDict = new Dictionary<string, Border>();
        private MainWindowViewModel _mwvm;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _mwvm = (MainWindowViewModel)this.DataContext;
            _mwvm.CreateNewListUIPanel += CreateToDoList;
            _mwvm.CreateNewTaskUIPanel += CreateToDoTask;

            
            List<TodoList> todoLists =  TodoListStore.DeserializeTodoLists();
            foreach (TodoList todoList in todoLists)
            {
                CreateToDoList(todoList.ListName);
                foreach (TodoTask task in todoList.Tasks)
                {
                    CreateToDoTask(todoList.ListName, task.TaskName, task.DueDate, task.TaskStatus);
                }
            }
        }


        private void CreateToDoList(string todoListName)
        {
            // Create Border element
            Border border = new Border();
            border.Background = (Brush)FindResource("Background_Level_2");
            border.Width = 300;
            border.Margin = new Thickness(0, 0, 0, 10);
            border.MaxHeight = 350;

            // Create Grid element
            Grid grid = new Grid();

            DockPanel outterDockPanelForButtons = new DockPanel();
            outterDockPanelForButtons.HorizontalAlignment = HorizontalAlignment.Center;
            DockPanel innerDockPanelForButtons = new DockPanel();
            innerDockPanelForButtons.HorizontalAlignment = HorizontalAlignment.Right;
            // create add task button
            Button addTaskButton = new Button();
            addTaskButton.HorizontalAlignment = HorizontalAlignment.Right;
            addTaskButton.Width = 30;
            addTaskButton.Height = 30;
            addTaskButton.Margin = new Thickness(0, 0, 10, 0);
            addTaskButton.CommandParameter = todoListName;
            addTaskButton.Command = new RelayCommand(OpenAddTaskUI);
            TextBlock plusSymbol = new TextBlock();
            plusSymbol.Text = "+";
            plusSymbol.FontSize = 25;
            plusSymbol.Margin = new Thickness(0, -6, 0, 0);
            plusSymbol.Background = Brushes.Transparent;
            addTaskButton.Content = plusSymbol;

            // create remove list button
            Button removeTaskButton = new Button();
            removeTaskButton.Width = 30;
            removeTaskButton.Height = 30;
            removeTaskButton.Margin = new Thickness(0, 0, 10, 0);
            removeTaskButton.CommandParameter = todoListName;
            removeTaskButton.Command = new RelayCommand(DeleteList);
            TextBlock xSymbol = new TextBlock();
            xSymbol.Text = "x";
            xSymbol.FontSize = 23;
            xSymbol.Margin = new Thickness(0, -7, 0, 0);
            xSymbol.Background = Brushes.Transparent;
            removeTaskButton.Content = xSymbol;
            
            // add buttons to inner dockpanel 
            innerDockPanelForButtons.Children.Add(addTaskButton);
            innerDockPanelForButtons.Children.Add(removeTaskButton);

            // Create TextBlock
            TextBlock todoListTitle = new TextBlock();
            todoListTitle.Text = todoListName;
            todoListTitle.TextAlignment = TextAlignment.Center;
            todoListTitle.Foreground = (Brush)FindResource("Primary");
            todoListTitle.FontSize = 25;
            todoListTitle.TextWrapping = TextWrapping.Wrap;
            todoListTitle.TextTrimming = TextTrimming.CharacterEllipsis;
            todoListTitle.Width = 200;
            todoListTitle.HorizontalAlignment = HorizontalAlignment.Center;
            todoListTitle.VerticalAlignment = VerticalAlignment.Center;

            outterDockPanelForButtons.Children.Add(todoListTitle);
            outterDockPanelForButtons.Children.Add(innerDockPanelForButtons);

            // Define RowDefinitions
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });


            // Create Rectangle
            Rectangle rect = new Rectangle();
            rect.Height = 1;
            rect.Fill = (Brush)FindResource("Primary");
            rect.VerticalAlignment = VerticalAlignment.Bottom;
            rect.Width = 220;

            // Create ScrollViewer
            ScrollViewer scrollViewer = new ScrollViewer();
            Grid.SetRow(scrollViewer, 1);

            // Create StackPanel
            StackPanel stackPanel = new StackPanel();

            // Add elements to Grid
            grid.Children.Add(outterDockPanelForButtons);
            grid.Children.Add(rect);
            grid.Children.Add(scrollViewer);

            // Add StackPanel to ScrollViewer
            scrollViewer.Content = stackPanel;

            // Add Grid to Border
            border.Child = grid;
            // Add Border to the uniformListsGrid
            uniformListsGrid.Children.Add(border);
            TodoListsUIElementsDict.Add(todoListName, border);
            TodoListToTasksStackPanel.Add(border, stackPanel);
        }
        private void CreateToDoTask(string todoListName, string taskName, DateTime dueDate, TaskStates taskStatus)
        {
            Border itemTask = CreateToDoTaskItem(todoListName, taskName, dueDate, taskStatus);
            Border todoList = TodoListsUIElementsDict[todoListName];
            StackPanel tasksStackPanel = TodoListToTasksStackPanel[todoList];
            TaskToTaskUIElement.Add($"{todoListName}@//>{taskName}", itemTask);
            tasksStackPanel.Children.Add(itemTask);
        }
        private Border CreateToDoTaskItem(string todoListName, string taskName, DateTime dueDate, TaskStates taskStatus)
        {
            Border itemBorder = new Border();
            itemBorder.Background = (Brush)FindResource("Background");
            itemBorder.Margin = new Thickness(5);

            Grid grid = new Grid();

            DockPanel dockPanel = new DockPanel();

            // setup the red task indicator dial
            Border taskIndicator = new Border();
            taskIndicator.Margin = new Thickness(5, 0, 0, 0);
            taskIndicator.Height = 20;
            taskIndicator.Width = 20;
            if(taskStatus == TaskStates.Finished)
                taskIndicator.Background = Brushes.Green;
            else if (taskStatus == TaskStates.Unfinished)
                taskIndicator.Background = Brushes.DarkOrange;
            else
                taskIndicator.Background = Brushes.Red;
            taskIndicator.CornerRadius = new CornerRadius(10);

            StackPanel itemStackPanel = new StackPanel();
            itemStackPanel.Margin = new Thickness(10, 5, 5, 5);

            TextBlock itemTextBlock1 = new TextBlock();
            itemTextBlock1.Text = taskName;
            itemTextBlock1.Foreground = Brushes.White;

            TextBlock itemTextBlock2 = new TextBlock();
            itemTextBlock2.Text = $" - {dueDate.ToString("yyyy/MM/dd")}";
            itemTextBlock2.Foreground = Brushes.Red;

            itemStackPanel.Children.Add(itemTextBlock1);
            itemStackPanel.Children.Add(itemTextBlock2);
            
            dockPanel.Children.Add(taskIndicator);
            dockPanel.Children.Add(itemStackPanel);

            grid.Children.Add(dockPanel);
            if(taskStatus != TaskStates.Failed)
            {
                Button toggleTaskStatusButton = new Button();
                toggleTaskStatusButton.Opacity = 0;
                toggleTaskStatusButton.CommandParameter = $"{todoListName}@//>{taskName}";
                toggleTaskStatusButton.Command = new RelayCommand(ToggleTaskStatus);
                grid.Children.Add(toggleTaskStatusButton);
            }

            Button deleteTaskButton = new Button();
            deleteTaskButton.VerticalAlignment = VerticalAlignment.Center;
            deleteTaskButton.HorizontalAlignment = HorizontalAlignment.Right;
            deleteTaskButton.Height = 25;
            deleteTaskButton.Width = 25;
            deleteTaskButton.Content = "X";
            deleteTaskButton.Margin = new Thickness(0, 0, 10, 0);
            deleteTaskButton.Background = (Brush)FindResource("Primary");
            deleteTaskButton.BorderBrush = Brushes.Black;
            deleteTaskButton.CommandParameter = $"{todoListName}@//>{taskName}";
            deleteTaskButton.Command = new RelayCommand(DeleteTaskFromList);

            grid.Children.Add(deleteTaskButton);

            itemBorder.Child = grid;


            TaskToTaskStatusIndicatorUIElementDict.Add($"{todoListName}@//>{taskName}", taskIndicator);
            return itemBorder;
        }

        private void ToggleTaskStatus(object param)
        {
            string todoListName = param.ToString().Split("@//>")[0];
            string taskName = param.ToString().Split("@//>")[1];
            //MessageBox.Show($"ListName:{todoListName}\nTaskName:{taskName}");
            TodoList todoList = _mwvm.TodoLists.Find(todolist => todolist.ListName == todoListName);
            TodoTask selectedTask = todoList.Tasks.Find(task => task.TaskName == taskName);
            Border selectedTaskUI = TaskToTaskStatusIndicatorUIElementDict[param.ToString()];
            
            if(selectedTask.TaskStatus == TodoTask.TaskStates.Finished)
            {
                selectedTask.TaskStatus = TodoTask.TaskStates.Unfinished;
                selectedTaskUI.Background = Brushes.DarkOrange;
            }
            else
            {
                selectedTask.TaskStatus = TodoTask.TaskStates.Finished;
                selectedTaskUI.Background = Brushes.Green;
            }
            TodoListStore.SerialiseToDoLists(todoList);
        }
        private void DeleteTaskFromList(object param)
        {
            // grab parameter values
            string todoListName = param.ToString().Split("@//>")[0];
            string taskName = param.ToString().Split("@//>")[1];
            
            // grab the task ui element
            Border selectedTaskUIElement = TaskToTaskUIElement[param.ToString()];
            TodoList todoList = _mwvm.TodoLists.Find(todolist => todolist.ListName == todoListName);
            // remove task from class
            todoList.RemoveTask(taskName);
            // remove task from UI
            StackPanel tasksStackPanel = TodoListToTasksStackPanel[TodoListsUIElementsDict[todoListName]];
            tasksStackPanel.Children.Remove(selectedTaskUIElement);
            // Serialise the list
            TodoListStore.SerialiseToDoLists(todoList);
        }
        private void DeleteList(object listname)
        {
            // remove list from data store
            _mwvm.TodoLists.Remove(_mwvm.TodoLists.Find(list => list.ListName == listname.ToString()));
            // remove list from UI
            Border ListUIElement = TodoListsUIElementsDict[listname.ToString()];
            uniformListsGrid.Children.Remove(ListUIElement);
            // serialise changes
            TodoListStore.SerialiseToDoLists(_mwvm.TodoLists);
        }
        private void OpenAddTaskUI(object listname)
        {
            _mwvm.SelectedListName = listname.ToString();
            _mwvm.ToggleCreateTaskUICommand();
        }
    }
}
