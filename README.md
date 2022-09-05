# .NET Core Web API - Token Based Authentication

## dependencies

PM> Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
PM> Install-Package Microsoft.IdentityModel.Tokens
PM> Install-Package System.IdentityModel.Tokens.Jwt

## step by step

add appsettings json

```json
{
  "JWT": {
    "Key": "MySuperSecretKey@345",
    "Issuer": "https://localhost:7069/",
    "Audience": "https://localhost:7069/"
  }
}
```

add Program.cs

```cs
/* First UseAuthentication, Second UseAuthorization */
app.UseAuthentication();
app.UseAuthorization();
    
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});
```

1. create User model and Fake Database class (or your Database and user table classs)
2. create loginModel model class
2. create ITokenService interface
3. create TokenService class
4. Program.cs add builder.Services.AddSingleton<ITokenService,TokenService>();
5. create AuthenticatedResponse class
6. create AuthController

## using

1. Use login endpoint with username and password while the user is logging in.
2. Keep the refresh token and access token you received in your memory.
3. use refresh token to get new token.
4. Use services that require authentication with an access token.
5. Use logout endpoint to delete refresh token.
6. When the refresh token expires, you must login again.