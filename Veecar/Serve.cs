using System;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;

namespace Veecar
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("1CE3E2DD-07DC-4005-B960-5CEDD25B22C4")]
    [ProgId("Serve")]
    public class Serve : IServe
    {
        private TcpListener listener;
        private CancellationToken token;
        private bool engine;

        private string backaddress;
        private int backport;

        const string ippattern = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

        [return: MarshalAs(UnmanagedType.Bool)]
        public static bool IsIP([MarshalAs(UnmanagedType.BStr)]string ip) => Regex.IsMatch(ip, ippattern) || ip.ToLower() == "localhost";

        public Action IfClosingOnFaulted;

        private Pragma.Instant Pragma;

        private void Init()
        {
            var localadress = () => Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault((ip) =>
            {
                var notation = ip.ToString();
                return Regex.IsMatch(notation, ippattern);
            });

            var address = localadress();
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                listener = new TcpListener(address, Program.Checkporthost(address.ToString(), 80).Await());
            else
                listener = new TcpListener(localadress(), Program.Checkporthost(address.ToString(), 5942).Await());

            token = new();

            Pragma = new(LocalEndPoint);
        }

        public Serve() => Init();

        public string LocalEndPoint { get => listener.LocalEndpoint.ToString(); }
        public string Log { get => Pragma.Channel; }

        public async Task start(string addressfocus, int portfocus) =>
            await Task.Run(() => _start(addressfocus, portfocus), token).Catch((err) =>
            {
                engine = false;
                Pragma.log($"mensaje revido con error: {err.Message}");
            });
        private async Task _start(string addressfocus, int portfocus)
        {
            var pending = Program.Checklocalhost(addressfocus, portfocus).then(it =>
            {
                backaddress = it;
            }).Catch((err, it) =>
            {
                token = new(false);
                IfClosingOnFaulted?.Invoke();
                Pragma.log($"Error:[{err}] al intentar obtener el puerto, intente verificar si el servidor esta en escucha");
            }).Await();
            backport = portfocus;

            listener.Start();
            Pragma.log($"ahora se encuentra abierto");
            engine = true;
            // Aceptar una conexión entrante
            while (engine)
            {
                if (!listener.Pending())
                    continue;

                //llamar a los clientes
                TcpClient client1 = await listener.AcceptTcpClientAsync();

                // Obtener un flujo desde la conexión de los clientes
                NetworkStream stream1 = client1.GetStream();

                TcpClient client2 = new(backaddress, backport);

                NetworkStream stream2 = client2.GetStream();

                // Leer la información enviada del primer cliente
                byte[] buffer1 = await stream1.ReadToEndAsync();
                if (buffer1.Length == 0)
                    goto jump;

                string data1 = Encoding.ASCII.GetString(buffer1, 0, 90);
                var split = data1.Split(' ');
                var metodo = split[0];
                var path = split[1];

                // Enviar datos al cliente dos
                await stream2.WriteAsync(buffer1, 0, buffer1.Length);

                //ManualResetEvent wait = new(stream2.DataAvailable.Callback(()=> Pragma.log("..Esperando respuesta..")));
                //if (!wait.WaitOne(50000))
                //    Pragma.log("Respuesta alargada, Continuando con la operacion");

                // Leer datos del segundo cliente como respuesta
                byte[] buffer2 = await stream2.ReadToEndAsync();

                // Enviar una respuesta al primer cliente
                await stream1.WriteAsync(buffer2, 0, buffer2.Length);
                Pragma.log($"[{metodo} {path}] Recivimos {buffer1.Length}b y enviamos {buffer2.Length}b ");
            jump:
                // Cerrar la conexión
                stream1.Close();
                client1.Close();
                stream2.Close();
                client2.Close();
            }
            listener.Stop();
            token = new(false);
            Pragma.log("Conexión cerrada.");
        }

        public void Reconect()
        {
            if (engine)
            {
                stop();
                Init();
                Pragma.log("\nReconectando servicios\n");
                _ = start(backaddress, backport);
            }
            else
                Init();
        }

        public void stop()
        {
            if (engine)
            {
                engine = false;
                token = new(true);
                Pragma.log("Deteniendo el proceso...");
            }
        }

        [return: MarshalAs(UnmanagedType.BStr)]
        public static string GetServer() => "Hola Kenneth";

        ~Serve()
        {
            engine = false;
            listener.Stop();
            listener = null;
            //Pragma.log("proceso detenido con exito.");
        }
    }
}
