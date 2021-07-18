using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        // Seed our application with some data.
        // The user manager from .Identity will provide us heaps of functionality for user 'managing'.
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            // Don't seed users if we have some already.
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Ale",
                        UserName = "ale",
                        Email = "ale@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Bob",
                        UserName = "bob",
                        Email = "bob@test.com"
                    },
                    new AppUser
                    {
                        DisplayName = "Stevo",
                        UserName = "stevo",
                        Email = "stevo@test.com"
                    },
                };
                foreach (var user in users)
                {
                    // Will create AND save.
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
            // Do not insert twice.
            if (context.Posts.Any()) return;

            var posts = new List<Post>
            {
                new Post
                {
                    Title = "Test Post 1",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 1",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 2",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 2",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 3",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 3",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 4",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 4",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 5",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 5",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 6",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 6",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 7",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 7",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 8",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 8",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 9",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 9",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 10",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 10",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 11",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 11",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 12",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 12",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 13",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 13",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 14",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 14",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 15",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 15",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 16",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 16",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 17",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 17",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 18",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 18",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 19",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 19",
                    Topic = "Test"
                },
                new Post
                {
                    Title = "Test Post 20",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Test Post Description 20",
                    Topic = "Test"
                }
            };

            // Add the list to the current db set.
            await context.Posts.AddRangeAsync(posts);
            // Save changes!
            await context.SaveChangesAsync();
        }
    }
}