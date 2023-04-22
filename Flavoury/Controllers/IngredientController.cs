using AutoMapper;
using Entities.Models;
using Flavoury.Filters;
using Flavoury.Filters.CanManage;
using Flavoury.Filters.Exist;
using Flavoury.Services;
using Flavoury.ViewModels.Ingredient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Flavoury.Controllers;

[Route("[controller]/[action]")]
public class IngredientController : ControllerBase
{
    private readonly IngredientService _ingredientService;
    private readonly RecipeService _recipeService;
    private readonly IMapper _mapper;

    public IngredientController(IngredientService ingredientService, IMapper mapper, RecipeService recipeService)
    {
        _ingredientService = ingredientService;
        _mapper = mapper;
        _recipeService = recipeService;
    }
    
    [HttpGet("{recipeId:int}")]
    [Exist<Recipe>(pathToId: "recipeId")]
    public async Task<IActionResult> Get(int recipeId)
    {
        var recipe = await _recipeService.GetAsync(recipeId, asTracking: false);
        return Ok(_mapper.Map<ICollection<IngredientViewModel>>(recipe!.Ingredients));
    }

    //todo: получить update view может только создатель или админ
    [Authorize]
    [HttpGet("{id:int}")]
    [Exist<Ingredient>]
    [CanManage<Ingredient>]
    public async Task<IActionResult> Update(int id)
    {
        var ingredient = await _ingredientService.GetAsync(id);
        var updateIngredient = _mapper.Map<UpdateIngredientViewModel>(ingredient);
        return Ok(updateIngredient);
    }

    //todo: изменять может только создатель или админ
    //todo: проверка существует ли ингредиент
    [Authorize]
    [HttpPut("")]
    [Exist<Ingredient>(pathToId: "updateIngredientViewModel.Id")]
    [CanManage<Ingredient>]
    //[CanManageRecipe("updateIngredientViewModel.RecipeId")]
    public async Task<IActionResult> Update([FromBody] UpdateIngredientViewModel updateIngredientViewModel)
    {
        var ingredient = await _ingredientService.GetAsync(updateIngredientViewModel.Id);
        _mapper.Map(updateIngredientViewModel, ingredient);
        await _ingredientService.UpdateAsync(ingredient!);
        return Ok(updateIngredientViewModel);
    }

    //todo: удалять может только создатель и админ
    [HttpDelete("{id:int}")]
    [Exist<Ingredient>]
    [CanManage<Ingredient>]
    public async Task<IActionResult> Delete(int id)
    {
        await _ingredientService.DeleteAsync(id);
        return Ok("Ингредиент удалён");
    }
}