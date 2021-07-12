using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Application.Core;


namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    
        // Standardise the way in which we return results to clients!
        protected ActionResult HandleResult<TResult>(HandlerResult<TResult> res)
        {
            if (res == null) return NotFound();
            if (res.IsSuccess && res.Value != null) return Ok(res.Value);
            if (res.IsSuccess && res.Value == null) return NotFound();
            return BadRequest(res.Error);
        }
    }
}