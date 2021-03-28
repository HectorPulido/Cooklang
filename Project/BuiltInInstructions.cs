using System;

namespace CookLang
{
    public static class BuiltInInstructions
    {

        public static void IfJumpTo(CookCompiler machine, string line)
        {
            var operators = line.Split(new[] { "jumpto" }, StringSplitOptions.None);
            var conditional = operators[0].Trim();
            conditional = machine.ApplyVariablesInString(conditional);

            bool result = false;
            try
            {
                conditional = ComparationParser.Evaluator(conditional);
                conditional = BooleanParser.Evaluator(conditional);
                result = BooleanParser.EvaluatorFinal(conditional);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
                machine.error = $"Conditional error at line {machine.pointer}";
                return;
            }

            if (result)
            {
                machine.returnStack.Push(machine.pointer);
                machine.pointer += int.Parse(operators[1], Utils.usNumberFormat) + 1;
                return;
            }
            machine.pointer++;
        }

        public static void Return(CookCompiler machine, string line)
        {
            if (machine.returnStack.Count == 0)
            {
                machine.pointer++;
                return;
            }

            machine.pointer = machine.returnStack.Pop() + 1;
        }

        public static void End(CookCompiler machine, string line)
        {
            machine.running = false;
            return;
        }

        public static void Jump(CookCompiler machine, string line)
        {
            machine.returnStack.Push(machine.pointer);

            var sign = "";
            if (line[0] == '+' || line[0] == '-')
            {
                sign = line[0].ToString();
                line = line.Substring(1).Trim();
            }

            int i = 0;
            if (!Int32.TryParse(line, out i))
            {
                machine.error = $"Jump parameter must to be integer number at {machine.pointer}";
                return;
            }
            if (i > machine.lines.Length || i <= 0)
            {
                machine.error = $"index is outside of the range at {machine.pointer}";
                return;
            }

            if (sign == "")
            {
                machine.pointer = i;
            }
            else if (sign == "+")
            {
                machine.pointer = i + machine.pointer + 1;
            }
            else if (sign == "-")
            {
                machine.pointer = machine.pointer - i;
            }
        }

        public static void Assign(CookCompiler machine, string line)
        {
            var v = line.Split(' ');
            var name = v[0].Trim().Replace(" ", "");
            var data = line.Replace($"{name} ", "");
            data = machine.ApplyVariablesInString(data);

            machine.pointer++;

            if (machine.variables.ContainsKey(name))
            {
                machine.variables[name] = data;
                return;
            }

            machine.variables.Add(name, data);
        }

        public static void GetValue(CookCompiler machine, string line)
        {
            var v = line.Split(' ');
            var name = v[0].Trim();
            var id1 = int.Parse(v[1].Trim(), Utils.usNumberFormat);
            var id2 = v.Length == 3 ? int.Parse(v[2].Trim(), Utils.usNumberFormat) : -1;

            machine.pointer++;

            if (id2 == -1)
            {
                machine.variables["Temporal"] = machine.variables[name].Substring(id1);
                return;
            }
            machine.variables["Temporal"] = machine.variables[name].Substring(id1, id2);
        }

        public static void Print(CookCompiler machine, string line)
        {
            line = machine.ApplyVariablesInString(line);
            Console.WriteLine(line);
            machine.pointer++;
        }

        public static void JumpTo(CookCompiler machine, string zoneToJump)
        {
            machine.returnStack.Push(machine.pointer);
            if (machine.zones.ContainsKey(zoneToJump))
            {
                machine.pointer = machine.zones[zoneToJump];
            }
            else
            {
                machine.error = $"Jump zone {zoneToJump} in line: {machine.pointer} does not exist";
            }
            machine.pointer++;
        }

        public static void Operation(CookCompiler machine, string line)
        {
            line = machine.ApplyVariablesInString(line);
            try
            {
                line = AritmeticParser.Evaluator(line).ToString();
            }
            catch
            {
                machine.error = $"Syntax in line {machine.pointer}";
                return;
            }
            machine.variables["Temporal"] = line;
            machine.pointer++;
        }
    }
}