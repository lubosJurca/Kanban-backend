using Kanban_backend.Data;
using Kanban_backend.Models;
using Kanban_backend.Repositories;
using Kanban_backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =============================================================================
// ================= Configure postgresql database connection ==================
// =============================================================================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// =================================================================================
// ================ Configure IDENTITY password and user settings ==================
// =================================================================================
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{ 
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = false; //Default value is false, this is just for clarity and for learning purposes
    options.SignIn.RequireConfirmedAccount = false; //Default value is false, this is just for clarity and for learning purposes

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); //Lockout for 15 minutes after failed attempts
    options.Lockout.MaxFailedAccessAttempts = 5; //Lockout after 5 failed attempts
    options.Lockout.AllowedForNewUsers = true; //Allow lockout for new users
})
    .AddEntityFrameworkStores<AppDbContext>() //Use the AppDbContext for Identity
    .AddDefaultTokenProviders(); //Add default token providers for password reset, email confirmation, etc.

builder.Services.ConfigureApplicationCookie(options =>
{
    //Cookie Security settings
    options.Cookie.HttpOnly = true; // Cookie accessible only via HTTP(S), not JavaScript - helps prevent XSS attacks
	options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
	 ? CookieSecurePolicy.SameAsRequest
	 : CookieSecurePolicy.Always; // Cookie only sent over HTTPS
	options.Cookie.SameSite = SameSiteMode.None; //Cookie sent with cross-site requests - when frontend and backend are on different domains
    options.Cookie.Name = "KanbanAuthCookie"; //Custom cookie name

    //Cookie lifetime settings
    options.ExpireTimeSpan = TimeSpan.FromDays(7); //COOKIe valid for 7 days
    options.SlidingExpiration = true; //If the user is active, renew the cookie expiration time on each request

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Return 401 instead of redirecting, for API scenarios
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden; // Return 403 instead of redirecting, for API scenarios
        return Task.CompletedTask;
    };
});

// ==============================================================
// =================== Configure CORS policy ====================
// ==============================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:4200") // Frontend URL
                .AllowAnyHeader() // Allow any http headers
                .AllowAnyMethod() // Allow any http methods (GET, POST, etc.)
                .AllowCredentials(); // Allow cookies to be sent in cross-origin requests
    });
});

// ============================================================================
// =================== Add Repositories to the DI container ===================
// ============================================================================
builder.Services.AddScoped<IBoardRepository, BoardRepository >();
builder.Services.AddScoped<IColumnRepository, ColumnRepository>();
builder.Services.AddScoped<IKanbanTaskRepository, KanbanTaskRepository>();
builder.Services.AddScoped<ISubtaskRepository, SubtaskRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ==================================================================
// =================== Add Controllers support ===================
// ==================================================================
builder.Services.AddControllers();

// =======================================================================================
// ================== Swagger configuration to explore the API endpoints ==================
// =======================================================================================
builder.Services.AddEndpointsApiExplorer();

// Without this line, Swagger UI will not work
builder.Services.AddSwaggerGen();

// ======================================================================
// ================= Build the app ==================
// ======================================================================
var app = builder.Build();

// ======================================================================
// ================= Middleware pipeline configuration ==================
// ======================================================================

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

// ================= Maps through the controllers in the project ==================
app.MapControllers();

if (app.Environment.IsDevelopment())

{

    app.UseSwagger();

    app.UseSwaggerUI(c =>

    {

        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        // Set Swagger UI at app's root
        c.RoutePrefix = string.Empty;

    });

}

app.Run();
