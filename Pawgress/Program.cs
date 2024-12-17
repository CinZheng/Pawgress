using Pawgress.Configurations;
using Pawgress.Data;
using Microsoft.EntityFrameworkCore;
using Pawgress.Extensions;

var builder = WebApplication.CreateBuilder(args);

// config services
builder.Services.RegisterServices();

// configure dbcontext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// configure jsonwebtoken
builder.Services.ConfigureJwt(builder.Configuration);

// configure swagger using the extension
builder.Services.ConfigureSwagger();

var app = builder.Build();

// swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// authenticatie en authorizatie
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
