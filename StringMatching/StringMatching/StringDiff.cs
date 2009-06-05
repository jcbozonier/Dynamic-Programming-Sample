using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringMatching
{
    public class StringDiff
    {
        public StringDeltaExecutionPlan Between(string a, string b)
        {
            var editDistanceMatrix = _GetDiffMatrix(a, b);
            var executionPlan = _CreateExecutionPlan(a, b, editDistanceMatrix);
            return executionPlan;
        }

        private StringDeltaExecutionPlan _CreateExecutionPlan(string oldString, string newString, List<List<int>> editDistanceMatrix)
        {
            var executionPlanBuilder = new StringDeltaExecutionPlan(oldString, newString);

            // String from bottom right corner of the matrix and work our
            // way up to the upper left corner.

            var oldStringIndex = oldString.Length - 1;
            var newStringIndex = newString.Length - 1;

            while (oldStringIndex >= 0 || newStringIndex >= 0)
            {
                // Deal with current cell
                
                // Decide how to move.
                var leftPathIsValid = oldStringIndex > 0;
                var topPathIsValid = newStringIndex > 0;

                if(leftPathIsValid && topPathIsValid)
                {
                    var leftPathValue = editDistanceMatrix[oldStringIndex - 1][newStringIndex];
                    var topLeftPathValue = editDistanceMatrix[oldStringIndex - 1][newStringIndex - 1];
                    var topPathValue = editDistanceMatrix[oldStringIndex][newStringIndex - 1];

                    if(topLeftPathValue <= leftPathValue && topLeftPathValue <= topPathValue)
                    {
                        if(oldString[oldStringIndex] != newString[newStringIndex])
                            executionPlanBuilder.Swap(newStringIndex, oldStringIndex);
                        newStringIndex--;
                        oldStringIndex--;
                        
                        continue;
                    }

                    if(leftPathValue <= topLeftPathValue && leftPathValue <= topPathValue)
                    {
                        executionPlanBuilder.Delete(oldStringIndex);
                        oldStringIndex--;

                        continue;
                    }

                    if(topPathValue <= topLeftPathValue && topPathValue <= leftPathValue)
                    {
                        executionPlanBuilder.Insert(newStringIndex);
                        newStringIndex--;

                        continue;
                    }
                }

                if (leftPathIsValid && 
                    editDistanceMatrix[oldStringIndex-1][newStringIndex] <= editDistanceMatrix[oldStringIndex][newStringIndex])
                {
                    executionPlanBuilder.Delete(oldStringIndex);
                    oldStringIndex--;
                    continue;
                }

                if(topPathIsValid && 
                    editDistanceMatrix[oldStringIndex][newStringIndex-1] <= editDistanceMatrix[oldStringIndex][newStringIndex])
                {
                    executionPlanBuilder.Insert(newStringIndex);
                    newStringIndex--;
                    continue;
                }

                if (!leftPathIsValid && !topPathIsValid)
                {
                    if(oldString[oldStringIndex] != newString[newStringIndex])
                    {
                        executionPlanBuilder.Swap(newStringIndex, oldStringIndex);
                    }

                    return executionPlanBuilder;
                }
            }

            return executionPlanBuilder;
        }

        private static List<List<int>> _GetDiffMatrix(string oldString, string newString)
        {
            var editDistanceMatrix = new List<List<int>>(oldString.Length);
            for (var i = 0; i < oldString.Length; i++)
                editDistanceMatrix.Add(new List<int>(newString.Length));

            //First fill out table
            foreach (var oldCharacterIndex in Enumerable.Range(0, oldString.Length))
            {
                foreach (var newCharacterIndex in Enumerable.Range(0, newString.Length))
                {
                    var editDistanceAtCell = _CalculateMinimumCellEditDistance(editDistanceMatrix, oldString, oldCharacterIndex, newString, newCharacterIndex);

                    editDistanceMatrix[oldCharacterIndex].Add(editDistanceAtCell);
                }
            }

            return editDistanceMatrix;
        }

        private static int _CalculateMinimumCellEditDistance(List<List<int>> matrix, string oldString, int oldCharacterIndex, string newString, int newCharacterIndex)
        {
            var oldChar = oldString[oldCharacterIndex];
            var newChar = newString[newCharacterIndex];

            var localEditDistance = oldChar == newChar ? 0 : 1;

            var leftArrayValue = localEditDistance + _GetCellValue(matrix, oldCharacterIndex - 1, newCharacterIndex);
            var topLeftArrayValue = localEditDistance + _GetCellValue(matrix, oldCharacterIndex - 1, newCharacterIndex - 1);
            var topArrayValue = localEditDistance + _GetCellValue(matrix, oldCharacterIndex, newCharacterIndex - 1);

            var minEditDistance = Math.Min(Math.Min(leftArrayValue, topLeftArrayValue), topArrayValue);

            return minEditDistance;
        }

        private static int _GetCellValue(List<List<int>> matrix, int oldCharacterIndex, int newCharacterIndex)
        {
            if (oldCharacterIndex < 0 && newCharacterIndex < 0)
                return 0;
            if (oldCharacterIndex < 0)
                return newCharacterIndex + 1;
            if (newCharacterIndex < 0)
                return oldCharacterIndex + 1;
            return matrix[oldCharacterIndex][newCharacterIndex];

        }
    }

    public class StringDeltaExecutionPlan
    {
        private string _OldString;
        private string _NewString;
        public readonly Stack<Step> Steps;

        public StringDeltaExecutionPlan(string oldString, string newString)
        {
            _OldString = oldString;
            _NewString = newString;
            Steps = new Stack<Step>();
        }

        public void Swap(int newStringIndex, int oldStringIndex)
        {
            Steps.Push(new SwapStep(newStringIndex, oldStringIndex));
        }

        public void Delete(int oldStringIndex)
        {
            Steps.Push(new DeleteStep(oldStringIndex));
        }

        public void Insert(int newStringIndex)
        {
            Steps.Push(new InsertStep(newStringIndex));
        }

        public string PrintStep(int index)
        {
            var steps = Steps.ToArray();
            return steps[index].Print(_OldString, _NewString);
        }

        public string PrintAllSteps()
        {
            var result = "";

            var steps = Steps.ToArray();
            foreach(var step in steps)
            {
                result += step.Print(_OldString, _NewString) + "\n";
            }

            return result;
        }
    }

    public class SwapStep : Step
    {
        private int _NewStringIndex;
        private int _OldStringIndex;

        public SwapStep(int newStringIndex, int oldStringIndex)
        {
            _NewStringIndex = newStringIndex;
            _OldStringIndex = oldStringIndex;
        }

        public override string Print(string oldString, string newString)
        {
            return String.Format("Old character \"{0}\" swapped with new character \"{1}\"",
                                 oldString[_OldStringIndex],
                                 newString[_NewStringIndex]);
        }
    }

    public class DeleteStep : Step
    {
        private int _OldStringIndex;

        public DeleteStep(int oldStringIndex)
        {
            _OldStringIndex = oldStringIndex;
        }

        public override string Print(string oldString, string newString)
        {
            return String.Format("Deleted old character \"{0}\"", oldString[_OldStringIndex]);
        }
    }

    public class InsertStep : Step
    {
        private int _NewStringIndex;

        public InsertStep(int newStringIndex)
        {
            _NewStringIndex = newStringIndex;
        }

        public override string Print(string oldString, string newString)
        {
            return String.Format("Inserted new character \"{0}\"", newString[_NewStringIndex]);
        }
    }

    public abstract class Step
    {
        public abstract string Print(string oldString, string newString);
    }
}
