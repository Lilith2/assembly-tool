using AssemblyLib;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using AssemblyLib.Utils;
using AssemblyLib.Dumper;
using AssemblyTool.Utils;

namespace AssemblyTool.Commands;

[Command("Dumper", Description = "Generates a dumper zip")]
public class Dumper : ICommand
{
    public ValueTask ExecuteAsync(IConsole console)
    {
        Debugger.TryWaitForDebuggerAttach();
        
        var app = new App();
        app.CreateDumper(GlobalPaths.ManagedDirectory);
        
        return default;
    }
}