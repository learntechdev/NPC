using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NPC.Server;
using NPC.Server.EntityModels;
using NPC.Server.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




//adding authorization box to swagger
builder.Services.AddSwaggerAuthorization();


//adding token validator from headers
builder.Services.AddtokenValidator(builder);
builder.Services.AddAuthentication();

//cors  
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCustomCors(MyAllowSpecificOrigins);



//reading appsettings.json file
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);


builder.Services.AddDbContext<NPCTableContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("NPC")));



//jwt
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("AppSettings:JwtOptions"));
builder.Services.Configure<EncryptionOptions>(builder.Configuration.GetSection("AppSettings:EncryptionOptions"));



//service registration
builder.Services.AddMyScoped();

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});


//builder.Services.Configure<IISServerOptions>(options =>
//{
//    options.AutomaticAuthentication = false;
//});



var app = builder.Build();


app.UseDefaultFiles();
app.UseStaticFiles();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<EncryptionMiddleware>();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});
//app.MapFallbackToFile("/index.html");

app.Run();
