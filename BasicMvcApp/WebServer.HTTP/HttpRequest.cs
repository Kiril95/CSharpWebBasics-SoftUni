using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.HTTP.Enumerators;

namespace WebServer.HTTP
{
    public class HttpRequest
    {
        public static IDictionary<string, Dictionary<string, string>>
            Sessions = new Dictionary<string, Dictionary<string, string>>();

        public HttpRequest(string requestString)
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();

            string[] lines = requestString.Split(new string[] { HttpConstants.NewLine }, StringSplitOptions.None);

            string headerLine = lines[0];
            string[] headerLineParts = headerLine.Split(' ');

            this.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerLineParts[0], true);
            this.Path = headerLineParts[1];

            int lineIndex = 1;
            bool isInHeaders = true;
            StringBuilder bodyBuilder = new StringBuilder();

            while (lineIndex < lines.Length)
            {
                var line = lines[lineIndex];
                lineIndex++;

                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeaders = false;
                    continue;
                }

                if (isInHeaders)
                {
                    this.Headers.Add(new Header(line));
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }

            if (this.Headers.Any(x => x.Name == "Cookie"))
            {
                string cookiesAsString = this.Headers.FirstOrDefault(x => x.Name == "Cookie").Value;

                string[] cookies = cookiesAsString.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var cookieAsString in cookies)
                {
                    this.Cookies.Add(new Cookie(cookieAsString));
                }
            }

            var sessionCookie = this.Cookies.FirstOrDefault(x => x.Name == "Cookie");
            if (sessionCookie == null)
            {
                string sessionId = Guid.NewGuid().ToString();
                this.Session = new Dictionary<string, string>();

                Sessions.Add(sessionId, this.Session);
                this.Cookies.Add(new Cookie("Cookie", sessionId));
            }
            else if (!Sessions.ContainsKey(sessionCookie.Value))
            {
                this.Session = new Dictionary<string, string>();
                Sessions.Add(sessionCookie.Value, this.Session);
            }
            else
            {
                this.Session = Sessions[sessionCookie.Value];
            }

        }

        public string Path { get; set; }

        public HttpMethod Method { get; set; }

        public ICollection<Header> Headers { get; set; }

        public ICollection<Cookie> Cookies { get; set; }

        public Dictionary<string, string> Session { get; set; }
    }
}
