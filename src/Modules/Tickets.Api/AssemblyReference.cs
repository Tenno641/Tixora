using System.Reflection;

namespace Tickets.Api;

public static class AssemblyReference
{
    public readonly static Assembly Assembly = Assembly.GetExecutingAssembly();
}