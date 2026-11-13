using Company.Security;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddIamAuthentication(builder.Configuration, "pricing.read", "pricing.write");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularSpa", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseHttpsRedirection();
// IMPORTANT: CORS must run after routing and before authentication.
app.UseCors("AngularSpa");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();