using ServiceBusLifeboat.Cli.Enums;

namespace ServiceBusLifeboat.Cli.Extensions;

public static class EnvironmentExtensions
{
    public static string GetSystemUserInfo()
    {
        var username = Environment.UserName;
        var machineName = Environment.MachineName;

        return $"{username}_{machineName}";
    }

    public static bool IsUnix()
    {
        string os = GetOperationalSystem();
        return os.Contains(nameof(OperationalSystem.Unix));
    }

    public static bool IsWin32NT()
    {
        string os = GetOperationalSystem();
        return os.Contains(nameof(OperationalSystem.Win32NT));
    }

    private static string GetOperationalSystem()
    {
        OperatingSystem os = Environment.OSVersion;
        return os.Platform.ToString();
    }
}