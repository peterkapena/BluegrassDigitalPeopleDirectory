using BluegrassDigitalPeopleDirectory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;
using BluegrassDigitalPeopleDirectory.Services.Setting;
using BluegrassDigitalPeopleDirectory.Services.Bug;
using static BluegrassDigitalPeopleDirectory.Models.DBContext;
using BluegrassDigitalPeopleDirectory.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


Setting setting = new();
builder.Configuration.Bind(nameof(setting), setting);

AddServices();

AddIdentity();

AddAuthentication();

ConfigureApp();



void AddServices()
{
    builder.Services.AddControllers(config =>
    {
        config.Filters.Add<HttpResponseExceptionFilter>();
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

    builder.Services.AddSwaggerGen(c =>
    {
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        c.IgnoreObsoleteActions();
        c.IgnoreObsoleteProperties();
        c.CustomSchemaIds(type => type.FullName);
    });

    builder.Services.AddSingleton(setting);
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddTransient<IErrorLogService, ErrorLogService>();
    var MyAllowAllHeadersPolicy = "MyAllowAllHeadersPolicy";

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowAllHeadersPolicy,
                          policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    });
}

void AddAuthentication()
{
    var tokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        //ValidateActor=true,
        ValidIssuer = setting.JwtSetting.Issuer,
        ValidAudience = setting.JwtSetting.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.JwtSetting.IssuerSigningKey))
    };

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = tokenValidationParameters;
    });
}

void AddIdentity()
{
    var connectionString = new ConnectionString();
    builder.Configuration.Bind(nameof(connectionString), connectionString);

    builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(setting.ConnectionString));

    builder.Services.AddIdentity<User, IdentityRole>(opts =>
    {
        opts.User.RequireUniqueEmail = setting.IsProduction;
        opts.Password.RequiredLength = 6;
        opts.Password.RequireNonAlphanumeric = false;
        opts.Password.RequireLowercase = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireDigit = false;
    }).AddEntityFrameworkStores<DBContext>().AddDefaultTokenProviders();
}

void ConfigureApp()
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.UseAuthorization();

    app.MapControllers();
    app.UseCors("MyAllowAllHeadersPolicy");

    app.Run();
}