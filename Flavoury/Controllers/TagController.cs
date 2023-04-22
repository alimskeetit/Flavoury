using AutoMapper;
using Entities.Models;
using Flavoury.Filters.Exist;
using Flavoury.Services;
using Flavoury.ViewModels.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.IdentityModel.Tokens;

namespace Flavoury.Controllers
{
    [Route("[controller]/[action]")]
    public class TagController: ControllerBase
    {
        private readonly TagService _tagService;
        private readonly IMapper _mapper;

        public TagController(TagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task Create([FromBody] CreateTagViewModel createTagViewModel)
        {
            var tag = _mapper.Map<Tag>(createTagViewModel);
            await _tagService.CreateAsync(tag);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tags = await _tagService.GetAllAsync();
            
            if (tags.IsNullOrEmpty())
                return NotFound("Тэги не найдены");
            
            return Ok(tags);
        }

        [HttpGet("{id:int}")]
        [Exist<Tag>]
        public async Task<IActionResult> Get(int id)
        {
            var tag = await _tagService.GetAsync(id);
            return Ok(_mapper.Map<TagViewModel>(tag));
        }

        [HttpGet("{id:int}")]
        [Exist<Tag>]
        public async Task<IActionResult> Update(int id)
        {
            var tag = await _tagService.GetAsync(id);
            return Ok(_mapper.Map<UpdateTagViewModel>(tag));
        }

        [HttpPut]
        [Exist<Tag>("updateTagViewModel.Id")]
        public async Task<IActionResult> Update([FromBody] UpdateTagViewModel updateTagViewModel)
        {
            var tag = await _tagService.GetAsync(updateTagViewModel.Id, asTracking: true);
            _mapper.Map(updateTagViewModel, tag);
            await _tagService.UpdateAsync(tag!);
            return Ok(updateTagViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        [Exist<Tag>]
        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteAsync(id);
            return Ok();
        }
    }
}
