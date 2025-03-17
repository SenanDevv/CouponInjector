using CouponInjector;

Console.WriteLine("Starting Amazon JS Injector...");

var proxyManager = new ProxyManager();

try
{
    await proxyManager.StartProxy();

    Console.WriteLine("Proxy server started on port 8000");
    Console.WriteLine("Press any key to stop...");
    Console.ReadKey();

    await proxyManager.StopProxy();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    await proxyManager.LogStatus($"error: {ex.Message}");
}