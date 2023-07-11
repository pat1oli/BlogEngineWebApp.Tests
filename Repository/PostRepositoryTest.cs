using BlogEngineWebApp.Data;
using BlogEngineWebApp.Models;
using BlogEngineWebApp.Repository;
using BlogEngineWebApp.Tests.Helper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace BlogEngineWebApp.Tests.Repository
{
    public class PostRepositoryTest
    {
    
        [Fact]
        public void PostRepository_GetPosts_ReturnList()
        {
            var context = GetApplicationDbContext();
            var repository = new PostRepository(context);

            var result = repository.GetPosts();
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Post>>();
        }

        [Fact]
        public void PostRepository_GetPostById_ReturnPost()
        {
            var context = GetApplicationDbContext();
            var repository = new PostRepository(context);
            int index = 1;

            var result = repository.GetPostById(index);
            result.Should().NotBeNull();
            result.Should().BeOfType<Post>();
        }

        private ApplicationDbContext GetApplicationDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName : "PostDbTest")
                .Options;
          
            var databaseContext = new ApplicationDbContext(options);

            databaseContext.Database.EnsureCreated();

            if (databaseContext.Posts.Count() <= 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    databaseContext.Posts.Add(
                    new Post()
                    {
                        Title = "Post "+ i,
                        Content = "Content "+ i,
                        PublicationDate = new DateTime(2023, 11, 07),
                        Category = new Category () { Title = "Categorie" + i},
                                          
                    });
                    databaseContext.SaveChanges();
                }
            }
            return databaseContext;
        }
    }
}