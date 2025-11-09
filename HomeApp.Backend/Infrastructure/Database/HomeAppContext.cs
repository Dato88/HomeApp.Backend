using Domain.Entities.Budgets;
using Domain.Entities.People;
using Domain.Entities.Recipes.RecipesCore;
using Domain.Entities.Recipes.RecipesNutrition;
using Domain.Entities.Recipes.RecipesPricing;
using Domain.Entities.Recipes.RecipesRef;
using Domain.Entities.Recipes.RecipesSearch;
using Domain.Entities.Todos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public sealed class HomeAppContext(DbContextOptions<HomeAppContext> options) : DbContext(options)
{
    public DbSet<Person> People { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<BudgetCell> BudgetCells { get; set; }
    public DbSet<BudgetGroup> BudgetGroups { get; set; }
    public DbSet<BudgetRow> BudgetRows { get; set; }
    public DbSet<Todo> Todos { get; set; }
    public DbSet<TodoGroup> TodoGroups { get; set; }
    public DbSet<TodoGroupTodo> TodoGroupTodos { get; set; }
    public DbSet<TodoPerson> TodoPeople { get; set; }

    // ================================
    // Recipe / Nutrition / Pricing domain
    // ================================

    // ref schema
    public DbSet<Unit> Units { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<IngredientCategory> IngredientCategories { get; set; }
    public DbSet<Allergen> Allergens { get; set; }
    public DbSet<IngredientAllergen> IngredientAllergens { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }

    // nutrition schema
    public DbSet<IngredientNutrition> IngredientNutritions { get; set; }

    // core schema (recipe core)
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeStep> RecipeSteps { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<RecipeImage> RecipeImages { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<RecipeRating> RecipeRatings { get; set; }
    public DbSet<RecipeComment> RecipeComments { get; set; }
    public DbSet<RecipeCategory> RecipeCategories { get; set; }
    public DbSet<RecipeTag> RecipeTags { get; set; }

    // pricing schema
    public DbSet<Store> Stores { get; set; }
    public DbSet<IngredientPrice> IngredientPrices { get; set; }

    // search schema
    public DbSet<RecipeSearchIndex> RecipeSearchIndices { get; set; }
    public DbSet<PopularSearchQuery> PopularSearchQueries { get; set; }

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
