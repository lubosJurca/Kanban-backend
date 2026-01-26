using Kanban_backend.Data;
using Kanban_backend.Repositories;
using Kanban_backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configure PostgreSQL database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add Repositories to the DI container
builder.Services.AddScoped<IBoardRepository, BoardRepository >();
builder.Services.AddScoped<IColumnRepository, ColumnRepository>();
builder.Services.AddScoped<IKanbanTaskRepository, KanbanTaskRepository>();
builder.Services.AddScoped<ISubtaskRepository, SubtaskRepository>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
//Add Controllers support
builder.Services.AddControllers();

//Swagger configuration to explore the API endpoints
builder.Services.AddEndpointsApiExplorer();

// Without this line, Swagger UI will not work
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Maps through the controllers in the project
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
