namespace ServiceBusLifeboat.Cli.Services;

public static class EnvironmentService
{
    public static string GetSystemUserInfo()
    {
        var username = Environment.UserName;
        var machineName = Environment.MachineName;

        return $"{username}_{machineName}";
    }
}