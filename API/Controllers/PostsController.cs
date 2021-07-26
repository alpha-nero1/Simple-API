using Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain;
using System;
using Application.Posts;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class PostsController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            return HandleResult(await Mediator.Send(new PostList.Query()));
        }

        // Get a single post.
        // Is protected.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            return HandleResult(await Mediator.Send(new PostDetails.Query { Id = id }));
        }

        // Because of ApiController attribute, dotnet knows to get the
        // post from the request body.
        // We could say ([FromBody] Post post) but not necessary.
        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            return HandleResult(await Mediator.Send(new PostCreate.Command { Post = post }));
        }

        [Authorize(Policy = "IsPostOwner")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPost(Guid id, Post post)
        {
            post.Id = id;
            return HandleResult(await Mediator.Send(new PostEdit.Command { Post = post }));
        }

        [Authorize(Policy = "IsPostOwner")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            return HandleResult(await Mediator.Send(new PostDelete.Command { Id = id }));
        }

        // Allows us to add/remove post users on a post or lets a post owner soft delete the post.
        [Authorize(Policy = "IsPostOwner")]
        [HttpPost("{id}/update")]
        public async Task<IActionResult> UpdatePost(Guid id)
        {
            return HandleResult(await Mediator.Send(new PostUpdate.Command { Id = id }));
        }
    }
}