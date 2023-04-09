using System.Security.Claims;
using AutoMapper;
using Entities.Models;
using Flavoury.Filters;
using Flavoury.Filters.Exist;
using Flavoury.Services;
using Flavoury.ViewModels.Recipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flavoury.Controllers
{
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;
        private readonly IngredientService _ingredientService;
        private readonly TagService _tagService;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public RecipeController(RecipeService recipeService, IMapper mapper, IngredientService ingredientService, TagService tagService, IAuthorizationService authorizationService)
        {
            _recipeService = recipeService;
            _mapper = mapper;
            _ingredientService = ingredientService;
            _tagService = tagService;
            _authorizationService = authorizationService;
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateRecipeViewModel createRecipeViewModel)
        {
            var recipe = _mapper.Map<Recipe>(createRecipeViewModel);
            //recipe.Tags.Clear();

            recipe.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            recipe.Tags.Clear();
            //Добавляем в тэги рецепта только те тэги, которые существуют в БД
            foreach (var tag in createRecipeViewModel.Tags)
            {
                var result = await _tagService.GetByNameAsync(tag.Name, asTracking: true);
                if (result == null)
                    return BadRequest(new { error = $"Тэг с названием {tag.Name} не существует", recipeViewModel = createRecipeViewModel });
                recipe.Tags.Add(result);
            }

            await _recipeService.CreateAsync(recipe);
            return Ok(recipe);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            var recipes = await _recipeService.GetAllAsync();
            if (recipes.Count == 0)
            {
                return NotFound("Рецепты не найдены");
            }
            return Ok(recipes);
        }

        [HttpGet("[action]/{id}")]
        [RecipeExists]
        public async Task<IActionResult> Get(int id)
        {
            var recipe = await _recipeService.GetAsync(id);

            return Ok(recipe);
        }

        [HttpGet("[action]/{id}")]
        [RecipeExists]
        public async Task<IActionResult> Update(int id)
        {
            var recipe = await _recipeService.GetAsync(id);

            var updateRecipe = _mapper.Map<UpdateRecipeViewModel>(recipe);

            return Ok(updateRecipe);
        }

        [Authorize]
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateRecipeViewModel updateRecipeViewModel)
        {
            var recipe = await _recipeService.GetAsync(updateRecipeViewModel.Id);

            var authResult = await _authorizationService.AuthorizeAsync(
                user: User,
                resource: recipe,
                policyName: "CanManageRecipe");

            if (!authResult.Succeeded)
                return Forbid();

            if (recipe == null)
                return NotFound($"Рецепт с id {updateRecipeViewModel.Id} не найден");

            _mapper.Map(updateRecipeViewModel, recipe);
            recipe.Tags.Clear();
            foreach (var tag in updateRecipeViewModel.Tags)
            {
                var result = await _tagService.GetByNameAsync(tag.Name, asTracking: true);
                if (result == null)
                    return BadRequest(
                        new
                        {
                            error = $"Тэг с названием {tag.Name} не существует", 
                            updateRecipeViewModel
                        });
                recipe.Tags.Add(result);

            }

            await _recipeService.UpdateAsync(recipe);
            return Ok(recipe);
        }

        [HttpDelete("[action]/{id}")]
        [RecipeExists]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _recipeService.GetAsync(id);

            var authResult = await _authorizationService.AuthorizeAsync(
                user: User,
                resource: recipe,
                policyName: "CanManageRecipe");

            if (!authResult.Succeeded)
                return Forbid();

            await _recipeService.DeleteAsync(recipe => recipe.Id == id);

            return Ok("Рецепт удалён");
        }
    }
}
