using AutoMapper;
using BaseSwagger.Data.Models;
using BaseSwagger.Services.Interfaces;
using BaseSwagger.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseSwagger.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DummiesController : ControllerBase
    {
        private readonly IDummyService _dummyService;
        private readonly IMapper _mapper;

        public DummiesController(IDummyService dummyService, IMapper mapper)
        {
            _dummyService = dummyService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DummyVm>> Get()
        {
            var dummies = _dummyService.Get();
            return Ok(_mapper.Map<IEnumerable<DummyVm>>(dummies));
        }

        [HttpGet("{id}")]
        public ActionResult<DummyVm> Get(string id)
        {
            var dummy = _dummyService.Get(id);
            return _mapper.Map<DummyVm>(dummy);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DummyVm dummyVm)
        {
            var dummy = _mapper.Map<Dummy>(dummyVm);
            await _dummyService.Create(dummy);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateDummyVm updateDummyVm)
        {
            var updateDummy = _mapper.Map<Dummy>(updateDummyVm);
            await _dummyService.Update(id, updateDummy);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _dummyService.Delete(id);
            return Ok();
        }
    }
}
