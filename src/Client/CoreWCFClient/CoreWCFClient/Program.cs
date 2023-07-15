// See https://aka.ms/new-console-template for more information

using System.ServiceModel;
using CoreWCFServer.Services;

const int HTTP_PORT = 8088;
const int HTTPS_PORT = 8443;
const int NETTCP_PORT = 8089;


Console.Title = "WCF .Net Core Client";

CallBasicHttpBinding($"http://localhost:{HTTP_PORT}");
CallBasicHttpBinding($"https://localhost:{HTTPS_PORT}");
CallWsHttpBinding($"http://localhost:{HTTP_PORT}");
CallNetTcpBinding($"net.tcp://localhost:{NETTCP_PORT}");


static void CallBasicHttpBinding(string hostAddr)
{
    IClientChannel channel = null;

    var binding =
        new BasicHttpBinding(IsHttps(hostAddr) ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);

    var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/EchoService/basicHttp"));
    factory.Open();
    try
    {
        IEchoService client = factory.CreateChannel();
        channel = client as IClientChannel;
        channel.Open();
        var result = client.Echo("Hello World!");
        channel.Close();
        Console.WriteLine(result);
    }
    finally
    {
        factory.Close();
    }
}

static void CallWsHttpBinding(string hostAddr)
{
    IClientChannel channel = null;

    var binding = new WSHttpBinding(IsHttps(hostAddr) ? SecurityMode.Transport : SecurityMode.None);

    var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/EchoService/wsHttp"));
    factory.Open();
    try
    {
        IEchoService client = factory.CreateChannel();
        channel = client as IClientChannel;
        channel.Open();
        var result = client.Echo("Hello World!");
        channel.Close();
        Console.WriteLine(result);
    }
    finally
    {
        factory.Close();
    }
}

static void CallNetTcpBinding(string hostAddr)
{
    IClientChannel channel = null;

    var binding = new NetTcpBinding();

    var factory = new ChannelFactory<IEchoService>(binding, new EndpointAddress($"{hostAddr}/EchoService/netTcp"));
    factory.Open();
    try
    {
        IEchoService client = factory.CreateChannel();
        channel = client as IClientChannel;
        channel.Open();
        var result = client.Echo("Hello World!");
        channel.Close();
        Console.WriteLine(result);
    }
    finally
    {
        factory.Close();
    }
}

static bool IsHttps(string url)
{
    return url.ToLower().StartsWith("https://");
}