using AssemblyLib;
using AssemblyLib.AutoMatcher;
using CliFx.Attributes;
using CliFx.Infrastructure;
using AssemblyLib.Utils;
using AssemblyTool.Utils;

namespace AssemblyTool.Commands;

[Command("regensig", Description = "regenerates the signature of a mapping if it is failing")]
public class RegenerateSignature : CliFx.ICommand
{
	[CommandParameter(0, IsRequired = true, Description = "Full old type name including namespace `Foo.Bar` for nested classes `Foo.Bar/FooBar`")]
	public required string OldTypeName { get; init; }
	
	[CommandParameter(1, IsRequired = true, Description = "The new type name as listed in the mapping file")]
	public required string NewTypeName { get; init; }
	
    public async ValueTask ExecuteAsync(IConsole console)
    {
	    Debugger.TryWaitForDebuggerAttach();
	    
	    var app = new App();
	    await app.RunAutoMatcher(
		    GlobalPaths.AssemblyCSharpPath,
		    GlobalPaths.OldAssemblyCSharpPath,
		    OldTypeName,
		    NewTypeName,
		    true
	    );
    }
}