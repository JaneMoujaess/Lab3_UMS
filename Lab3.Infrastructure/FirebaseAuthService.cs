using AutoMapper;
using Firebase.Auth;
using Lab3.Application.DTOs;
using Lab3.Persistence;
using Microsoft.Extensions.Logging;
using FirebaseAuth = FirebaseAdmin.Auth.FirebaseAuth;
using User = Lab3.Domain.Models.User;

namespace Lab3.Infrastructure;

public class FirebaseAuthService : IFirebaseAuthService
{
    //private static readonly string WebApiKey = Environment.GetEnvironmentVariable("FIREBASE_API_KEY");
    private readonly ILogger<FirebaseAuthService> _logger;
    private const string WebApiKey = "AIzaSyB-3k-P8GazZPxVjMMGFEGjJ2tNqU6jITM";
    private readonly UmsDbContext _dbContext;
    private readonly IMapper _mapper;
    
    private readonly FirebaseAuthProvider _auth;

    public FirebaseAuthService(ILogger<FirebaseAuthService> logger,UmsDbContext dbContext)
    {
        _logger = logger;
        //_logger.LogInformation(WebApiKey);
        _auth = new FirebaseAuthProvider(new FirebaseConfig(WebApiKey));
        _dbContext = dbContext;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        });
        _mapper = new Mapper(config);
    }

    public async Task<string> SignIn(string email, string password)
    {
        //log in an existing user
        var fbAuthLink = await _auth
            .SignInWithEmailAndPasswordAsync(email, password);
        var token = fbAuthLink.FirebaseToken;
        return token;
    }

    public async Task<string> SignUp(UserDto userDto)
    {
        var fbUser = await _auth.CreateUserWithEmailAndPasswordAsync(userDto.Email, userDto.Password);
        
        
        var newUser = _mapper.Map<User>(userDto);
        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("New user has id of "+newUser.Id);
        Dictionary<string, object> customClaims = new Dictionary<string, object>
        {
            {"roleId", newUser.RoleId},
            {"branchTenantId",newUser.BranchTenantId},
            {"userId",newUser.Id}
        };
        
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        
        FirebaseAdmin.Auth.UserRecord userRecord = await auth.GetUserByEmailAsync(userDto.Email);
        string uid = userRecord.Uid;
        await auth.SetCustomUserClaimsAsync(uid, customClaims);

        var fbLink=await _auth
            .SignInWithEmailAndPasswordAsync(userDto.Email, userDto.Password);
        return fbLink.FirebaseToken;
    }
    /*public async Task<string> SignUp(string email,string password,string role,int branchTenantId)
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
    }*/
}