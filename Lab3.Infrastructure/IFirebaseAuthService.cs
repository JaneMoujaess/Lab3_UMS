using Firebase.Auth;
using Lab3.Application.DTOs;

namespace Lab3.Infrastructure;

public interface IFirebaseAuthService
{
    public Task<string> SignIn(string email, string password);
    //public Task<string> SignUp(string email,string password,string role,int branchTenantId);
    public Task<string> SignUp(UserDTO userDto);

}