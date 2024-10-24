﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using VJN.Models;
using VJN.Authenticate;
using VJN.Repositories;
using VJN.Services;
using VJN.ModelsDTO.Imagekit;
using VJN.ModelsDTO.EmailDTOs;
using System.Text.Json.Serialization;

namespace VJN
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Schedule API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                   Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                      {
                           Reference = new OpenApiReference
                          {
                               Type = ReferenceType.SecurityScheme,
                              Id = "Bearer"
                          },
                   },
                      Array.Empty<string>()
                    }
                });
            });
            //add authentication
            builder.Services.AddScoped<JwtTokenGenerator>();
            builder.Services.AddScoped<OTPGenerator>();
            //add authentication
            var jwtSettings = new JwtSetting();
            builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
            builder.Services.AddSingleton(jwtSettings);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                   ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            }).AddGoogle(options =>
            {
                var googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
                options.CallbackPath = "/auth/callback";
            });

            var imagekitSettings = new ImagekitSettings();
            builder.Configuration.GetSection("Imagekit").Bind(imagekitSettings);

            builder.Services.AddSingleton(new Imagekit.Imagekit( imagekitSettings.PublicKey,
                imagekitSettings.PrivateKey,
                imagekitSettings.UrlEndpoint));

            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("CORSPolicy", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed((host) => true);
                });
            });

            var configuration = builder.Configuration;
            builder.Services.AddDbContext<VJNDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(typeof(Program));
            // Register services and repositories
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings")); //add email
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<IBlogService, BlogService>();

            builder.Services.AddScoped<IGoogleService, GoogleService>();

            builder.Services.AddScoped<IMediaItemRepository, MediaItemRepository>();
            builder.Services.AddScoped<IMediaItemService, MediaItemService>();

            builder.Services.AddScoped<IPostJobRepository, PostJobRepository>();
            builder.Services.AddScoped<IPostJobService, PostJobService>();

            builder.Services.AddScoped<IApplyJobRepository, ApplyJobRepository>();
            builder.Services.AddScoped<IApplyJobService, ApplyJobService>();

            builder.Services.AddScoped<ISlotRepository, SlotRepository>();
            builder.Services.AddScoped<ISlotService, SlotService>();




            // Register services and repositories

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Remove("Cross-Origin-Opener-Policy");
                await next();
            });




            app.UseHttpsRedirection();
            app.UseCors("CORSPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                await next();
                Console.WriteLine($"Response Status Code: {context.Response.StatusCode}");
                Console.WriteLine($"Response Headers: {context.Response.Headers}");
                if (!context.Response.HasStarted && context.Response.ContentType != null && context.Response.ContentType.Contains("application/json"))
                {
                    context.Response.Headers["Content-Type"] = "application/json; charset=utf-8";
                }
            });

            app.MapControllers();

            app.Run();
        }
    }
}
