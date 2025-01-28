using Authentication.Domain.Entities;
using Authentication.Domain.Interface;
using Authentication.Infrastructure.Persistence;
using Authentication.Infrastructure.Repositories;
using Authentication.WebApi.Ioc;
using CodeFlow.Start.Lib.Config;
using Destructurama;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// =====================================
// Logging Configuration with Serilog
// =====================================

builder.Host.UseSerilog((context, configuration) =>
	configuration
		.ReadFrom.Configuration(context.Configuration)
		.Destructure.UsingAttributes(x => x.IgnoreNullProperties = true)
);

// =====================================
// Services Configuration
// =====================================

builder.Services.AddControllers();

string? sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContextPool<SqlContext>(options =>
	options.UseSqlServer(sqlConnection));
builder.Services.ConfigureDatabaseSqlServer<SqlContext>(sqlConnection!);
builder.Services.UpdateMigrationDatabase<SqlContext>();


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
	.AddEntityFrameworkStores<SqlContext>()
	.AddDefaultTokenProviders();
builder.Services.ConfigureAuthentication(builder.Configuration);



// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
});
builder.Services.ConfigureSwagger();


// Repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Application
builder.Services.ConfigureServiceIoc();

var app = builder.Build();

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
