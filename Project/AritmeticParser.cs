using System;
using System.Collections.Generic;

namespace CookLang
{
    class AritmeticParser
    {
        public static double? Evaluator(string input)
        {
            input = input.Replace(" ", "");

            if (input.Contains("(") || input.Contains(")"))
            {
                if (!ValidParenthesys(input))
                    return null;

                Dictionary<string, string> Changes = new Dictionary<string, string>();

                bool openPar = false;
                int openIndex = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    if (!openPar)
                    {
                        if (input[i] == '(')
                        {
                            openPar = true;
                            openIndex = i;
                        }
                    }
                    else
                    {
                        if (input[i] == ')')
                        {
                            string subInput = input.Substring(openIndex + 1, i - openIndex - 1);

                            if (!Changes.ContainsKey(subInput))
                            {
                                string evaluatedSubInput = Evaluator(subInput).ToString();
                                Changes.Add(subInput, evaluatedSubInput);
                            }
                            openPar = false;
                            openIndex = 0;
                        }
                    }
                }

                foreach (var item in Changes.Keys)
                {
                    input = input.Replace(item, Changes[item]);
                }
                input = input.Replace("(", "");
                input = input.Replace(")", "");
            }

            List<string> ParsedOperations = OperationParser(input);
            return OperationEvaluator(ref ParsedOperations);
        }
        static bool ValidParenthesys(string input)
        {
            int openPar = 0;
            int closePar = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                    openPar++;
                else if (input[i] == ')')
                    closePar++;
            }
            return openPar == closePar;
        }
        static double OperationEvaluator(ref List<string> OperationList)
        {
            while (OperationList.Count > 1)
            {
                int index = 0;
                while (index < OperationList.Count)
                {
                    if (OperationList[index] == "/")
                    {
                        double div1 = double.Parse(OperationList[index - 1]);
                        double div2 = double.Parse(OperationList[index + 1]);
                        OperationList[index - 1] = (div1 / div2).ToString();
                        OperationList.RemoveAt(index);
                        OperationList.RemoveAt(index);
                    }
                    index++;
                }
                index = 0;
                while (index < OperationList.Count)
                {
                    if (OperationList[index] == "*")
                    {
                        double mult1 = double.Parse(OperationList[index - 1]);
                        double mult2 = double.Parse(OperationList[index + 1]);
                        OperationList[index - 1] = (mult1 * mult2).ToString();
                        OperationList.RemoveAt(index);
                        OperationList.RemoveAt(index);
                    }
                    index++;
                }
                index = 0;
                while (index < OperationList.Count)
                {
                    if (OperationList[index] == "-")
                    {
                        double res1 = double.Parse(OperationList[index - 1]);
                        double res2 = double.Parse(OperationList[index + 1]);
                        OperationList[index - 1] = (res1 - res2).ToString();
                        OperationList.RemoveAt(index);
                        OperationList.RemoveAt(index);
                    }
                    index++;
                }
                index = 0;
                while (index < OperationList.Count)
                {
                    if (OperationList[index] == "+")
                    {
                        double sum1 = double.Parse(OperationList[index - 1]);
                        double sum2 = double.Parse(OperationList[index + 1]);
                        OperationList[index - 1] = (sum1 + sum2).ToString();
                        OperationList.RemoveAt(index);
                        OperationList.RemoveAt(index);
                    }
                    index++;
                }
            }
            return double.Parse(OperationList[0]);
        }
        static List<string> OperationParser(string input)
        {
            List<string> Operations = new List<string>();
            int index = 0;
            while (index < input.Length)
            {
                if (CharEvaluator(input[index]) == 0)
                {
                    string SubEvaluator = "";

                    for (int i = index; i < input.Length; i++)
                    {
                        if (CharEvaluator(input[i]) == 0)
                        {
                            SubEvaluator += input[i];
                            if (i == input.Length - 1)
                            {
                                Operations.Add(SubEvaluator);
                                index = input.Length;
                            }
                        }
                        else
                        {
                            Operations.Add(SubEvaluator);
                            index = i;
                            break;
                        }
                    }
                }
                else if (CharEvaluator(input[index]) == 1)
                {
                    Operations.Add(input[index].ToString());
                    index++;
                }
                else
                {
                    return null;
                }
            }
            return Operations;
        }
        static int CharEvaluator(char input)
        {
            return CharEvaluator(input.ToString());
        }
        static int CharEvaluator(string input)
        {
            if ("0987654321.,".Contains(input))
                return 0;
            else if ("/*-+".Contains(input))
                return 1;
            else if ("()".Contains(input))
                return 2;
            else
                return -1;
        }
    }
}