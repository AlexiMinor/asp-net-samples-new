using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.WebAPI.Responses;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public InitController(IArticleService articleService)
        {
            _articleService = articleService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Init()
        {
            RecurringJob.AddOrUpdate(
                "GetAllArticleData",
                () => _articleService.AggregateArticlesDataFromRssAsync(new CancellationToken()),
                "0,20,40 * * * *");



            return Ok();
        }

    }
}
