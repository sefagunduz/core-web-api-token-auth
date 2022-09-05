# .NET Core Web API - Token Based Authentication

## dependencies

PM> Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
PM> Install-Package Microsoft.IdentityModel.Tokens
PM> Install-Package System.IdentityModel.Tokens.Jwt

## step by step

add appsettings json

  "JWT": {
    "Key": "MySuperSecretKey@345",
    "Issuer": "https://localhost:7069/",
    "Audience": "https://localhost:7069/"
  }

0- add Program.cs
    app.UseAuthentication();  
    
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key)
    };
});


1- create User model and Fake Database class (or your Database and user table classs)
2- create loginModel model class
2- create ITokenService interface
3- create TokenService class
4- Program.cs add builder.Services.AddSingleton<ITokenService,TokenService>();
5- create AuthenticatedResponse class
6- create AuthController

## using

