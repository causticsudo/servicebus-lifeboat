using System.Net.NetworkInformation;
using ServiceBusLifeboat.Cli.Extensions;

namespace ServiceBusLifeboat.Cli.Services;

public class NetworkInterfaceService
{
    public static string GetMacAddress()
    {
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (var @interface in networkInterfaces)
        {
            if (@interface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                @interface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            {
                var physicalAddress = @interface.GetPhysicalAddress();
                var macAddress = physicalAddress.ToString().FormatMacAddress();

                return macAddress.FormatMacAddress();
            }
        }

        return string.Empty;
    }
}