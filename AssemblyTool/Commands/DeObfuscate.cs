using AssemblyLib;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using AssemblyLib.Utils;
using AssemblyLib.ReMapper;
using AssemblyTool.Utils;

namespace AssemblyTool.Commands;

[Command("DeObfuscate", Description = "Generates a de-obfuscated -cleaned dll in the folder your assembly is in")]
public class DeObfuscate : ICommand
{
    [CommandParameter(0, IsRequired = false, Description = "Is the target the EFT launcher?")]
    public bool IsLauncher { get; init; } = false;

    public ValueTask ExecuteAsync(IConsole console)
    {
        Debugger.TryWaitForDebuggerAttach();
        
        var app = new App();
        app.DeObfuscate(GlobalPaths.AssemblyCSharpPath, IsLauncher);
        
        return default;
    }
}