using System;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.ServiceModel.Web;
using TestService;

namespace TestClient
{
    public partial class Form1 : Form
    {
        WebChannelFactory<ITestService> cf = null;
        ITestService channel = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.HorizontalScrollbar = true;

            Uri uri = new Uri("https://126.108.62.68:5000/TestService");
            EndpointAddress endpointAddress = new EndpointAddress(uri);

            //--------------------------チャンネル
            cf = new WebChannelFactory<ITestService>(uri);
            WebHttpBinding binding = cf.Endpoint.Binding as WebHttpBinding;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            binding.Security.Mode = WebHttpSecurityMode.Transport;

            var behavior = new WebHttpBehavior();
            behavior.FaultExceptionEnabled = false;
            behavior.HelpEnabled = true;
            behavior.DefaultOutgoingRequestFormat = WebMessageFormat.Json;
            behavior.DefaultOutgoingResponseFormat = WebMessageFormat.Json;
            cf.Endpoint.Behaviors.Add(behavior);

            var clientCertificate = new X509Certificate2(@"D:\Work\TestService\ServerCert1.pfx", "pswd", X509KeyStorageFlags.UserKeySet);
            cf.Credentials.ClientCertificate.Certificate = clientCertificate;
            cf.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;

            channel = cf.CreateChannel();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageData msg = new MessageData()
            {
                Name = "Taro",
                Gender = 1,
                Age = 3
            };
            MessageData rtn = channel.PostMsg(msg);
            listBox1.Items.Insert(0, string.Format("Name:{0}, Gender:{1}, Age{2} ", rtn.Name, rtn.Gender, rtn.Age));

        }
    }
}
