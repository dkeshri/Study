using Company.IAM.Api.Configuration;
using Company.IAM.Api.Data;
using Company.IAM.Api.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IamDatabase")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false; // Enable email confirmation for production.
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Password.RequiredLength = 12;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultUI();


builder.Services.AddIamOpenIddict(builder.Configuration);

// IAM's own user-management API validates access tokens just like other resource APIs.
var issuer = builder.Configuration["OpenIddict:Issuer"]!;
builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.Authority = issuer;
    options.MetadataAddress = new Uri(new Uri(issuer), ".well-known/openid-configuration").ToString();
    options.RequireHttpsMetadata = true;
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = "iam-api",
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.FromSeconds(30),
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("users.read", policy => policy.RequireAuthenticatedUser()
        .RequireAssertion(ctx => ctx.User.FindAll("scope")
            .SelectMany(c => c.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Contains("users.read", StringComparer.Ordinal)));
});

builder.Services.AddControllers();
builder.Services.AddRazorPages();

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
app.UseRouting();
// IMPORTANT: CORS must run after routing and before authentication.
app.UseCors("AngularSpa");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();

// Apply EF migrations as a deployment step before starting the app.
// Seed OAuth clients/scopes and an optional bootstrap account after migrations exist.
await IdentitySeeder.SeedAsync(app.Services, app.Configuration);

app.Run();