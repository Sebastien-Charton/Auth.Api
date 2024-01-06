using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Auth.Api.Infrastructure.Extensions;

public static class EnvironmentExtension
{
    public static bool IsContainer(this IWebHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment("Container");
    }
    
    public static bool IsLocal(this IWebHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment("Local");
    }
}
