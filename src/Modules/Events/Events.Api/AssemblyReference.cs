using System.Reflection;

namespace Events.Api;

internal static class AssemblyReference
{
    public static Assembly Assembly => Assembly.GetExecutingAssembly();
}