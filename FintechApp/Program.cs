using Coravel;
using FluentValidation;
using Serilog;
using Microsoft.EntityFrameworkCore;
using FintechApp.Repository;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

 
// builder.Logging.AddFile("Logs/myapp-{Date}.txt");
builder.Services.AddLogging(builder => {
    builder.ClearProviders();
     builder.AddFile("Logs/myapp-{Date}.txt");
    builder.AddSerilog();
    // builder.AddFile("Logs/myapp-{Date}.txt");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((c)=> 
{
c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "FintechApp.xml"));

c.SwaggerDoc("v1", new() { Title = "Pappa´s API", Version = "v1" });

    // Define the OAuth2.0 scheme that's in use (i.e., Implicit Flow)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
}
);
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddDbContext<AppDbContext>((options)=>{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ITransactionRepository<Transaction>, TransactionRepository>();
builder.Services.AddScoped<IUserRepository<User>, UserRepository>();
builder.Services.AddScoped<TransactionJob>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddScheduler();

 builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// app.Services.UseScheduler(s=>{
//     s.Schedule(() => Console.WriteLine("It's alive! 🧟")).EverySecond();
//     s.Schedule<TransactionJob>().EverySecond();
// });

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


 
public partial class Program{

}