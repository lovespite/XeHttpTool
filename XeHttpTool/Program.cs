
var httpClientHandler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
    AutomaticDecompression = System.Net.DecompressionMethods.All,
    UseCookies = false, // We handle cookies manually
};

var httpClient = new HttpClient(httpClientHandler)
{
    Timeout = TimeSpan.FromSeconds(100),
};

using var window = new XeHttpTool.UI.MainWindow(httpClient);

window.Run();