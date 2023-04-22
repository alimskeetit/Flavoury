using AutoMapper;
using Entities;
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
            
            CreateMap<RecipeViewModel, Recipe>()
                .ReverseMap()
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(recipe => recipe.UserId));

            CreateMap<UpdateRecipeViewModel, Recipe>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CreateRecipeViewModel, Recipe>().ReverseMap()
                .ForMember(dest => dest.Tags, opt => opt.Ignore())
                .ReverseMap();
            
            CreateMap<TagViewModel, Tag>().ReverseMap();
            CreateMap<UpdateTagViewModel, Tag>()
                .ReverseMap();

            CreateMap<UserViewModel, User>().ReverseMap();
            CreateMap<RegisterViewModel, User>().ReverseMap();
            CreateMap<EditUserViewModel, User>().ReverseMap();
        }
    }
}
