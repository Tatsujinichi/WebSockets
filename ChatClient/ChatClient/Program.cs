#region

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace ChatClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           // var base64Stringtest = ComputeHashBase64String("V2ViU29ja2V0IHJvY2tzIQ==");


            TcpListener server = null;
            try
            {
                CancellationTokenSource source = null;
                Console.WriteLine("Starting up listener");

                server = new TcpListener(IPAddress.Parse("127.0.0.1"), 555);
                server.Start();
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                byte[] key;
                using (NetworkStream stream = client.GetStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var handshake = new string[13];
                        for (int i = 0; i < 12; i++)
                        {
                            handshake[i] = reader.ReadLine();
                            Console.WriteLine(handshake[i]);
                        }
                        string[] handShakeSplit = handshake[7].Split(new[] {':'});
                        string stringKey = handShakeSplit[1].TrimStart(new[] {' '});

                        var base64String = ComputeHashBase64String(stringKey);

                        Console.WriteLine();

                        string reply = "HTTP/1.1 101 Switching Protocols" +
                            Environment.NewLine + "Upgrade: websocket" + 
                            Environment.NewLine + "Connection: Upgrade" + 
                            Environment.NewLine + "Sec-WebSocket-Accept: " + 
                            base64String
                            + Environment.NewLine
                            + Environment.NewLine;

                        Console.WriteLine(reply);

                        Console.WriteLine();

                        byte[] res = Encoding.UTF8.GetBytes(reply);
                        stream.Write(res, 0, res.Length);

                        Console.WriteLine("reading");
                        reader.ReadLine();
                        Console.WriteLine("end");
                        Console.ReadKey();
                    }
                }

                try
                {
                    source = new CancellationTokenSource();
                    Task t = Task.Factory.StartNew(() => Action(source.Token), source.Token);
                    t.Wait(source.Token);
                }
                catch (Exception e)
                {
                    if (source != null)
                        source.Cancel();
                    Console.WriteLine(e);
                }
            }
            finally
            {
                if (server != null)
                    server.Stop();
            }
        }

        private static string ComputeHashBase64String(string skey)
        {
            byte[] key;
            key = Encoding.UTF8.GetBytes((skey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"));
            Console.WriteLine();
            Console.WriteLine("Computing hash on \"{0}\"", skey);
            Console.WriteLine();
            var provider = new SHA1CryptoServiceProvider();
            byte[] hash = provider.ComputeHash(key);
            string base64String = Convert.ToBase64String(hash);
            return base64String;
        }

        private static void Action(CancellationToken token)
        {
        }

        //GET / HTTP/1.1
        //Upgrade: websocket
        //Connection: Upgrade
        //Host: 127.0.0.1:555
        //Origin: http://localhost:63342
        //Pragma: no-cache
        //Cache-Control: no-cache
        //Sec-WebSocket-Key: QR029sn6m5bNzeognzXoJg==
        //Sec-WebSocket-Version: 13
        //Sec-WebSocket-Extensions: permessage-deflat
        //eflate-frame
        //User-Agent: Mozilla/5.0 (Windows NT 6.1; WO
        //Gecko) Chrome/37.0.2062.103 Safari/537.36
    }
}