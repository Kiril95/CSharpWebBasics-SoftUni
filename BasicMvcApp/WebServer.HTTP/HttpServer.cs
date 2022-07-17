using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using WebServer.HTTP.Enumerators;
using System.Net;

namespace WebServer.HTTP
{
    public class HttpServer
    {
        List<Route> routeTable;

        public HttpServer(List<Route> routeTable)
        {
            this.routeTable = routeTable;
        }

        public async Task StartAsync(int port)
        {
            // Loopback is localhost
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                await ProcessClientAsync(tcpClient);
            }
        }

        private async Task ProcessClientAsync(TcpClient tcpClient)
        {
            try
            {
                using (NetworkStream stream = tcpClient.GetStream())
                {
                    List<byte> data = new List<byte>();
                    int position = 0;
                    byte[] buffer = new byte[HttpConstants.BufferSize];

                    while (true)
                    {
                        int count = await stream.ReadAsync(buffer, position, buffer.Length);
                        position += count;

                        if (count < buffer.Length)
                        {
                            var partialBuffer = new byte[count];
                            Array.Copy(buffer, partialBuffer, count);
                            data.AddRange(partialBuffer);
                            break;
                        }
                        else
                        {
                            data.AddRange(buffer);
                        }
                    }

                    string requestAsString = Encoding.UTF8.GetString(data.ToArray());
                    HttpRequest request = new HttpRequest(requestAsString);
                    Console.WriteLine($"{request.Method} {request.Path} => {request.Headers.Count} headers");

                    HttpResponse response;
                    Route route = this.routeTable.FirstOrDefault(
                        x => string.Compare(x.Path, request.Path, true) == 0 && x.Method == request.Method);

                    if (route != null)
                    {
                        response = route.Action(request);
                    }
                    else
                    {
                        // Not Found 404
                        response = new HttpResponse("text/html", new byte[0], Enumerators.HttpStatusCode.NotFound);
                    }

                    response.Headers.Add(new Header("Server", "My Server 1.0"));

                    Cookie sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == "Cookie");
                    if (sessionCookie != null)
                    {
                        ResponseCookie responseSessionCookie = new ResponseCookie(sessionCookie.Name, sessionCookie.Value);
                        responseSessionCookie.Path = "/";
                        response.Cookies.Add(responseSessionCookie);
                    }

                    byte[] responseHeaderBytes = Encoding.UTF8.GetBytes(response.ToString());

                    await stream.WriteAsync(responseHeaderBytes, 0, responseHeaderBytes.Length);

                    if (response.Body != null)
                    {
                        await stream.WriteAsync(response.Body, 0, response.Body.Length);
                    }
                }

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
