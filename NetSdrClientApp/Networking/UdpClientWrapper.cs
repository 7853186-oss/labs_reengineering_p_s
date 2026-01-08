using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
namespace NetSdrClientApp.Networking
{
    public class UdpClientWrapper : BaseClientWrapper, IUdpClient
    {
        private readonly IPEndPoint _localEndPoint;
        private UdpClient? _udpClient;

        public event EventHandler<byte[]>? MessageReceived;

        public UdpClientWrapper(int port)
        {
            _localEndPoint = new IPEndPoint(IPAddress.Any, port);
        }

        public async Task StartListeningAsync()
        {
            ResetCancellationToken();
            Console.WriteLine("Start listening for UDP messages...");

            try
            {
                _udpClient = new UdpClient(_localEndPoint);
                while (!_cts.Token.IsCancellationRequested)
                {
                    UdpReceiveResult result = await _udpClient.ReceiveAsync(_cts.Token);
                    MessageReceived?.Invoke(this, result.Buffer);

                    Console.WriteLine($"Received from {result.RemoteEndPoint}");
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving message: {ex.Message}");
            }
        }

        public void StopListening()
        {
            try
            {
                StopCancellationToken();
                _udpClient?.Close();
                Console.WriteLine("Stopped listening for UDP messages.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while stopping: {ex.Message}");
            }
        }

        public void Exit()
        {
            StopListening();
        }

        
        public override int GetHashCode()
        {
            return HashCode.Combine(_localEndPoint.Address, _localEndPoint.Port);
        }

        public override bool Equals(object? obj)
        {
            if (obj is UdpClientWrapper other)
            {
                return _localEndPoint.Address.Equals(other._localEndPoint.Address) &&
                       _localEndPoint.Port == other._localEndPoint.Port;
            }
            return false;
        }
    }
}
