using Lab3.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lab3.Application.Services.UserIdentifierService;

public interface IUserIdentifierService
{
    long GetUserId();
    Task<long> GetRoleId();
    Task<long> GetTenantId();
}
public class UserIdentifierService:IUserIdentifierService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UmsDbContext _dbContext;
    private readonly ILogger<UserIdentifierService> _logger;
    private long UserId { set;get; }
 
    public UserIdentifierService(IHttpContextAccessor httpContextAccessor,ILogger<UserIdentifierService> logger,UmsDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _dbContext = dbContext;
        UserId = long.Parse(_httpContextAccessor.HttpContext.Items["userId"].ToString());
    }

    public long GetUserId()
    {
        return UserId;
    }
 
    public async Task<long> GetTenantId()
    {

        _logger.LogInformation("UserId=" + UserId);
        var tenantId = await _dbContext.Users.Where(user => user.Id == UserId)
            .Select(user => user.BranchTenantId)
            .SingleAsync();
        _logger.LogInformation("TenantId=" + tenantId);
        return tenantId;
    }

    public async Task<long> GetRoleId()
    {
        var roleId=await _dbContext.Users.Where(user => user.Id == UserId)
            .Select(user => user.RoleId)
            .SingleAsync();
        return roleId;
    }
}