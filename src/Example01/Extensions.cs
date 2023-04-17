namespace Example01;

public static class Extensions
{
    public const string ContinuousIntegrationEnvironmentName = "CI-ENV";
    
    public static bool IsContinuousIntegration(this IWebHostEnvironment environment)
    {
        return environment.IsEnvironment(ContinuousIntegrationEnvironmentName);
    }
}