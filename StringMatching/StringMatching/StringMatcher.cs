using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringMatching
{
    public class StringMatcher
    {
        private const string _OffByOneLiteral = ".";
        private const string _OffByManyLiteral = "*";

        public static bool IsMatch(string pattern, string str)
        {
            var patternMatrix = _GetPatternMatrix(pattern, str);

            return _HasCompletePath(patternMatrix, pattern.Length-1, str.Length-1, pattern);
        }

        private static bool _HasCompletePath(IList<List<bool>> matrix, int patternIndex, int stringIndex, string pattern)
        {
            var result = false;

            if(patternIndex < 0 || stringIndex < 0)
                return false;

            if (patternIndex == 0 && stringIndex == 0 && matrix[patternIndex][stringIndex])
            {
                result = true;
            }
            else if(matrix[patternIndex][stringIndex])
            {
                var tempResult = _HasCompletePath(matrix, patternIndex - 1, stringIndex - 1, pattern);
                if(!tempResult && _HasCompletePath(matrix, patternIndex, stringIndex-1, pattern) && pattern[patternIndex] == '*')
                {
                    tempResult = true;
                }
                result |= tempResult;
            }

            return result;
        }

        private static List<List<bool>> _GetPatternMatrix(string pattern, string str)
        {
            var patternMatrix = new List<List<bool>>(pattern.Length);
            for (var i = 0; i < pattern.Length; i++)
                patternMatrix.Add(new List<bool>(str.Length));

            //First fill out table
            foreach(var patternIndex in Enumerable.Range(0, pattern.Length))
            {
                var patternCharacter = pattern[patternIndex];

                var isLiteralMatch = patternCharacter != '*' && patternCharacter != '.';
                foreach(var stringIndex in Enumerable.Range(0, str.Length))
                {
                    var stringCharacter = str[stringIndex];

                    if(isLiteralMatch)
                    {
                        patternMatrix[patternIndex].Add(_IsMatch(patternCharacter, stringCharacter));
                    }
                    else
                    {
                        patternMatrix[patternIndex].Add(true);
                    }

                }
            }
            return patternMatrix;
        }

        private static bool _IsMatch(char pattern, char str)
        {
            if (pattern == '*')
                return true;
            if(pattern == '.')
                return true;
            if(pattern == str)
                return true;

            return false;
        }
    }
}
