using Firebase.Auth;
using FirebaseAdmin.Auth;
using FirebaseAuth = FirebaseAdmin.Auth.FirebaseAuth;
namespace Lab3.Infrastructure;

public class FirebaseAuthService : IFirebaseAuthService
{
    private const string WebApiKey = "AIzaSyB-3k-P8GazZPxVjMMGFEGjJ2tNqU6jITM";

    private readonly FirebaseAuthProvider _auth;

    public FirebaseAuthService()
    {
        _auth = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
    }

    public async Task<string> SignIn(string email, string password)
    {
        //log in an existing user
        var fbAuthLink = await _auth
            .SignInWithEmailAndPasswordAsync(email, password);
        var token = fbAuthLink.FirebaseToken;
        return token;
    }

    public async Task<string> SignUp(string email,string password)
    {
        var user = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
        
        Dictionary<string, object> customClaims = new Dictionary<string, object>
        {
            {"role", "teacher"}
        };
        
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        
        FirebaseAdmin.Auth.UserRecord userRecord = await auth.GetUserByEmailAsync(email);
        string uid = userRecord.Uid;
        await auth.SetCustomUserClaimsAsync(uid, customClaims);

        var fbLink=await _auth
            .SignInWithEmailAndPasswordAsync(email, password);
        return fbLink.FirebaseToken;
    }
}