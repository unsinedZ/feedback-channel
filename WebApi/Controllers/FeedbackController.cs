using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebApi.Constants;
using WebApi.Models;
using WebApi.Projections;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiVersion(ApiVersions.OneDotZero)]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}/feedbacks")]
    public class FeedbackController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IFeedbackRepository feedbackRepository;

        public FeedbackController(IMapper mapper, IFeedbackRepository feedbackRepository)
        {
            this.mapper = mapper;
            this.feedbackRepository = feedbackRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<FeedbackModel>> GetListAsync()
        {
            return (await feedbackRepository.GetListAsync())
                .Select(mapper.Map<FeedbackModel>);
        }

        [HttpGet]
        [Route("{id:required}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var feedback = await feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
                return NotFound();
            
            return Ok(mapper.Map<FeedbackModel>(feedback));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]FeedbackModel feedback)
        {
            if (!ModelState.IsValid)
                throw new InvalidOperationException("Model is invalid.");

            try
            {
                var id = await feedbackRepository.CreateAsync(mapper.Map<FeedbackProjection>(feedback));
                return CreatedAtAction(nameof(GetByIdAsync), id);
            }
            catch (MySqlException)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{id:required}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var affected = await feedbackRepository.DeleteByIdAsync(id);
            if (affected == 0)
                return NotFound();

            return Ok();
        }
    }
}
