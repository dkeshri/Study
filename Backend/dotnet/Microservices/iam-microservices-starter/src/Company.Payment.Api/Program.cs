using Company.Payment.Api.Clients;
using Company.Security;
using Company.ServiceToService;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddIamAuthentication(builder.Configuration, "payment.read", "payment.write");
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient("iam-token", client => client.Timeout = TimeSpan.FromSeconds(10));
builder.Services.AddSingleton<IamClientCredentialsTokenProvider>();
builder.Services.AddTransient<IamBearerHandler>();
builder.Services.AddHttpClient<IamUsersClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:Iam"]!);
    client.Timeout = TimeSpan.FromSeconds(10);
}).AddHttpMessageHandler<IamBearerHandler>();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();