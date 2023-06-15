

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using Villa_WebAPI;
using Villa_WebAPI.Data;
using Villa_WebAPI.Logging;
using Villa_WebAPI.Repository;
using Villa_WebAPI.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//minimum level here is after which all logs will be tracked
//rollinginterval here is defining that how often you want new file to be created.
//Log.Logger = new LoggerConfiguration().MinimumLevel
//    .Debug().WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day)
//    .CreateBootstrapLogger();

//this is you telling the application that it does not have to use default logger
//and use the serilog instead
//builder.Host.UseSerilog();




//add entityFrameword connection string
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

//enable caching
builder.Services.AddResponseCaching();
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//add automapper service
builder.Services.AddAutoMapper(typeof(MappingConfig));

//this is to add versioning in API
builder.Services.AddApiVersioning(options =>
{
    //assume the default version when its not specified
    options.AssumeDefaultVersionWhenUnspecified = true;
    //but we also have to define whats the default version
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

//specify which version of API we are hitting
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var key = builder.Configuration.GetValue<string>("AppSettings:Secret");
builder.Services.AddAuthentication(x => {
    //these are just constant name that are inside jwtbearer defaults
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata=false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateAudience = false,
            ValidateIssuer = false,
        };
    }); ;

builder.Services.AddControllers
    (option => {
        //option.ReturnHttpNotAcceptable = true;
        //add general caching of 30 seconds for the project
        option.CacheProfiles.Add("Default30", new CacheProfile()
        {
            Duration = 30
        });
    })
    .AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Swagger take the value of whatever you typed in, saves it,
//and uses that for all future requests to the API that require authorization.
builder.Services.AddSwaggerGen(options =>
//AddSecutiryDefinition basically describes how the API is protected to the generator swagger
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT authorization header using the bearer scheme. \r\n\r\n" +
        "Enter 'Bearer' [space] and then your token is the next input below. \r\n\r\n" +
        "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    //this is for global security requirement
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Scheme = "oAuth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    //add v1 for swagger. here v1 should match with v1 in "/swagger/v1/swagger.json" that we
    //have defined at bottom in our URL
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Villa_API",
        Description = "API to manage Villa",
        TermsOfService = new Uri("https://www.example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Dotnetmastery",
            Url = new Uri("https://www.dotnetmastery.com")
        },
        License = new OpenApiLicense
        {
            Name = "Example Licence",
            Url = new Uri("https://www.example.com")
        }
    }) ;

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Villa_API",
        Description = "API to manage Villa",
        TermsOfService = new Uri("https://www.example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Dotnetmastery",
            Url = new Uri("https://www.dotnetmastery.com")
        },
        License = new OpenApiLicense
        {
            Name = "Example Licence",
            Url = new Uri("https://www.example.com")
        }
    });

});


builder.Services.AddSingleton<ILogging, Logging>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //add v1 for swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "VillaAPIV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "VillaAPIV2");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
