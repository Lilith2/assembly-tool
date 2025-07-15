using AssemblyLib;
using AssemblyLib.AutoMatcher;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using AssemblyTool.Utils;

namespace AssemblyTool.Commands;

[Command("AutoMatch", Description = "This command will automatically try to generate a mapping object given old type and new type names.")]
public class AutoMatchCommand : ICommand
{
	[CommandParameter(0, IsRequired = true, Description = "Full old type name including namespace `Foo.Bar` for nested classes `Foo.Bar/FooBar`")]
	public required string OldTypeName { get; init; }
	
	[CommandParameter(1, IsRequired = true, Description = "The name you want the type to be renamed to")]
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
			false
		);
	}
}