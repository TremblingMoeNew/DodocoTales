using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace DodocoTales.Loader
{
    public class DDCGAuthkeyCaptureProxyServer
    {
        public ProxyServer PServer { get; set; }
        private readonly ExplicitProxyEndPoint EndPoint;
        public event EventHandler<string> OnAuthkeyCaptured;
        public DDCGAuthkeyCaptureProxyServer()
        {
            PServer = new ProxyServer();
            PServer.CertificateManager.EnsureRootCertificate();
            PServer.BeforeRequest += BeforeRequest;
            EndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 12199);
        }

        public void StartProxy()
        {
            PServer.AddEndPoint(EndPoint);
            PServer.Start();
            PServer.SetAsSystemHttpProxy(EndPoint);
            PServer.SetAsSystemHttpsProxy(EndPoint);
        }
        public void EndProxy()
        {
            if (PServer.ProxyRunning)
            {
                PServer.Stop();
            }
            PServer.DisableAllSystemProxies();
        }

        private Task BeforeRequest(object sender, SessionEventArgs e)
        {
            var request = e.HttpClient.Request;

            if ((request.Host == "webstatic.mihoyo.com" || request.Host == "webstatic-sea.mihoyo.com") && request.RequestUri.AbsolutePath == "/hk4e/event/e20190909gacha-v2/index.html")
            {
                string pattern = @".+\?(.+)";
                var result = Regex.Matches(request.Url, pattern);
                Regex regex = new Regex(@"lang=.+&device_type");
                var authkey = regex.Replace(result[result.Count - 1].Groups[1].Value, "lang=en&device_type");
                OnAuthkeyCaptured(this, authkey);
            }
            return Task.CompletedTask;
        }
    }
}
