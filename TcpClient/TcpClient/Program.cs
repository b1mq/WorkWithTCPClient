using System.Net.Sockets;
using System.Text;

namespace TcpClient1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string host = "127.0.0.1"; 
            int port = 8888;
            using (TcpClient client = new TcpClient() )
            {
                await client.ConnectAsync(host, port);
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    while(true)
                    {
                        Console.WriteLine("Message:");
                        string text = Console.ReadLine();
                        if (string.IsNullOrEmpty(text)) break;
                        byte[] data = Encoding.UTF8.GetBytes(text);
                        await stream.WriteAsync(data, 0, data.Length);
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string response = Encoding.UTF8.GetString(buffer,0,bytesRead);
                        Console.WriteLine($"Response from server {response}");

                    }
                }
            }
        }
    }
}
