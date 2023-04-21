using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Veecar
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("BBF7FBAD-7AB4-4564-B562-46896E38DF59")]
    public interface IServe
    {
        Task start([MarshalAs(UnmanagedType.BStr)] string ip, [MarshalAs(UnmanagedType.SysInt)] int port);
        void stop();
        void Reconect();

    }
}
