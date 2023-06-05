using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AspNetSamples.WebAPI.Requests;
using AspNetSamples.WebAPI.Responses;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetSamples.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with Article resources
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;
        
        public ArticleController(IArticleService articleService, 
            IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
        }


        [HttpGet]
        [Route("article-get")]
        [ProducesResponseType(typeof(ArticleResponse[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetData()
        {
          
            return Ok();
        }

        /// <summary>
        /// Get Collection of articles by page size and page number
        /// </summary>
        /// <param name="request">Model which contains PageSize & Page number </param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ArticleResponse[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] GetArticlesByPagingInfoRequest request)
        {
            var articles = (await _articleService
                .GetArticlesByPageAsync(request.Page, request.PageSize))
                .Select(dto => _mapper.Map<ArticleResponse>(dto)).ToArray();
            return Ok(articles);
        }

        // GET api/<ArticleController>/5
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ArticleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var article = await _articleService.GetArticleByIdWithSourceNameAsync(id);
                if (article == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<ArticleResponse>(article));
            }
            catch (Exception ex)
            { 
                //todo log
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        // POST api/<ArticleController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrUpdateArticleRequest request)
        {
            var articleDto = _mapper.Map<ArticleDto>(request);
            await _articleService.AddAsync(articleDto);
            //response should contain id of created resource
            return Created("/Article/1", null);
        }

        // PUT api/<ArticleController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CreateOrUpdateArticleRequest request)
        {
            var articleDto = await _articleService.GetArticleByIdWithSourceNameAsync(id);
            //execute method with replace all fields of resource or create that resource 

            if (articleDto == null)
            {
                await _articleService.AddAsync(_mapper.Map<ArticleDto>(request));
            }
            return Ok();
        }

        // PUT api/<ArticleController>/5
        [HttpPatch("{id}")]
        public void Patch(int id, [FromBody] string value)
        {
        }




        // DELETE api/<ArticleController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //await _articleService.DeleteByIdAsync(id);
            return Ok();
        }
    }
}
