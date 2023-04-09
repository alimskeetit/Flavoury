using Entities;
using Entities.Models;
using Flavoury.AutoMapper;
using Flavoury.Filters.Exist;
using Flavoury.Requirements;
using Flavoury.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Create services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<RecipeExistsAttribute>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Подключаем БД MS SQL server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(optionsAction: options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Flavoury"));
    options.EnableSensitiveDataLogging();
});

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;

});

// Авторизация
builder.Services.AddAuthorization(options =>
{
    //Политика
    options.AddPolicy("CanManageRecipe", policybuilder =>
    {
        policybuilder.AddRequirements(new IsCreatorRequirement());
    });
});
builder.Services.AddScoped<IAuthorizationHandler, IsCreatorHandler>();

// Добавляем сервисы
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<IngredientService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddTransient<DataSeeder>();
builder.Services.AddAutoMapper(typeof(AppMappingProfile));


var app = builder.Build();
//var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
//using (var scope = scopedFactory!.CreateScope())
//{
//    var service = scope.ServiceProvider.GetService<DataSeeder>();
//    await service!.SeedAsync();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

