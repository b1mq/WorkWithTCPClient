using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpProjekt
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello,this is Tcp Projekt created by Egor Tagirov");
            await StartServer();
        }
        public static async Task StartServer(int port = 8888)
        {
            IPAddress adress = IPAddress.Any;


            TcpListener server = new TcpListener(adress, port);
            try
            {
                server.Start();
                Console.WriteLine($"Tcp server is started on port {port} with ipAddress {adress}");
                while(true )
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    Console.WriteLine("New client is connected");
                    _ = Task.Run(() => HandleCientAsync(client));
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            finally
            {
                Console.WriteLine("Server is off");
                server.Stop();
            }
        }
        public static async Task HandleCientAsync(TcpClient client)
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                try
                {
                   
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        
                        string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Received from client: {text}");

                        
                        string response = text.ToUpper();
                        byte[] data = Encoding.UTF8.GetBytes(response);
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client error: {ex.Message}");
                }
            }

            Console.WriteLine("Client disconnected");
        
        }
    }
}
