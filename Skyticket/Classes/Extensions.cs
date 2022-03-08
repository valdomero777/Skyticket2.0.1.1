using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;

namespace Skyticket
{
    static class Extensions
    {
        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }

    }
}
