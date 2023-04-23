

using Serilog;
using Villa_WebAPI.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//minimum level here is after which all logs will be tracked
//rollinginterval here is defining that how often you want new file to be created.
//Log.Logger = new LoggerConfiguration().MinimumLevel
//    .Debug().WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day)
//    .CreateBootstrapLogger();

//this is you telling the application that it does not have to use default logger
//and use the serilog instead
builder.Host.UseSerilog();


builder.Services.AddControllers
    (option => { 
        //option.ReturnHttpNotAcceptable = true;
    })
    .AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogging, Logging>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
