﻿using AsmResolver;
using AsmResolver.DotNet;
using AssemblyLib.Models;
using AssemblyLib.Utils;
using SPTarkov.DI.Annotations;

namespace AssemblyLib.AutoMatcher.Filters;

[Injectable]
public class MethodFilters(
    DataProvider dataProvider
    )
{
    private List<string>? MethodsToIgnore;
    
    public bool Filter(TypeDefinition target, TypeDefinition candidate, MethodParams methods)
    {
        MethodsToIgnore ??= dataProvider.Settings.MethodNamesToIgnore;
        
        // Target has no methods and type has no methods
        if (!target.Methods.Any() && !candidate.Methods.Any())
        {
            methods.MethodCount = 0;
            return true;
        }
		
        // Target has no methods but type has methods
        if (!target.Methods.Any() && candidate.Methods.Any()) return false;
		
        // Target has methods but type has no methods
        if (target.Methods.Any() && !candidate.Methods.Any()) return false;
		
        // Target has a different number of methods
        if (target.Methods.Count != candidate.Methods.Count) return false;
        
        // Methods in target that are not in candidate
        var includeMethods = GetFilteredMethodNamesInType(target)
            .Except(GetFilteredMethodNamesInType(candidate));
		
        // Methods in candidate that are not in target
        var excludeMethods = GetFilteredMethodNamesInType(candidate)
            .Except(GetFilteredMethodNamesInType(target));
		
        methods.IncludeMethods.UnionWith(includeMethods);
        methods.ExcludeMethods.UnionWith(excludeMethods);
		
        methods.MethodCount = target.Methods
            .Count(m => 
                m is { IsConstructor: false, IsGetMethod: false, IsSetMethod: false, IsSpecialName: false });

        if (target.Methods.Any(m => m is { IsConstructor: true, Parameters.Count: > 0 }))
        {
            methods.ConstructorParameterCount = target.Methods.First(m => 
                    m is { IsConstructor: true, Parameters.Count: > 0 })
                .Parameters.Count;
        }
		
        // True if we have common methods, or all methods are constructors
        return HasCommonMethods(target, candidate) || target.Methods.All(m => m.IsConstructor);
    }
    
    /// <summary>
    /// Filter method names to those we can use for matching, do not include interface pre-appended method names,
    /// or any that are de-obfuscator given 
    /// </summary>
    /// <param name="type">Type to clean methods on</param>
    /// <returns>A collection of cleaned method names</returns>
    private IEnumerable<string> GetFilteredMethodNamesInType(TypeDefinition type)
    {
        return type.Methods
            .Where(m => m is { IsConstructor: false, IsGetMethod: false, IsSetMethod: false })
            // Don't match de-obfuscator given method names
            .Where(m => !MethodsToIgnore.Any(mi => 
                m.Name!.StartsWith(mi) || m.Name!.Contains('.')))
            .Select(s => s.Name!.ToString());
    }

    /// <summary>
    /// Produce an intersecting set of methods by name and return if any are common
    /// </summary>
    /// <param name="target">Target type</param>
    /// <param name="candidate">Candidate type</param>
    /// <returns>True if there are common methods</returns>
    private bool HasCommonMethods(TypeDefinition target, TypeDefinition candidate)
    {
        return target.Methods
                // Get target methods that are not a constructor a get, or set method
            .Where(m => m is { IsConstructor: false, IsGetMethod: false, IsSetMethod: false })
            .Select(s => s.Name)
                // Produce a set of method names that exist in both the target and the candidate
            .Intersect(candidate.Methods
                // Get candidate methods that are not a constructor a get, or set method
                .Where(m => m is { IsConstructor: false, IsGetMethod: false, IsSetMethod: false })
                .Select(s => s.Name))
                // Is there any common methods?
            .Any();
    }
}