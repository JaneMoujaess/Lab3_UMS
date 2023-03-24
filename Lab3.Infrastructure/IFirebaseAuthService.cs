using Firebase.Auth;

namespace Lab3.Infrastructure;

public interface IFirebaseAuthService
{
    public Task<string> SignIn(string email, string password);
    public Task<string> SignUp(string email,string password);
}