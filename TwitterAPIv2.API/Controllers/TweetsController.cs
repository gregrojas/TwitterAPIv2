using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwitterAPIv2.Application.Queries;

namespace TwitterAPIv2.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TweetsController : Controller
    {
        private readonly IMediator _mediator;

        public TweetsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetSampleStream")]
        public async Task<IActionResult> GetSampleStream()
        {
            try
            {
                var result = await _mediator.Send(new GetSampleTweetsQuery());

                if (result.IsSuccess)
                    return Ok(result.Value);
                else
                    return BadRequest(result.Error.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.StackTrace);
            }
        }
    }
}