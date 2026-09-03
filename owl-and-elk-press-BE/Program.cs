using owl_and_elk_press_BE.Endpoints;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
// builder.Services.AddDbContext<ApplicationDbContext>(...);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddScoped<IStripeService, StripeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();

StripeConfiguration.ApiKey = app.Configuration["Stripe:SecretKey"]; 

app.UseCors("AllowFrontend"); 
app.MapPaymentEndpoints();  

app.Run();

public partial class Program { }
