namespace Lab3.Application.DTOs;

public class UserDTO
{
    public string Name { get; set; } = null!;

    public long RoleId { get; set; }
    
    public string Email { get; set; } = null!;
    public string KeycloakId { get; set; }

    public string Password { get; set; }

    public long BranchTenantId { get; set; }
    
}