using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Veecar
{
    internal class Program
    {
        internal static bool islib = true;
        public static void log(object msg)
        {
            if (!islib)
                Console.WriteLine(msg);
            else
                Pragma.log(msg);
        }
        static void Main(string[] args)
        {
            islib = false;
        }
        public static async Task<int> Checkporthost(string address, int port)
        {
            string output = string.Empty;
            ProcessStartInfo startInfo = new()
            {
                FileName = "cmd.exe",
                Arguments = "/C netstat -atn",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process process = Process.Start(startInfo))
                output = process.StandardOutput.ReadToEnd();

            string[] info = output.split(Environment.NewLine).ToArray();
            var list = info.Where(it => it.Contains(address + ":" + port));
            if (list.Count() == 1)
                return await Checkporthost(address, new Random(port).Next(5, 54699));
            return port;

        }
        public static async Task<string> Checklocalhost(string address, int port)
        {
            if (address.ToLower() != "localhost")
                return address;

            string output = string.Empty;

            ProcessStartInfo startInfo = new()
            {
                FileName = "cmd.exe",
                Arguments = "/C netstat -atn",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            log("Checkeando tipo de red local\n");
            using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo))
                output = process.StandardOutput.ReadToEnd();

            string[] info = output.split(Environment.NewLine).ToArray();
            if (info.Where(it=> it.Contains($"0.0.0.0:{port}")).FirstOrDefault() != null)
                return "127.0.0.1".After(()=>log("Protocolo Ipv4/Ivp6 en red local"));

            var list = info.Where(it => it.Contains("[::1]") || it.Contains("127.0.0.1")).
                Where(it => it.Contains($":{port}")).ToArray();
            if (list.Length > 0)
            {
                var item = list[0];
                if (item.Contains($"[::1]:{port}"))
                    //la conxion es ipv6
                    return "[::1]".After(()=>log(".. protolo Ipv6 red local\n"));
                return "127.0.0.1".After(() => log(".. protocolo Ipv4 red local\n"));
            }
            throw new Error() { Message = $"No se encuentra locahost:{port}" };
        }

    }
}
