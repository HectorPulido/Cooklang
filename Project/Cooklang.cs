using System;
using System.Collections.Generic;

namespace CookLang
{
    [System.Serializable]
    public class CookCompiler
    {
        public Dictionary<string, Action<CookCompiler, String>> instructions;
        public Dictionary<string, string> variables;
        public Dictionary<string, int> zones;
        public Stack<int> returnStack;
        public string[] lines;
        public int pointer;
        public bool running = true;
        public string error = "";


        public CookCompiler(
            string code,
            Dictionary<string, string> initialVariables = null,
            Dictionary<string, Action<CookCompiler, String>> customInstructions = null
        )
        {
            CleanCode(code);
            Initialize();
            SetZones();
            SetVariables(initialVariables);
            SetInstructions(customInstructions);
        }

        private void CleanCode(string code)
        {
            while (code.Contains("  "))
            {
                code = code.Replace("  ", " ");
            }
            code = code.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            lines = code.Split(';');
        }

        private void SetZones()
        {
            zones = new Dictionary<string, int>();
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = ParseLine(lines[i]);
                string instruction = line[0].ToLower().Trim();
                if (instruction.Equals("zone"))
                {
                    string zone = line[1].Trim();
                    zones.Add(zone, i);
                }
            }
        }

        private void Initialize()
        {
            returnStack = new Stack<int>();
            instructions = new Dictionary<string, Action<CookCompiler, String>>();
            variables = new Dictionary<string, string>();
            pointer = 0;
        }

        private string[] ParseLine(string line)
        {
            return line.Split(' ');
        }

        private void SetVariables(Dictionary<string, string> initialVariables)
        {

            variables.Add("Temporal", "");
            if (initialVariables != null)
            {
                variables.AddRange<string, string>(initialVariables);
            }
        }

        private void SetInstructions(
            Dictionary<string, Action<CookCompiler, String>> customInstructions
        )
        {
            instructions.Add("error", (m, s) => Console.WriteLine(s));
            instructions.Add("print", BuiltInInstructions.Print);
            instructions.Add("jumpto", BuiltInInstructions.JumpTo);
            instructions.Add("return", BuiltInInstructions.Return);
            instructions.Add("end", BuiltInInstructions.End);
            instructions.Add("jump", BuiltInInstructions.Jump);
            instructions.Add("assign", BuiltInInstructions.Assign);
            instructions.Add("operation", BuiltInInstructions.Operation);
            instructions.Add("if", BuiltInInstructions.IfJumpTo);
            instructions.Add("getvalue", BuiltInInstructions.GetValue);

            if (customInstructions != null)
            {
                instructions.AddRange<string, Action<CookCompiler, String>>(customInstructions);
            }
        }

        public void NextStep()
        {
            if (pointer >= lines.Length)
            {
                running = false;
                return;
            }

            string currentLine = lines[pointer];
            string[] line = ParseLine(currentLine);
            string instruction = line[0].ToLower().Trim();
            string parameters = currentLine.Substring(instruction.Length).Trim();

            if (instructions.ContainsKey(instruction))
            {
                try
                {
                    instructions[instruction].Invoke(this, parameters);
                }
                catch (System.Exception)
                {
                    error = $"System error at line {pointer}";
                    return;
                }
            }
            else
            {
                pointer++;
            }
        }

        public string ApplyVariablesInString(string line)
        {
            foreach (var item in variables)
            {
                line = line.Replace("{" + item.Key + "}", item.Value);
            }

            return line;
        }

        public void Run()
        {
            while (running && error == "")
            {
                NextStep();
            }

            if (error != "")
            {
                instructions["error"].Invoke(this, "Error: " + error);
            }
            else
            {
                instructions["print"].Invoke(this, "Successful runtime");
            }
        }

    }
}