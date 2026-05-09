using System.Reflection;

namespace Tickets.Application;

public static class AssemblyReference
{
    public readonly static Assembly Assembly = Assembly.GetExecutingAssembly();
}