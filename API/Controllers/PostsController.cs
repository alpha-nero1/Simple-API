using Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain;
using System.Collections.Generic;
using System;
using Application.Posts;


namespace API.Controllers
{
    public class PostsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts()
        {
            return await Mediator.Send(new PostList.Query());
        }

        // Get a single post.
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(Guid id)
        {
            return await Mediator.Send(new PostDetails.Query { Id = id });
        }

        // Because of ApiController attribute, dotnet knows to get the
        // post from the request body.
        // We could say ([FromBody] Post post) but not necessary.
        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            return Ok(await Mediator.Send(new PostCreate.Command { Post = post }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPost(Guid id, Post post)
        {
            post.Id = id;
            return Ok(await Mediator.Send(new PostEdit.Command { Post = post }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return Ok(await Mediator.Send(new PostDelete.Command { Id = id }));
        }
    }
}