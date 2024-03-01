using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace todoapp.Model
{
    class MergeSort
    {

        public static List<TodoTask> Sort(List<TodoTask> unsortedList)
        {
            if(IsSorted(unsortedList)) 
                return unsortedList;
            if (unsortedList == null || unsortedList.Count <= 1)
                return unsortedList;

            // split the list up into 2
            int mid = unsortedList.Count / 2;
            List<TodoTask> leftHalf = new List<TodoTask>(unsortedList.GetRange(0, mid));
            List<TodoTask> rightHalf = new List<TodoTask>(unsortedList.GetRange(mid, unsortedList.Count - mid));

            leftHalf = Sort(leftHalf);
            rightHalf = Sort(rightHalf);

            return Merge(leftHalf, rightHalf);
        }

        private static List<TodoTask> Merge(List<TodoTask> left, List<TodoTask> right)
        {
            List<TodoTask> merged = new List<TodoTask>();
            int leftIndex = 0;
            int rightIndex = 0;

            // avoid indexOutOfRange error
            while (leftIndex < left.Count && rightIndex < right.Count)
            {
                if (string.Compare(left[leftIndex].TaskName.ToLower(), right[rightIndex].TaskName.ToLower(), StringComparison.Ordinal) <= 0)
                {
                    merged.Add(left[leftIndex]); // left goes before right
                    leftIndex++;
                }
                else
                {
                    merged.Add(right[rightIndex]); // right goes before left
                    rightIndex++;
                }
            }

            // This area adds any left overs onto merge
            while (leftIndex < left.Count)
            {
                merged.Add(left[leftIndex]);
                leftIndex++;
            }

            while (rightIndex < right.Count)
            {
                merged.Add(right[rightIndex]);
                rightIndex++;
            }

            return merged;
        }

        static bool IsSorted(List<TodoTask> list)
        {
            if(list == null)
            {
                MessageBox.Show("List is NULL");
                return false;
            }
            // If the list has fewer than 2 elements, it's considered sorted
            if (list.Count < 2)
                return true;

            // Iterate through the list to check if each element is less than or equal to the next one
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (string.Compare(list[i].TaskName.ToLower(), list[i + 1].TaskName.ToLower(), StringComparison.Ordinal) > 0)
                {
                    // If any element is greater than the next one, the list is not sorted
                    return false;
                }
            }

            // If the loop completes without finding any unsorted elements, the list is sorted
            return true;
        }
    }
}
