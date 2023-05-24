using Villa_WebApp;
using Villa_WebApp.Services;
using Villa_WebApp.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));
var app = builder.Build();


//adding httpClient on the VillaService
builder.Services.AddHttpClient<IVillaServices, VillaService>();


//Registering VillaService to dependency injection using the addScoped.
//AddScoped is  - for a single request we will have one object of VillaService even if it is requested
//10 times, same object will be used
builder.Services.AddScoped<IVillaServices, VillaService>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
