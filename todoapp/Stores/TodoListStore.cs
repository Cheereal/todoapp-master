using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using todoapp.Model;
using static System.Formats.Asn1.AsnWriter;

namespace todoapp
{
    public class TodoListStore
    {
        public static List<TodoList> TodoLists { get; set; }

        private static string _path = "";
        private static string _jsonname = "todolists.json";
        private static string filepath => _path + _jsonname;

        public static void SerialiseToDoLists(List<TodoList> todoList)
        {
            TodoLists = todoList;
            SerialiseToDoLists();
        }
        public static void SerialiseToDoLists(TodoList todoList)
        {
            if(TodoLists.Find(list => list.ListName == todoList.ListName) != null)
            {
                int index = TodoLists.FindIndex(list => list.ListName == todoList.ListName);
                TodoLists[index] = todoList; // Update the list at the found index
                SerialiseToDoLists();
            }
            else
            {
                MessageBox.Show("ERROR: List could not be found.");
            }
        }

        public static void SerialiseToDoLists()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(TodoLists, Formatting.Indented);
                File.WriteAllText(filepath, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        public static List<TodoList> DeserializeTodoLists()
        {
            try
            {
                // if file doesnt exist, make one and return a new list
                if(!File.Exists(filepath))
                {
                    File.Create(filepath);
                    TodoLists = new List<TodoList>();
                    return new List<TodoList>();
                }
                // load list and deserialise the object
                string jsonString = File.ReadAllText(filepath);
                List<TodoList> lists = JsonConvert.DeserializeObject<List<TodoList>>(jsonString);
                // check for failed tasks
                foreach (TodoList list in lists)
                {
                    foreach(TodoTask task in list.Tasks)
                    {
                        if(task.IsTaskPastDueDate() && task.TaskStatus != TodoTask.TaskStates.Finished)
                        {
                            task.TaskStatus = TodoTask.TaskStates.Failed;
                        }
                    }
                }    
                // sort list
                List<TodoList> sortedLists = new List<TodoList>();
                foreach (TodoList list in lists)
                {
                    List<TodoTask> sortedTasks = MergeSort.Sort(list.Tasks);
                    sortedLists.Add(new TodoList
                    {
                        Tasks = sortedTasks,
                        ListName = list.ListName
                    });
                }
                // assign the loaded list into the static TodoLists property
                TodoLists = sortedLists;
                // return the list
                return TodoLists;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
    }
}
