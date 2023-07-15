using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.Serialization;
using CoreWCF;
using CoreWCF.OpenApi.Attributes;
using CoreWCF.Web;

namespace CoreWCFServer.Services;

[DataContract]
public class EchoFault
{
    [AllowNull] private string _text;

    [DataMember]
    [AllowNull]
    public string Text
    {
        get { return _text; }
        set { _text = value; }
    }
}

[ServiceContract]
[OpenApiBasePath("/api")]
public interface IEchoService
{
    [OperationContract]
    [WebGet(UriTemplate = "/hello/{text}")]
    [OpenApiResponse(ContentTypes = new[] { "application/json"}, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(string))]
    string Echo(string text);

    [OperationContract]
    [WebInvoke(Method = "POST", UriTemplate = "/echo")]
    [OpenApiResponse(ContentTypes = new[] { "application/json"}, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(EchoMessage))]
    string ComplexEcho(
        [OpenApiParameter(ContentTypes = new[] { "application/json"}, Description = "Text")] EchoMessage text);

    [OperationContract]
    [FaultContract(typeof(EchoFault))]
    string FailEcho(string text);
}

[DataContract(Name = "EchoMessage")]
public class EchoMessage
{
    [AllowNull] [DataMember][OpenApiProperty(Description = "Text")] public string Text { get; set; }
}