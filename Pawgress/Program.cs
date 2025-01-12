using Pawgress.Configurations;
using Pawgress.Data;
using Microsoft.EntityFrameworkCore;
using Pawgress.Extensions;

var builder = WebApplication.CreateBuilder(args);

// voor bug
var isMigration = args.Contains("--migration");

if (!isMigration)
{
    // Config services
    builder.Services.RegisterServices();

    // Configure JSON Web Tokens
    builder.Services.ConfigureJwt(builder.Configuration);

    // Configure Swagger alleen tijdens runtime
    if (builder.Environment.IsDevelopment())
    {
        Console.WriteLine("Configuring Swagger...");
        builder.Services.ConfigureSwagger();
    }
}
else
{
    Console.WriteLine("Skipping Swagger configuration...");
}
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddControllers();

// Configure DbContext altijd (voor migraties en runtime)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configureer CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger middleware alleen als geen migratie
if (!isMigration && app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next.Invoke();
});

// Gebruik de CORS-policy
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// // seeding db
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     DatabaseSeeder.Seed(context);
// }

app.MapControllers();
app.Run();




