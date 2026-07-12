using Domain.Entities.Articles;
using Domain.Entities.Finance;
using Domain.Entities.Households;
using Domain.Entities.People;
using Domain.Entities.Recipes;
using Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public sealed class HomeAppContext(DbContextOptions<HomeAppContext> options) : DbContext(options)
{
    // PEOPLE
    public DbSet<Person> People { get; set; }
    public DbSet<Household> Households { get; set; }
    public DbSet<HouseholdMember> HouseholdMembers { get; set; }

    // FINANCE
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountHousehold> AccountHouseholds { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryGroup> CategoryGroups { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    // TODO
    public DbSet<Todo> Todos { get; set; }
    public DbSet<TodoGroup> TodoGroups { get; set; }
    public DbSet<TodoGroupTodo> TodoGroupTodos { get; set; }
    public DbSet<TodoPerson> TodoPeople { get; set; }

    // ARTICLE
    public DbSet<Allergen> Allergens { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleAllergen> ArticleAllergens { get; set; }
    public DbSet<ArticleCategory> ArticleCategories { get; set; }
    public DbSet<ArticleNutrition> ArticleNutritions { get; set; }
    public DbSet<ArticlePrice> ArticlePrices { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<Unit> Units { get; set; }

    // RECIPES
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<PopularSearchQuery> PopularSearchQueries { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeCategory> RecipeCategories { get; set; }
    public DbSet<RecipeComment> RecipeComments { get; set; }
    public DbSet<RecipeImage> RecipeImages { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<RecipeRating> RecipeRatings { get; set; }
    public DbSet<RecipeSearchIndex> RecipeSearchIndexes { get; set; }
    public DbSet<RecipeStep> RecipeSteps { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Your default schema for legacy / other tables
        modelBuilder.HasDefaultSchema(Schemas.Default);

        // All IEntityTypeConfiguration<T> in Infrastructure (including recipe configs)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HomeAppContext).Assembly);

        // Your custom column ordering helper (works nicely together with ConfigureAuditable)
        modelBuilder.ApplyAuditedColumnOrdering();
    }
}
