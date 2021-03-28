using System;
using System.Collections.Generic;

namespace CookLang
{
    class ComparationParser
    {
        public static string Evaluator(string input)
        {

            //Console.WriteLine(input);

            input = input.Replace(" ", "");
            input = input.ToLower();

            if (input.Contains("{") || input.Contains("}"))
            {
                if (!ValidKeys(input))
                {
                    input = input.Replace("{", " ");
                    input = input.Replace("}", " ");
                    return Evaluator(input);
                }

                Dictionary<string, string> Changes = new Dictionary<string, string>();

                bool openPar = false;
                int openIndex = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    if (!openPar)
                    {
                        if (input[i] == '{')
                        {
                            openPar = true;
                            openIndex = i;
                        }
                    }
                    else
                    {
                        if (input[i] == '}')
                        {
                            string subInput = input.Substring(openIndex + 1, i - openIndex - 1);

                            if (!Changes.ContainsKey(subInput))
                            {
                                string evaluatedSubInput = Evaluator(subInput);
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
            }

            input = input.Replace("{", " ");
            input = input.Replace("}", " ");

            if (!input.Contains(">=") &&
                !input.Contains("<=") &&
                !input.Contains("=") &&
                !input.Contains(">") &&
                !input.Contains("<"))
            {
                return input;
            }

            var resp = "";
            if (input.Contains(">="))
            {
                var v = input.Split(new[] { ">=" }, StringSplitOptions.None);
                if (v.Length == 2)
                {
                    try
                    {
                        resp = int.Parse(v[0], Utils.usNumberFormat) >= int.Parse(v[1], Utils.usNumberFormat) ? "true" : "false";
                    }
                    catch
                    {
                        resp = null;
                    }

                }
                else
                {
                    resp = null;
                }
            }
            else if (input.Contains("<="))
            {
                var v = input.Split(new[] { "<=" }, StringSplitOptions.None);
                if (v.Length == 2)
                {
                    try
                    {
                        resp = int.Parse(v[0], Utils.usNumberFormat) <= int.Parse(v[1], Utils.usNumberFormat) ? "true" : "false";
                    }
                    catch
                    {
                        resp = null;
                    }
                }
                else
                {
                    resp = null;
                }
            }
            else if (input.Contains(">"))
            {
                var v = input.Split('>');
                if (v.Length == 2)
                {
                    try
                    {
                        resp = int.Parse(v[0], Utils.usNumberFormat) > int.Parse(v[1], Utils.usNumberFormat) ? "true" : "false";
                    }
                    catch
                    {
                        resp = null;
                    }

                }
                else
                {
                    resp = null;
                }

            }
            else if (input.Contains("<"))
            {
                var v = input.Split('<');
                if (v.Length == 2)
                {
                    try
                    {
                        resp = int.Parse(v[0], Utils.usNumberFormat) < int.Parse(v[1], Utils.usNumberFormat) ? "true" : "false";
                    }
                    catch
                    {
                        resp = null;
                    }

                }
                else
                {
                    resp = null;
                }
            }
            else if (input.Contains("="))
            {
                var v = input.Split('=');
                if (v.Length == 2)
                {
                    resp = v[0] == v[1] ? "true" : "false";
                }
                else
                {
                    resp = null;
                }
            }

            return resp;
        }

        static bool ValidKeys(string input)
        {
            int openPar = 0;
            int closePar = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '{')
                    openPar++;
                else if (input[i] == '}')
                    closePar++;
            }
            return openPar == closePar;
        }
    }
}