using LibraryManagement.BUSINESS.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using LibraryManagement.DATAACCESS;
using LibraryManagement.DATAACCESS.Interfaces;
using LibraryManagement.MODELS.Models;
using LibraryManagement.DATAACCESS.Repository;
using LibraryManagement.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Serilog Implementation //Read Configuration from appSettings    
var logconfig = new ConfigurationBuilder().AddJsonFile("logconfig.json").Build();
//Initialize Logger    
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(logconfig).CreateLogger();

//5 . Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // React app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });

    //optional
    //options.AddPolicy("AllowAll",
    //policy =>
    //{
    //    policy.AllowAnyOrigin()
    //          .AllowAnyHeader()
    //          .AllowAnyMethod();
    //});

});



//1.Database Connection:

builder.Services.AddDbContext<LibraryManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));




// Add services to the container.
//2. Dependency Injection
// configure DI for application services
builder.Services.ConfigureBusinessService();


//3. JWT Authentication

// Configure JWT Authentication --- START
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//Authenticate design
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryManagementAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });


    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Enable CORS in the middleware
app.UseCors("AllowReactApp");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//4.Controllers and Middleware:

app.UseAuthentication(); // Ensures JWT tokens are validated
app.UseAuthorization();  // Enforces role-based access control
app.MapControllers();    // Maps all API controllers


app.Run();
