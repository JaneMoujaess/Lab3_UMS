using Firebase.Auth;
using Microsoft.Extensions.Logging;
using FirebaseAuth = FirebaseAdmin.Auth.FirebaseAuth;
namespace Lab3.Infrastructure;

public class FirebaseAuthService : IFirebaseAuthService
{
    //private static readonly string WebApiKey = Environment.GetEnvironmentVariable("FIREBASE_API_KEY");
    private readonly ILogger<FirebaseAuthService> _logger;
    private const string WebApiKey = "AIzaSyB-3k-P8GazZPxVjMMGFEGjJ2tNqU6jITM";
    
    private readonly FirebaseAuthProvider _auth;

    public FirebaseAuthService(ILogger<FirebaseAuthService> logger)
    {
        _logger = logger;
        //_logger.LogInformation(WebApiKey);
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

    public async Task<string> SignUp(string email,string password,string role,int branchTenantId)
    {
        var user = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
        
        Dictionary<string, object> customClaims = new Dictionary<string, object>
        {
            {"role", role},
            {"branchTenantId",branchTenantId}
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