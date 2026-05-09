using System.Reflection;

namespace Tickets.Infrastructure;

public static class AssemblyReference
{
    public readonly static Assembly Assembly = Assembly.GetExecutingAssembly();
}