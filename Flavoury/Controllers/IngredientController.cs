using AutoMapper;
using Entities.Models;
using Flavoury.Services;
using Flavoury.ViewModels.Ingredient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Flavoury.Controllers;

[Route("[controller]")]
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

    [HttpGet("[action]/{recipeId}")]
    public async Task<IActionResult> Get(int recipeId)
    {
        var recipe = await _recipeService.GetAsync(recipeId);
        
        return recipe == null ? 
            NotFound($"Рецепт с id {recipeId} не найден") 
            : Ok(_mapper.Map<ICollection<IngredientViewModel>>(recipe.Ingredients));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Update([FromBody] UpdateIngredientViewModel updateIngredientViewModel)
    {
        var ingredient = await _ingredientService.GetAsync(updateIngredientViewModel.Id);
        
        if (ingredient == null) 
            return NotFound($"Ингредиент с id {updateIngredientViewModel.Id} не найден");
        
        _mapper.Map(updateIngredientViewModel, ingredient);

        await _ingredientService.UpdateAsync(ingredient);

        return Ok(updateIngredientViewModel);
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ingredient = await _ingredientService.GetAsync(id);

        if (ingredient == null)
            return NotFound($"Ингредиент с id {id} не найден");

        await _ingredientService.DeleteAsync(id);

        return Ok("Ингредиент удалён");
    }
}