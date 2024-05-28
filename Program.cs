using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsAPI.Models;
using System.Text;

var MyAllowSpesificOrigins = "_myAllowSpesificOrigins";
var builder = WebApplication.CreateBuilder(args);

//----------------------CORS POLICY----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpesificOrigins, policy =>
    {
        policy.WithOrigins("http://127.0.0.1:8080")
        .AllowAnyHeader() // hepsi için bu şekilde. /belli parametrelerin(authentication) olma zorunluluğu Requestteki header Bareer Token 
        .AllowAnyMethod(); //hepsi için bu şekilde /hangi method
    });
});


builder.Services.AddDbContext<ProductsContext>(x => x.UseSqlite("Data Source=products.db"));
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<ProductsContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;

    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    
});

// ---------------------Token doğruluk kontrolü
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        //api yayınlayan
        ValidateIssuer = false,
        ValidIssuer = "sevvalalp.com",

        //api kim/hangi servisler için
        ValidateAudience = false,
        ValidAudience = "",
        ValidAudiences = new string[]
        {
            "a", "b"
        },

        ValidateIssuerSigningKey = true, //tokeni validate etmek
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes( //validate ederken kullanılacak key bilgisi
            builder.Configuration.GetSection("AppSettings:Secret").Value ?? "")),
            ValidateLifetime = true

    };

});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//------------------swagger arayüzüne authentication button--------------
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseRouting();

app.UseCors(MyAllowSpesificOrigins); // bu aralıkta olmalı

app.UseAuthorization();

app.MapControllers();

app.Run();
