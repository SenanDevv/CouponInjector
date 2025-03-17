using System.Net;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace CouponInjector
{
    public class ProxyManager
    {
        private ProxyServer _proxyServer;
        private ExplicitProxyEndPoint _explicitEndPoint;
        private readonly string _logFilePath;
        private readonly IJavaScriptInjector _jsInjector;

        public ProxyManager()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "proxy_status.txt");
            _jsInjector = new AmazonJavaScriptInjector();

            Console.CancelKeyPress += async (s, e) => await StopProxy();
            AppDomain.CurrentDomain.ProcessExit += async (s, e) => await StopProxy();
            AppDomain.CurrentDomain.UnhandledException += async (s, e) => await StopProxy();
        }

        public async Task StartProxy()
        {
            await LogStatus("active");

            _proxyServer = new ProxyServer();

            _explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8000, true);

            _proxyServer.AddEndPoint(_explicitEndPoint);

            _proxyServer.BeforeResponse += OnResponse;

            _proxyServer.Start();

            _proxyServer.SetAsSystemProxy(_explicitEndPoint, ProxyProtocolType.AllHttp);
        }

        public async Task StopProxy()
        {
            try
            {
                if (_proxyServer != null && _proxyServer.ProxyRunning)
                {
                    if (_explicitEndPoint != null)
                    {
                        _proxyServer.DisableAllSystemProxies();
                        _proxyServer.RestoreOriginalProxySettings();
                    }

                    _proxyServer.Stop();

                    Console.WriteLine("Proxy stopped and system settings restored.");
                }

                await LogStatus("inactive");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during cleanup: {ex.Message}");
                await LogStatus($"cleanup_error: {ex.Message}");
            }
        }

        public async Task LogStatus(string status)
        {
            await File.WriteAllTextAsync(_logFilePath, status);
        }

        private async Task OnResponse(object sender, SessionEventArgs e)
        {
            if (e.HttpClient.Request.Url.Contains("amazon") &&
                e.HttpClient.Response.StatusCode == 200 &&
                e.HttpClient.Response.ContentType?.Contains("text/html") == true)
            {
                string body = await e.GetResponseBodyAsString();

                string modifiedBody = _jsInjector.InjectJavaScript(body);

                e.SetResponseBodyString(modifiedBody);

                Console.WriteLine("JavaScript injected into Amazon.com response");
            }
        }
    }
}