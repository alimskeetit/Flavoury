using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Recipe> Recipes { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var tags = new Dictionary<string, Tag>
        {
            { "Огуречный", new Tag { Id = 1, Name = "Огуречный" } },
            { "Помидорный", new Tag { Id = 2, Name = "Помидрный" } },
            { "Тэг", new Tag { Id = 3, Name = "Тэг" } }
        };

        var ingredients = new Dictionary<string, Ingredient>
        {
            {
                "Огурец", new Ingredient
                {
                    Id = 1,
                    Name = "Огурец",
                    Calories = 15,
                    RecipeId = 1
                }
            },
            {
                "Помидор", new Ingredient
                {
                    Id = 2,
                    Name = "Помидор",
                    Calories = 15,
                    RecipeId = 1
                }
            }
        };
        modelBuilder.Entity<Tag>().HasData(new Tag { Id = 1, Name = "Огуречный" }, new Tag { Id = 2, Name = "Помидрный" });

        //modelBuilder.Entity<Ingredient>().HasData(ingredients["Огурец"], ingredients["Помидор"]);
        
        //modelBuilder.Entity<Recipe>().HasData(
        //    new Recipe[]
        //    {
        //        new()
        //        {
        //            Id = 1,
        //            Description = "Огуречный помидор",
        //            Tags = new[]
        //            {
        //                tags["Огуречный"],
        //                tags["Помидорный"]
        //            },
        //            Ingredients = new[]
        //            {
        //                ingredients["Огурец"],
        //                ingredients["Помидор"]
        //            }
        //        }
        //    }
        //);


        //modelBuilder.Entity<Recipe>().HasData(
        //        new Recipe
        //        {
        //            Id = 1, 
        //            Description = "Огурчик", 
        //            Ingredients = new List<Ingredient>
        //            {
        //                new() {Name = "Огурец", Calories = 15}
        //            }
        //        }
        //    );
    }
}