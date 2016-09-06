# AssemblyInvoker

Small executable for invoking .NET methods in an assembly. 

Useful in build steps.

NuGet package: https://www.nuget.org/packages/AssemblyInvoker/

## Usage

```
AssemblyInvoker.exe [assembly] [class] [method] [args...]
```

## Example

```
AssemblyInvoker.exe MyAssembly.dll SomeNamespace.MyClass RunThatCode "42"
```