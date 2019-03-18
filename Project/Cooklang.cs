using System;
using System.Collections.Generic;
using System.Globalization;

namespace CookLang
{
  public class CookCompiler
  {
    string[] lines;
    int pointer;
    bool running = true;
    
    Dictionary<string, Action<String>> customEvents = new Dictionary<string, Action<String>>(); 
    
    Dictionary<string, string> variables = new Dictionary<string, string>();
    Dictionary<string, int> zones = new Dictionary<string, int>();

    Stack<int> returnStack = new Stack<int>();
    CultureInfo ci = new CultureInfo("en-US");

    string error = "";
    
    public CookCompiler(string code)
    {
      Clean(code);
      SetZones();
      Initialize();
      SetCustomEvents();
    }
    public void SetCustomEvents()
    {
      customEvents = new Dictionary<string, Action<String>>(); 
      this.customEvents.Add("Print", Console.WriteLine);
      this.customEvents.Add("Error", Console.Error.WriteLine);

    }
    public void SetCustomEvents(Dictionary<string, Action<String>> customEvents)
    {
      this.customEvents = customEvents;

      if(!this.customEvents.ContainsKey("Print"))
      {
        this.customEvents.Add("Print", Console.WriteLine);
      }
      if(!this.customEvents.ContainsKey("Error"))
      {
        this.customEvents.Add("Error", Console.Error.WriteLine);
      }
    }

    void Initialize()
    {
      returnStack = new Stack<int>();
      variables = new Dictionary<string, string>();
      variables.Add("Temporal", "");
      pointer = 0;
    }

    public bool TicTac(bool startFromZero = false)
    {
      bool tic = true;
      if(startFromZero)
      {
        Initialize();
      }

      if(running && error == "")
      {
        Run(lines[pointer]);
      }

      if(error != "")
      {
        customEvents["Error"].Invoke("Error: " + error);
        tic = false;
      }
      else
      {
        customEvents["Print"].Invoke("Succeedful runtime");
        tic = false;
      }
      return tic;
    }

    public void Run(bool startFromZero = true)
    {
      if(startFromZero)
      {
        Initialize();
      }

      while(running && error == "")
      {
        Run(lines[pointer]);
      }
      if(error != "")
      {
        customEvents["Error"].Invoke("Error: " + error);
      }
      else
      {
        customEvents["Print"].Invoke("Succeedful runtime");
      }
    }

    void SetZones()
    {
      for(int i = 0 ; i < lines.Length; i++)
      {
        if(lines[i].StartsWith("Zone", true, ci))
        {
          string zone = lines[i].Substring(4);
          zone = RemoveSpacesAtStart(zone);
          zones.Add(zone, i);
        }
      }
    }


    void Run(string code)
    {
      if(lines[pointer].StartsWith("RETURN", true, ci))
      {
        
        if(returnStack.Count == 0)
        {
          pointer ++;
          return;
        }
        
        pointer = returnStack.Pop() + 1;
        return;
      }
      if(lines[pointer].StartsWith("END", true, ci))
      {
        running = false;
        return;
      }
      if(lines[pointer].StartsWith("Zone", true, ci))
      {
        pointer ++;
        return;
      }
      if(lines[pointer].StartsWith("#", true, ci)) /// COMENTS
      {
        pointer ++;
        return;
      }
      if(lines[pointer].StartsWith("Print", true, ci))
      {
        var l = ApplyVariablesInString(lines[pointer].Substring(5));
        customEvents["Print"].Invoke(l);
        pointer ++;
        return;
      }
      if(lines[pointer].StartsWith("JumpTo", true, ci))
      {
        returnStack.Push(pointer);
        JumpTo(lines[pointer].Substring(6));
        return;
      }
      if(lines[pointer].StartsWith("Jump", true, ci))
      {
        returnStack.Push(pointer);
        Jump(lines[pointer].Substring(4));
        return;
      }
      if(lines[pointer].StartsWith("Assign", true, ci))
      {
        Assign(lines[pointer].Substring(6));
        pointer ++;
        return;
      }
      if(lines[pointer].StartsWith("Update", true, ci))
      {
        Update(lines[pointer].Substring(6));
        pointer ++;
        return;
      }
      if(lines[pointer].StartsWith("If", true, ci))
      {
        IfJumpTo(lines[pointer].Substring(2));
        return;
      }
      if(lines[pointer].StartsWith("Operation", true, ci))
      {
        Operation(lines[pointer].Substring(9));
        pointer ++;
        return;
      }

      foreach(var key in customEvents.Keys)
      {
        if(lines[pointer].StartsWith("key", true, ci))
        {
          customEvents[key].Invoke(lines[pointer]);
          pointer ++;
          return;
        }
      }


      error = $"Command not found in line {pointer}";
      return;
    }
    void IfJumpTo(string line)
    {
      line = line.Replace(" ", "");  
      var operators = line.Split(new[] { "jumpto" }, StringSplitOptions.None);
      
      if(operators.Length != 2)
      {
        error = $"Unexpected entry at {pointer}";
        return;
      }


      bool result = false;
      try
      {
        operators[0] = ApplyVariablesInString(operators[0]);
        operators[0] = ComparationParser.Evaluator(operators[0]);
        result = BooleanParser.EvaluatorFinal(BooleanParser.Evaluator(operators[0]));
      }
      catch
      {
        error = $"Conditional error at line {pointer}";
        return;
      }

      if(result)
      {
        pointer += int.Parse(operators[1]) + 1;
        return;
      }
      pointer ++;
      return;

    }

