using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.ServiceModel.Web;

namespace TestService
{
    class Program
    {
        static WebServiceHost host;

        static void Main()
        {
            //--------------------------バインディング
            WebHttpBinding binding = new WebHttpBinding();
            binding.Security.Mode = WebHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;

            //--------------------------アドレス
            Uri uri = new Uri("https://localhost:5000/TestService");

            //--------------------------サービスの作成とエンドポイントの追加
            host = new WebServiceHost(typeof(TestService));
            ServiceEndpoint se = host.AddServiceEndpoint(typeof(ITestService), binding, uri);

            var behavior = new WebHttpBehavior();
            behavior.FaultExceptionEnabled = false;
            behavior.HelpEnabled = true;
            behavior.DefaultOutgoingRequestFormat = WebMessageFormat.Json;
            behavior.DefaultOutgoingResponseFormat = WebMessageFormat.Json;
            se.EndpointBehaviors.Add(behavior);

            //--------------------------
            ServiceDebugBehavior debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            debug.IncludeExceptionDetailInFaults = true;

            ServiceMetadataBehavior metad = new ServiceMetadataBehavior();
            metad.HttpGetEnabled = true;
            metad.HttpsGetEnabled = true;
            host.Description.Behaviors.Add(metad);

            //--------------------------
            var certificate = new X509Certificate2(@"D:\Work\TestService\ServerCert1.pfx", "pswd", X509KeyStorageFlags.UserKeySet);
            host.Credentials.ServiceCertificate.Certificate = certificate;
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

            //--------------------------
            host.Open();
            Console.WriteLine(string.Format(null, "URL : {0}", uri.ToString()));
            Console.WriteLine("Press <ENTER> to terminate");
            Console.ReadLine();
            host.Close();

        }
    }
}
