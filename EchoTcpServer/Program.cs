using System;
using System.Threading.Tasks;
using EchoTcpServerApp.Server;
using EchoTcpServerApp.Client;

namespace EchoTcpServerApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            EchoServer server = new EchoServer(5000);
        
            _ = Task.Run(() => server.StartAsync());
            string host = "127.0.0.1";
            int port = 60000;         
            int intervalMilliseconds = 5000; 

            using (var sender = new UdpTimedSender(host, port))
            {
                Console.WriteLine("Press 'q' to quit...");
                sender.StartSending(intervalMilliseconds);

                while (Console.ReadKey(intercept: true).Key != ConsoleKey.Q)
                {
                }
                sender.StopSending();
                server.Stop();
                Console.WriteLine("Sender stopped.");
            }
        }
    }
}
