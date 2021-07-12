using Domain;
using FluentValidation;

namespace Application.Posts
{
    // Custom validator for the post class.
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Topic).NotEmpty();
        }
    }
}