namespace Tixora.Bootstrapper.Extensions;

public static class ConfigurationExtensions
{
    public static void AddModulesConfiguration(this IConfigurationBuilder builder, string[] modules)
    {
        foreach (string name in modules)
        {
            builder.AddJsonFile($"modules.{name}.json", false, true);
            builder.AddJsonFile($"modules.{name}.Development.json", true, true);
        }
    }
}