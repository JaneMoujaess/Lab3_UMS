using Microsoft.AspNetCore.Http;

namespace Lab3.Application.Services.TenantProviderService;

public class TenantProviderService:ITenantProviderService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
 
    public TenantProviderService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
 
    public long GetTenantId()
    {
        return long.Parse(_httpContextAccessor.HttpContext.Items["TenantId"].ToString());
    }
}