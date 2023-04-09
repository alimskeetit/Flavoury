using AutoMapper;
using Entities.Models;
using Flavoury.ViewModels.Account;
using Flavoury.ViewModels.Ingredient;
using Flavoury.ViewModels.Recipe;
using Flavoury.ViewModels.Tag;

namespace Flavoury.AutoMapper
{
    public class AppMappingProfile: Profile
    {
        public AppMappingProfile()
        {
            CreateMap<RecipeViewModel, Recipe>().ReverseMap();

            // Маппинг из ViewModel в Entity и обратно
            CreateMap<IngredientViewModel, Ingredient>().ReverseMap();
            CreateMap<CreateIngredientViewModel, Ingredient>().ReverseMap();
            CreateMap<UpdateIngredientViewModel, Ingredient>().ReverseMap();
            CreateMap<UpdateRecipeViewModel, Recipe>()
                //.ForMember(dest => dest.Ingredients, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<CreateRecipeViewModel, Recipe>().ReverseMap()
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<TagViewModel, Tag>().ReverseMap();
            CreateMap<UserViewModel, User>().ReverseMap();
        }
    }
}
