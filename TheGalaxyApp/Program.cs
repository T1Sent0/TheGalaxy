using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;

using System.Text;

using TheGalaxyApp;

const string CORS_NAME = "CustomPolicy";

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

// Add services to the container.
ConfigureMassTransit.AddMassTransitWithTransport(builder.Services);

builder.Services.AddCors(opts =>
{
    opts.AddPolicy(CORS_NAME, policy =>
    {
        policy.WithOrigins(config["FRONT_APP_HOST"])
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .Build();
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(opts =>
{
    opts.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
    opts.SerializerSettings.DateFormatString = "dd.MM.yyyy HH:mm";
    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "The Galaxy API",
        Description = "Прикладной программный интерфейс компании Галактика",
        Contact = new OpenApiContact
        {
            Name = "Aleksey Gaydash",
            Email = "foregestore@gmail.com"
        }
    });

    opts.EnableAnnotations();
});

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Bearer", options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // укзывает, будет ли валидироваться издатель при валидации токена
        ValidateIssuer = true,
        // строка, представляющая издателя
        ValidIssuer = config["ACCESS_TOKEN_ISSUER"],

        // будет ли валидироваться потребитель токена
        ValidateAudience = true,
        // установка потребителя токена
        ValidAudience = config["ACCESS_TOKEN_AUDIENCE"],
        // будет ли валидироваться время существования
        ValidateLifetime = true,

        // установка ключа безопасности
        IssuerSigningKey = GetSymmetricSecurityKey(config),
        // валидация ключа безопасности
        ValidateIssuerSigningKey = true,
        ////Смещение времени при проверки времени жизни токена
        ClockSkew = TimeSpan.FromMinutes(0)
    };
});

var app = builder.Build();
app.UseCors(CORS_NAME);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        x.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });

app.Run();


static SymmetricSecurityKey GetSymmetricSecurityKey(IConfiguration configuration)
{
    return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["ACCESS_TOKEN_SECRET_KEY"]));
}