    void Operation(string line)
    {
      line = ApplyVariablesInString(line);
      try
      {
        line = AritmeticParser.Evaluator(line).ToString();
      }
      catch
      {
        error = $"Syntax in line {pointer}";
        return;
      }
      variables["Temporal"] = line;
    }

    void Update(string line)
    {
      line = RemoveSpacesAtStart(line);
      var v = line.Split(' ');  
      v[0] = v[0].Replace(" " , "");
      v[1] = v[1].Replace(" " , "");

      if(!variables.ContainsKey(v[0]))
      {
        error = $"The variable {v[0]} does not exist";
        return;
      }

      var data = ApplyVariablesInString(v[1]);

      variables[v[0]] = data;
    }
    void Assign(string line)
    {     
      line = RemoveSpacesAtStart(line);

      var v = line.Split(' ');  
      v[0] = v[0].Replace(" " , "");
      v[1] = v[1].Replace(" " , "");

      var data = ApplyVariablesInString(v[1]);

      if(variables.ContainsKey(v[0]))
      {
        error = $"The variable {v[0]} is already defined";
        return;
      }

      variables.Add(v[0], data);
    }
    void JumpTo(string zoneToJump)
    {
        zoneToJump = RemoveSpacesAtStart(zoneToJump);
        if(zones.ContainsKey(zoneToJump))
        {
          pointer = zones[zoneToJump];
        }
        else
        {
          error = $"Jump zone {zoneToJump} in line: {pointer} does not exist";
        }
    }
    void Jump(string line)
    {  
      line = RemoveSpacesAtStart(line);

      var sign = "";
      if(line[0] == '+' || line[0] == '-')
      {
        sign = line[0].ToString();
        line = line.Substring(1);
      }

      int i = 0;
      if (!Int32.TryParse(line, out i))
      {
        error = $"Jump parameter is allways a integer number {pointer}";
        return;
      }
      if(i > lines.Length || i <= 0)
      {
        error = $"index is outside of the range at {pointer}";
        return;
      }
      if(sign == "")
      {
        pointer = i;
        return;
      }
      else if(sign == "+")
      {
        pointer = i + pointer + 1; 
        return;
      }
      else if(sign == "-")
      {
        pointer = pointer - i;
        return;
      }

    }

    string ApplyVariablesInString(string line)
    {
        foreach(var item in variables)
        {
          line = line.Replace ("{" + item.Key + "}" , item.Value);
        }

        return line;
    }
    string RemoveSpacesAtStart(string line)
    {
      while(line[0] == ' ') 
        line = line.Substring(1);
      return line;
    }
    void Clean(string code)
    {
      while(code.Contains("  ")) 
        code = code.Replace("  ", " ");
      code = code.Replace("\r", "").Replace("\n","").Replace("\t","");        
      lines = code.Split(';');
    }
    

  }
}