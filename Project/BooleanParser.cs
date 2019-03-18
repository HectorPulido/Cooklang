using System;
using System.Collections.Generic;

namespace CookLang
{
    class BooleanParser
    {
      public static Dictionary<string, string> operationList = new Dictionary<string, string>(){
        {"t", "t"},
        {"f", "f"},
        {"nt", "f"},
        {"nf", "t"},
        {"tat", "t"},
        {"taf", "f"},
        {"fat", "f"},
        {"faf", "f"},
        {"tot", "t"},
        {"tof", "t"},
        {"fot", "t"},
        {"fof", "t"}
      };

      public static bool EvaluatorFinal(string input)
      {
        return input == "t";
      }
      
      public static string Evaluator(string input)
      {
        input = input.Replace(" ", "");
        input = input.ToLower();
        input = input.Replace("true", "t");
        input = input.Replace("false", "f");
        input = input.Replace("not", "n");
        input = input.Replace("and", "a");
        input = input.Replace("or", "o");

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

      return operationList[input];
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
  }      
}