using CoreWCF;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;
using CoreWCFServer.Services;

const string HOST_IN_WSDL = "localhost";
const int HTTP_PORT = 8088;
const int HTTPS_PORT = 8443;
const int NETTCP_PORT = 8089;
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseNetTcp(NETTCP_PORT);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(HTTP_PORT);
    options.ListenAnyIP(HTTPS_PORT, listenOptions => { listenOptions.UseHttps(); });
});

// Add WSDL support
builder.Services.AddServiceModelServices().AddServiceModelMetadata();
// Use the scheme/host/port used to fetch WSDL as that service endpoint address in generated WSDL 
builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

var app = builder.Build();

app.UseServiceModel(builder =>
{
    builder.AddService<EchoService>((serviceOptions) =>
        {
            // Set the default host name:port in generated WSDL and the base path for the address 
            serviceOptions.BaseAddresses.Add(new Uri($"http://{HOST_IN_WSDL}/EchoService"));
            serviceOptions.BaseAddresses.Add(new Uri($"https://{HOST_IN_WSDL}/EchoService"));
        })
        .AddServiceEndpoint<EchoService, IEchoService>(new BasicHttpBinding(), "/basicHttp")
        .AddServiceEndpoint<EchoService, IEchoService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/basichttp")
        // Add WSHttpBinding endpoints
        .AddServiceEndpoint<EchoService, IEchoService>(new WSHttpBinding(SecurityMode.None), "/wsHttp")

        // Add NetTcpBinding
        .AddServiceEndpoint<EchoService,
            IEchoService>(new NetTcpBinding(), $"net.tcp://localhost:{NETTCP_PORT}/EchoService/netTcp");
});

// Enable WSDL for http & https
var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
serviceMetadataBehavior.HttpGetEnabled = serviceMetadataBehavior.HttpsGetEnabled = true;


app.Run();