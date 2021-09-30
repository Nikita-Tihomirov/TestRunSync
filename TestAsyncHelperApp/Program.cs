using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TestWebApp;

namespace TestAsyncHelperApp
{
	class Program
	{
		static void Main(string[] args)
        {
            //ThreadPool.SetMaxThreads(1000,1000);

            var hostBuilder = new WebHostBuilder()
			.UseKestrel()
			.UseStartup<Startup>()
			.Build();

			hostBuilder.RunAsync();

			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			HttpClient client = new HttpClient(clientHandler);

			MediaTypeHeaderValue mediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");
            var tasks = new List<Task>();
            for (int i = 0; i < 700; i++)
            {
                int x = i;
                var t = Task.Run(async () =>
                {
                    var content = new StringContent("\"" + Guid.NewGuid().ToString() + "\"");
                    content.Headers.ContentType = mediaTypeHeaderValue;
                    await client.PostAsync("http://localhost:5000/api/values", content);
                    Console.WriteLine(x);
                });
                tasks.Add(t);
            }
            Task.WhenAll(tasks).Wait();
        }
	}
}
