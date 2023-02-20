using EPRIN_Klient_V3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPRIN_Klient_V3
{
    public class Server
    {
        public static NetworkStream stream;
        public static string content;
        public static TcpClient client;
    }
}
