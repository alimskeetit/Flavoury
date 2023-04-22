using System.Security.Claims;
using AutoMapper;
using Entities.Models;
using Flavoury.Filters;
using Flavoury.Filters.CanManage;
using Flavoury.Filters.Exist;
using Flavoury.Services;
using Flavoury.ViewModels.Recipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flavoury.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeService;
        private readonly IngredientService _ingredientService;
        private readonly TagService _tagService;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;

        public RecipeController(RecipeService recipeService, IMapper mapper, IngredientService ingredientService, TagService tagService, IAuthorizationService authorizationService, UserManager<User> userManager)
        {
            _recipeService = recipeService;
            _mapper = mapper;
            _ingredientService = ingredientService;
            _tagService = tagService;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRecipeViewModel createRecipeViewModel)
        {
            var recipe = _mapper.Map<Recipe>(createRecipeViewModel);
            recipe.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            recipe.Tags.Clear();
            //Добавляем в тэги рецепта только те тэги, которые существуют в БД
            foreach (var tagName in createRecipeViewModel.Tags.Select(tag => tag.Name))
            {
                var result = await _tagService.GetByNameAsync(tagName, asTracking: true);
                if (result == null)
                    return BadRequest(new
                    {
                        error = $"Тэг с названием {tagName} не существует",
                        createRecipeViewModel
                    });
                recipe.Tags.Add(result);
            }

            await _recipeService.CreateAsync(recipe);
            var recipeViewModel = _mapper.Map<RecipeViewModel>(recipe);
            return Ok(recipeViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var recipes = await _recipeService.GetAllAsync();
            if (recipes.Count == 0)
            {
                return NotFound("Рецепты не найдены");
            }

            var recipeViewModels = _mapper.Map<ICollection<RecipeViewModel>>(recipes);
            foreach (var recipe in recipeViewModels)
            {
                recipe.Creator = (await _userManager.FindByIdAsync(recipe.Creator))!.UserName!;
            }
            return Ok(recipeViewModels);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [Exist<Recipe>]
        public async Task<IActionResult> Get(int id)
        {
            var recipe = await _recipeService.GetAsync(id, asTracking: false);
            var recipeViewModel = _mapper.Map<RecipeViewModel>(recipe);
            recipeViewModel.Creator = (await _userManager.FindByIdAsync(recipeViewModel.Creator))!.UserName!;
            return Ok(recipeViewModel);
        }
        
        [HttpGet("{id:int}")]
        [Exist<Recipe>]
        [CanManage<Recipe>]
        public async Task<IActionResult> Update(int id)
        {
            var recipe = await _recipeService.GetAsync(id, asTracking: true);
            var updateRecipe = _mapper.Map<UpdateRecipeViewModel>(recipe);
            return Ok(updateRecipe);
        }
        
        [HttpPut]
        [Exist<Recipe>(pathToId: "updateRecipeViewModel.Id")]
        [CanManage<Recipe>(pathToId: "updateRecipeViewModel.Id")]
        //todo: баг: можно изменить и вставить ингредиент, который относится совсем к другому рецепту
        public async Task<IActionResult> Update([FromBody] UpdateRecipeViewModel updateRecipeViewModel)
        {
            var recipe = await _recipeService.GetAsync(updateRecipeViewModel.Id, asTracking: true);
            _mapper.Map(updateRecipeViewModel, recipe);
            // Очищаем тэги изначального рецепта
            recipe!.Tags.Clear();
            foreach (var tagName in updateRecipeViewModel.Tags.Select(tagViewModel => tagViewModel.Name))
            {
                var result = await _tagService.GetByNameAsync(tagName, asTracking: true);
                if (result == null)
                    return BadRequest(
                        new
                        {
                            error = $"Тэг с названием {tagName} не существует", 
                            updateRecipeViewModel
                        });
                recipe.Tags.Add(result);
            }
            await _recipeService.UpdateAsync(recipe);
            return Ok(_mapper.Map<RecipeViewModel>(recipe));
        }

        [HttpDelete("{id:int}")]
        [Exist<Recipe>]
        [CanManage<Recipe>]
        public async Task<IActionResult> Delete(int id)
        {
            await _recipeService.DeleteAsync(recipe => recipe.Id == id);
            return Ok("Рецепт удалён");
        }
    }
}
