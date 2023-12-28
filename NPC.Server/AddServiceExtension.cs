using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using System.Text;
using NPC.Server.Models;

//using webapi.Manager;
//using webapi.Manager.IManager;
//using webapi.Models;
//using webapi.Services;
//using webapi.Services.IServices;
using static System.Net.Mime.MediaTypeNames;

namespace NPC.Server
{
    //adding dependent services
    public static class AddServiceExtension
    {
        public static IServiceCollection AddMyScoped(this IServiceCollection serviceCollection)
        {
            //serviceCollection.AddScoped<IAuthService, AuthService>();
            //serviceCollection.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            //serviceCollection.AddScoped<IImageService, ImageService>();
            //serviceCollection.AddScoped<ICategoryService, CategoryService>();
            //serviceCollection.AddScoped<ILeadBoardService, LeadBoardService>();
            //serviceCollection.AddScoped<IGameService, GameService>();
            //serviceCollection.AddScoped<IGamePointsService, GamePointsService>();
            //serviceCollection.AddScoped<IBallService, BallService>();
            //serviceCollection.AddScoped<IBallManager, BallManager>();
            //serviceCollection.AddScoped<IUserService, UserService>();
            //serviceCollection.AddScoped<ILookupService, LookupService>();

            return serviceCollection;
        }
    }


    //adding authorization box to swagger
    public static class AddSwaggerAuthorizationExtension
    {
        public static IServiceCollection AddSwaggerAuthorization(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Description = "Enter the bearer Authorization string  as following: 'Bearer Generated-jwt-token'",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                Type = ReferenceType.SecurityScheme,
                Id =JwtBearerDefaults.AuthenticationScheme
                }
            },new string[]{ }

        }

    });
            });
            return serviceCollection;
        }
    }

    //adding token validator from headers
    public static class AddtokenValidatorExtension
    {
        public static IServiceCollection AddtokenValidator(this IServiceCollection serviceCollection, WebApplicationBuilder builder)
        {
            var appsettingSection = builder.Configuration.GetSection("AppSettings:JwtOptions");
            string? secret = appsettingSection.GetValue<string>("Secret");
            string? issuer = appsettingSection.GetValue<string>("Issuer");
            string? audience = appsettingSection.GetValue<string>("Audience");
            byte[] key = Encoding.ASCII.GetBytes(secret);
            serviceCollection.AddAuthentication(x =>
            {

                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateAudience = true
                };
            });
            return serviceCollection;
        }
    }

    //adding cors
    public static class AddCustomCorsExtension
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection serviceCollection, string MyAllowSpecificOrigins)
        {
            serviceCollection.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("*")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                  });
            });
            return serviceCollection;
        }
    }



    //EncryptionMiddleware 

    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly EncryptionOptions _encryptionOptions;
        public EncryptionMiddleware(RequestDelegate next, IOptions<EncryptionOptions> encryptionOptions)
        {
            _next = next;
            _encryptionOptions = encryptionOptions.Value;
        }
        // Whenever we call any action method then call this before call the action method
        public async Task Invoke(HttpContext httpContext)
        {
            List<string> includedURL = GetIncludedURLList();
            if (includedURL.Contains(httpContext.Request.Path.Value))
            {
                httpContext.Request.Body = DecryptStream(httpContext.Request.Body);
                if (httpContext.Request.QueryString.HasValue)
                {
                    string decryptedString = DecryptString(httpContext.Request.QueryString.Value.Substring(1));
                    httpContext.Request.QueryString = new QueryString($"?{decryptedString}");
                }
            }
            await _next(httpContext);
        }
        // This function is not needed but if we want anything to encrypt then we can use
        public CryptoStream EncryptStream(Stream responseStream)
        {
            Aes aes = GetEncryptionAlgorithm();
            ToBase64Transform base64Transform = new ToBase64Transform();
            CryptoStream base64EncodedStream = new CryptoStream(responseStream, base64Transform, CryptoStreamMode.Write);
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(base64EncodedStream, encryptor, CryptoStreamMode.Write);
            return cryptoStream;
        }
        public string EncryptString(string plainText)
        {
            byte[] encrypted;
           
            using (Aes aes = GetEncryptionAlgorithm())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);             
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }         

            return Convert.ToBase64String(encrypted);
        }
        // This are main functions that we decrypt the payload and  parameter which we pass from the angular service.
        public Stream DecryptStream(Stream cipherStream)
        {
            Aes aes = GetEncryptionAlgorithm();
            FromBase64Transform base64Transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces);
            CryptoStream base64DecodedStream = new CryptoStream(cipherStream, base64Transform, CryptoStreamMode.Read);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream decryptedStream = new CryptoStream(base64DecodedStream, decryptor, CryptoStreamMode.Read);
            return decryptedStream;
        }
        public string DecryptString(string cipherText)
        {
            Aes aes = GetEncryptionAlgorithm();
            byte[] buffer = Convert.FromBase64String(cipherText);
            MemoryStream memoryStream = new MemoryStream(buffer);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            StreamReader streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
        // We have to use same KEY and IV as we use for encryption in angular side.
        // _appSettings.EncryptKey= 1203199320052021
        // _appSettings.EncryptIV = 1203199320052021
        private Aes GetEncryptionAlgorithm()
        {
            Aes aes = Aes.Create();
            var secret_key = Encoding.UTF8.GetBytes(_encryptionOptions.EncryptKey);
            var initialization_vector = Encoding.UTF8.GetBytes(_encryptionOptions.EncryptIV);
            aes.Key = secret_key;
            aes.IV = initialization_vector;
            return aes;
        }
        // This are excluded URL from encrypt- decrypt that already we added in angular side and as well as in ASP.NET CORE side.
        private List<string> GetIncludedURLList()
        {
            List<string> includedURL = new List<string>();
            //includedURL.Add("/api/login/ValidateUser");
            //includedURL.Add("/api/login/SendOtp");
            //includedURL.Add("/api/login/SaveUser");
            return includedURL;
        }

      

    }
}