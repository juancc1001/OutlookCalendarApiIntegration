using AutoMapper;
using OutlookSyncApi.Mappings;
using OutlookSyncApi.Services.Interfaces;
using User.Calendar.API.Services;

var builder = WebApplication.CreateBuilder(args);

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();
builder.WebHost.UseConfiguration(configBuilder);

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddSingleton(_ => builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(300);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".CalendarSyncApi.Session";
});

//add services
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IAuthService, OutlookSyncApi.Services.AuthenticationService>();

//automapper configuration
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfiles());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSession();

app.MapControllers();

app.Run();
