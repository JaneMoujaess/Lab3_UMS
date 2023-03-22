using Firebase.Auth;

namespace Lab3.Infrastructure;

public class FirebaseAuthService:IFirebaseAuthService
{
    private const string WebApiKey="AIzaSyB-3k-P8GazZPxVjMMGFEGjJ2tNqU6jITM";

    private readonly FirebaseAuthProvider _auth;
    public FirebaseAuthService()
    {
        _auth = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
    }
    
    public async Task<string> SignIn(string email,string password)
    {
        //log in an existing user
        var fbAuthLink = await _auth
            .SignInWithEmailAndPasswordAsync(email,password);
        string token = fbAuthLink.FirebaseToken;
        return token;
    }
  
    public async Task<string> SignUp(string email,string password)
    {
        //create the user
        await _auth.CreateUserWithEmailAndPasswordAsync(email,password);
        //log in the new user
        var fbAuthLink = await _auth
            .SignInWithEmailAndPasswordAsync(email,password);
        string token = fbAuthLink.FirebaseToken;
        return token;
    }
}