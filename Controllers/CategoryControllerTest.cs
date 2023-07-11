using AutoMapper;
using FakeItEasy;
using BlogEngineWebApp.Repository.Interfaces;
using BlogEngineWebApp.Models;
using BlogEngineWebApp.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using BlogEngineWebApp.Dto;

namespace BlogEngineWebApp.Tests.Controllers
{
    public class CategoryControllerTest
    {
        private CategoryController _categoryController;
        private ICategoryRepository _categoryRepository;
        private IMapper _mapper;
        public CategoryControllerTest()
        {
            _categoryRepository = A.Fake<ICategoryRepository>();
            _mapper = A.Fake<IMapper>();
            _categoryController = new CategoryController(_categoryRepository, _mapper);
        }

        [Fact]
        public void CategoryController_CreateCategory_ReturnsSuccess()
        {
            var categoryDto = A.Fake<CategoryDto>();
            var category = A.Fake<Category>();

            A.CallTo(() => _categoryRepository.IsUniqueTitle(categoryDto.Title)).Returns(0);
            A.CallTo(() => _mapper.Map<Category>(categoryDto)).Returns(category);
            A.CallTo(() => _categoryRepository.CreateCategory(category)).Returns(true);
            
            var result = _categoryController.Add(categoryDto);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void CategoryController_CreateCategory_WhenTitleNotUnique_ReturnsSuccess()
        {
            var categoryDto = A.Fake<CategoryDto>();
            var category = A.Fake<Category>();

            A.CallTo(() => _categoryRepository.IsUniqueTitle(categoryDto.Title)).Returns(1);

            var result = _categoryController.Add(categoryDto);
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public void CategoryController_UpdateCategory_ReturnsSuccess()
        {
            var categoryDtoUpdate = new CategoryDto { CategoryId = 1, Title = "The Ok Tilte" };
            var categoryUpdate = new Category { CategoryId = 1, Title = "The Ok Title", Posts = null };

            A.CallTo(() => _categoryRepository.CategoryExists(categoryDtoUpdate.CategoryId)).Returns(true);
            A.CallTo(() => _categoryRepository.IsUniqueTitle(categoryDtoUpdate.Title)).Returns(1);
            A.CallTo(() => _mapper.Map<Category>(categoryDtoUpdate)).Returns(categoryUpdate);
            A.CallTo(() => _categoryRepository.UpdateCategory(categoryUpdate)).Returns(true);

            var result = _categoryController.Update(categoryDtoUpdate);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void CategoryController_UpdateCategory_WhenMoreThan1Title_ReturnsBadRequest()
        {
            var categoryDtoUpdate = new CategoryDto { CategoryId = 1, Title = "The Ok Tilte" };
            var categoryUpdate = new Category { CategoryId = 1, Title = "The Ok Title", Posts = null };

            A.CallTo(() => _categoryRepository.CategoryExists(categoryDtoUpdate.CategoryId)).Returns(true);
            A.CallTo(() => _categoryRepository.IsUniqueTitle(categoryDtoUpdate.Title)).Returns(2);

            var result = _categoryController.Update(categoryDtoUpdate);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void CategoryController_GetCategories_ReturnsSuccess()
        {
            var categorieDto = new CategoryDto
            {
                CategoryId = 1,
                Title = "Title"
            };

            var category = new Category
            {
                CategoryId = 1,
                Title = "Title",
                Posts = null
            };

            var categories = new List<Category>() { category };
            var categoriesDto = new List<CategoryDto>
            {
                categorieDto
            };

            A.CallTo(() => _categoryRepository.GetCategories()).Returns(categories);
            A.CallTo(() => _mapper.Map<List<CategoryDto>>(categories)).Returns(categoriesDto);

            var result = _categoryController.GetCategories();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void CategoryController_GetCategories_WhenNoCategory_ReturnsNoContent()
        {            
            var categories = A.Fake<ICollection<Category>>();
            A.CallTo(() => _categoryRepository.GetCategories()).Returns(categories);
           
            var result = _categoryController.GetCategories();
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void CategoryController_GetCategoryById_ReturnsSuccess()
        {
            int id = 1;
            var category = A.Fake<Category>();
            var categoryDto = A.Fake<CategoryDto>();

            A.CallTo(() => _categoryRepository.CategoryExists(id)).Returns(true);
            A.CallTo(() => _mapper.Map <CategoryDto>(category)).Returns(categoryDto);
            A.CallTo(() => _categoryRepository.GetCategoryById(id)).Returns(category);

            var result = _categoryController.GetCategoryById(id);
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void CategoryController_GetCategoryById_WhenIdNotExists_ReturnsNotFound()
        {
            int id = 1;

            A.CallTo(() => _categoryRepository.CategoryExists(id)).Returns(false);

            var result = _categoryController.GetCategoryById(id);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void CategoryController_GetPostByCategoryById_ReturnSuccess()
        {
            int id = 1;
            var post = A.Fake<PostDto>();
            var postList = new List<PostDto>() { post };
            var postCollection = A.Fake<ICollection<Post>>();
            
            A.CallTo(() => _categoryRepository.CategoryExists(id)).Returns(true);
            A.CallTo(() => _categoryRepository.GetPostsByCategoryId(id)).Returns(postCollection);
            A.CallTo(() => _mapper.Map<List<PostDto>>(postCollection)).Returns(postList);

            var result = _categoryController.GetPostByCategoryId(id);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void CategoryController_GetPostCategoryById_WhenIdNotExists_ReturnNotFound()
        {
            int id = 1;

            A.CallTo(() => _categoryRepository.CategoryExists(id)).Returns(false);

            var result = _categoryController.GetPostByCategoryId(id);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void CategoryController_GetPostByCategoryId_WhenNoPost_ReturnsNoContent()
        {
            int id = 1;
            var post = A.Fake<ICollection<Post>>();

            A.CallTo(() => _categoryRepository.CategoryExists(id)).Returns(true);
            A.CallTo(() => _categoryRepository.GetPostsByCategoryId(id)).Returns(post);

            var result = _categoryController.GetPostByCategoryId(id);
            result.Should().BeOfType<NoContentResult>();
        }


    }
}