using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Contracts;
using Entities;

namespace Repository
{
    internal class RecipeRepository: RepositoryBase<Recipe>, IRecipeRepository
    {
        public RecipeRepository(AppDbContext context): base(context) { }
    }
}
