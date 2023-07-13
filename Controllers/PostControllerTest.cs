using AutoMapper;
using BlogEngineWebApp.Repository.Interfaces;
using BlogEngineWebApp.Controllers;
using BlogEngineWebApp.Models;
using BlogEngineWebApp.Dto;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using BlogEngineWebApp.Tests.Helper;

namespace BlogEngineWebApp.Tests.Controllers
{

    public class PostControllerTest
    {
        private PostController _postController;
        private IPostRepository _postRepository;
        private ICategoryRepository _categoryRepository;
        private IMapper _mapper;
        private Post _post;
        private PostDto _postDto;

        public PostControllerTest()
        {
            _postRepository = A.Fake<IPostRepository>();
            _categoryRepository = A.Fake<ICategoryRepository>();
            _mapper = A.Fake<IMapper>();
            _postController = new PostController(_postRepository, _mapper, _categoryRepository);
            _post = Utility.CreateCustomPost();
            _postDto = Utility.CreateCustomPostDto();
        }

        [Fact]
        public void PostController_AddPost_ReturnOk()
        {

            A.CallTo(() => _mapper.Map<Post>(_postDto)).Returns(_post);
            A.CallTo(() => _postRepository.IsUniqueTitle(_postDto.Title)).Returns(0);
            A.CallTo(() => _postRepository.CreatePost(_post)).Returns(true);

            var result = _postController.Add(_postDto);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void PostController_GetPosts_ReturnOk()
        {
            var listPost = new List<Post>() { _post};
            var listPostDto = new List<PostDto>() { _postDto };

            A.CallTo(() => _postRepository.GetPosts()).Returns(listPost);
            A.CallTo(() => _mapper.Map<List<PostDto>>(_postRepository.GetPosts())).Returns(listPostDto);

            var result = _postController.GetPosts();
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void PostController_GetPosts_WhenNoPostFound_ReturnNoContent()
        {
            A.CallTo(() => _mapper.Map<List<PostDto>>(_postRepository.GetPosts())).Returns(null);

            var result = _postController.GetPosts();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void PostController_GetPostById_ReturnOk()
        {
            int id = 1;

            A.CallTo(() => _postRepository.PostExists(id)).Returns(true);
            A.CallTo(() => _postRepository.GetPostById(id)).Returns(_post);

            var result = _postController.GetPostById(id);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void PostController_GetPostById_WhenIdNotFound_ReturnNoContent()
        {
            int id = 1;

            A.CallTo(() => _postRepository.PostExists(id)).Returns(false);

            var result = _postController.GetPostById(id);
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
