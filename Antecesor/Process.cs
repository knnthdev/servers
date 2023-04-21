using System;
using System.Linq;
using System.Diagnostics;
using System.Net.NetworkInformation;

internal class Process
{
    public static string GetNetworkName()
    {
        var redes = NetworkInterface.GetAllNetworkInterfaces();
        var filtro = redes.filter((it) => it.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 && it.OperationalStatus == OperationalStatus.Up);

        if (NetworkInterface.GetIsNetworkAvailable())
            if (filtro.ToArray().Length > 0)// si esta connectado a wifi
                return _getNameSSID();
            else
                return network_name();
        return "local";
    }

    static string network_name()
    {
        var redes = NetworkInterface.GetAllNetworkInterfaces();
        var filtro = redes.filter((it) => it.OperationalStatus == OperationalStatus.Up);
        var net = filtro.First();
        string name = net.Name;
        return name;
    }

    static string _getNameSSID()
    {
        string output = string.Empty;

        ProcessStartInfo startInfo = new()
        {
            FileName = "cmd.exe",
            Arguments = "/C netsh wlan show interfaces",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo))
            output = process.StandardOutput.ReadToEnd();

        string[] info = output.split(Environment.NewLine).Where(it => it.Contains(':')).ToArray();
        string ssid = info.Where(it => it.Contains("ssid") || it.Contains("SSID") || it.Contains("Sssid")).First().split(":")[1].Trim();
        return ssid;
    }

}
