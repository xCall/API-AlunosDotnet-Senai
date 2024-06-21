using Microsoft.AspNetCore.Http.Json;
using Microsoft.PowerPlatform.Dataverse.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddSingleton<IOrganizationServiceAsync2>(service =>
    new ServiceClient("AuthType=ClientSecret;Url=https://org1e120378.crm2.dynamics.com;ClientId=b0bbd54a-70b7-4e57-8890-a341e13a4d9f;ClientSecret=_~Y8Q~afrIEKfBjnqsEcSAlCreTcq8qpq5XZ4bDI")
);

builder.Services.Configure<JsonOptions>(options =>
{ 
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "DevAllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

app.UseCors("DevAllowSpecificOrigins");

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
