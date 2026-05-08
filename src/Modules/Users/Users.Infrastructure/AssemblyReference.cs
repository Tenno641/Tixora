using System.Reflection;

namespace Users.Infrastructure;

internal static class AssemblyReference
{
    public static Assembly Assembly = Assembly.GetExecutingAssembly();
}