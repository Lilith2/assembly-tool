using AssemblyLib;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using AssemblyTool.Utils;

namespace AssemblyTool.Commands;

[Command("ReMap", Description = "Generates a re-mapped dll provided a mapping file and dll. If the dll is obfuscated, it will automatically de-obfuscate.")]
public class ReMap : ICommand
{
    public async ValueTask ExecuteAsync(IConsole console)
    {
        Debugger.TryWaitForDebuggerAttach();

        var outPath = Path.GetDirectoryName(GlobalPaths.AssemblyCSharpPath);

        if (outPath is null)
        {
            throw new DirectoryNotFoundException("OutPath could not be resolved.");
        }

        var app = new App();
        await app.RunRemapProcess(
            GlobalPaths.AssemblyCSharpPath,
            GlobalPaths.OldAssemblyCSharpPath,
            outPath
        );
    }
}