using AutoMapper;
using Entities.Models;
using Flavoury.Services;
using Flavoury.ViewModels.Tag;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.IdentityModel.Tokens;

namespace Flavoury.Controllers
{
    [Route("[controller]")]
    public class TagController: ControllerBase
    {
        private readonly TagService _tagService;
        private readonly IMapper _mapper;

        public TagController(TagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public async Task Create([FromBody] CreateTagViewModel createTagViewModel)
        {
            var tag = _mapper.Map<Tag>(createTagViewModel);
            
            await _tagService.CreateAsync(tag);
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            var tags = await _tagService.GetAllAsync();
            
            if (tags.IsNullOrEmpty())
                return NotFound("Тэги не найдены");
            
            return Ok(tags);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var tag = await _tagService.GetAsync(id);
        
            return tag == null ? 
                NotFound($"Тэг c id {id} не найден") 
                : Ok(_mapper.Map<TagViewModel>(tag));
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _tagService.GetAsync(id);

            if (tag == null)
                return NotFound($"Тэг с id {id} не найден");

            await _tagService.DeleteAsync(id);

            return Ok();
        }
    }
}
