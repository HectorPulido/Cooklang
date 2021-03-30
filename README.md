# CookLang - A mock programing language
Coocklang is a mock programing language made with C# just for fun, sintaxis is similar to ASM.
[![youtube video](https://img.youtube.com/vi/hvVg72uCMt4/0.jpg)](https://www.youtube.com/watch?v=hvVg72uCMt4&list=PLHdk97dabfvzTZyKLb_spWHvI-qLArEzm&index=4)

## Features
- Custom actions and commands 
- Turing complete 

## TODO
- More examples
- ~~Unity implementation~~

## Examples
The project contains 3 different examples
- Factorial: y'know 5! = 1x2x3x4x5
- Fibonacci: like the bunnies, 1,1,2,3,5,8...
- Unity integration: This script makes a rigidbody to jump every x seconds, useful for a modding system

## How it works
Just like the basic turing machine, we have an pointer that moves arround the code, every line is an action, Every acction has a name, like 'print' or 'operation' the rest of the line is the "Argument". Every line must end with a semicolon ";" and every code must end with the end command
```
end
```
the commands are not case sensitive

### Print
This command shows something in the console
```
print Hello world;
```
the command also can print a variable, but lets talk about that later
```
print {myVariable};
```
### Jump
The pointer moves top to bottom for default, but you can change the flow by the command 
```
jump +1;
```
the argument of jump can be a number like 1, 5 or 99, that means that the pointer will move to that line, but the argument cant also be a number with sign, that means the pointer will move to the current line plus (or minus) the number.
Also exist the Jumpto command.
```
jumpto myZone;
```
This command is like a jump but instead of jump to a specific line, jumps to a zone
```
zone myZone;
```
A zone is a specific line of the code where the pointer can jump easily

### Variables
You can set or modify a variable just like this
```
assign variableName Value of the variable;
```

There are some reserved variable names like "Temporal", this variable keeps the value of an operation, like this
```
operation {variable1} / 10 ;
assign operationResult {Temporal};
```
### Comments
The line of code that starts with a # or any other special character will be ignored, like.
```
# This is a comment, will be ignored by the compiler;
@ But still must end with ;
```
### Conditinals
Like almost every language, this one have if statements, but this works a little different, this works like this
```
#x is 16
ir {{x} < 15} jumpto 1;
print this will not be printed;
print this WILL be printed;
```
thats a medium complex example, a simpler statement could be
```
if true jumpto 3;
print this will not be printed;
print this will not be printed;
print this will not be printed;
print this WILL be printed;
```
but the if statement can be as complex you want
```
assign x 20;
if {{{x} >= 15} and (not{{x} = 20}}) jumpto 1;
print not will be printed;
print will be printed;
```
#### just some rules: 
- you cant not make aritmetic operations in if statements
- every variable must be called inside this brackets { }
- same thing with comparations
- the valid comparations are =, !=, <, >, >=, <=
- every boolean statement (except the most general) must be inside a parentesys
- the valid booleans operation are and, or, not


## How to use
The language is increidible simple to use, you can use it from C# itself.

### From a file
To run a file code you can type this on the shell 
```
dotnet run -o /path/path/myfile.cl
```
or if you compile the proyect
```
dotnet cooklang.dll -o /path/path/myfile.cl
```

### C# Use
Just add Cooklang to your project, then import Cooklang like this.
```Csharp
using CookLang;
```
To use it just create a new compiler (indeed is an interpreter but... you know) and run it.
```Csharp
var compiler = new CookCompiler(code);
compiler.Run();
```

You can also add new actions and initial variables to the compiler like this
```Csharp
var customEvents = new Dictionary<string, Action<CookCompiler, string>>();
customEvents.Add("MyCustomAction", MyFunction);
//Or just
customEvents.Add("MyOtherCustomAction", (m, args)=>{
    Console.WriteLine(args + " foo");
});

var customVariables = new Dictionary<string, string>();
customVariables.Add("myVariable", jumpTime.ToString());

var compiler = new CookCompiler(code, customVariables, customEvents);
compiler.Run();
```

You also can execute the code line by line with this
```Csharp
var sw = true;
var compiler = new CookCompiler(code);
while(sw)
{
    cook.NextStep();
    sw = cook.running;
    foo();
}
```

## More interesting projects
I have a lot of fun projects, check this:

### Machine learning
- https://github.com/HectorPulido/Evolutionary-Neural-Networks-on-unity-for-bots
- https://github.com/HectorPulido/Imitation-learning-in-unity
- https://github.com/HectorPulido/Chatbot-seq2seq-C-

### Games
- https://github.com/HectorPulido/Unity-MMO-Framework
- https://github.com/HectorPulido/Contra-Like-game-made-with-unity
- https://github.com/HectorPulido/Pacman-Online-made-with-unity

### Random
- https://github.com/HectorPulido/Arithmetic-Parser-made-easy
- https://github.com/HectorPulido/Simple-php-blog
- https://github.com/HectorPulido/Decentralized-Twitter-with-blockchain-as-base

### You also can follow me in social networks
- Twitter: https://twitter.com/Hector_Pulido_
- Youtube: http://youtube.com/c/hectorandrespulidopalmar
- Twitch: https://www.twitch.tv/hector_pulido_

## Licence
This proyect was totally handcrafted by me, so the licence is MIT, use it as you want.
