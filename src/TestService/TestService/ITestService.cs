using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace TestService
{
    [ServiceContract]
    interface ITestService
    {
        [OperationContract]
        [WebInvoke(Method = "POST"
                            , RequestFormat = WebMessageFormat.Json
                            , UriTemplate = "/PostMsg"
            )]
        MessageData PostMsg(MessageData msg);
    }

    [DataContract]
    public class MessageData
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Gender { get; set; }

        [DataMember]
        public int Age { get; set; }

    }

}
