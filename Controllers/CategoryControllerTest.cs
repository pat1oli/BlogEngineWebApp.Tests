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
        public void CategoryController_GetCategories_ReturnsSuccess()
        {
            //
            var categorieDto = new CategoryDto{ 
                CategoryId = 1, 
                Title = "Title"
            };
            var categories = A.Fake<ICollection<Category>>();
            var categoriesDto = new List<CategoryDto>
            {
                categorieDto
            };

            A.CallTo(() => _categoryRepository.GetCategories()).Returns(categories);
            A.CallTo(() => _mapper.Map<List<CategoryDto>>(categories)).Returns(categoriesDto);

            var result = _categoryController.GetCategories();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void CategoryController_CreateCategory_ReturnsSuccess()
        {
            //
            var categoryDto = A.Fake<CategoryDto>();
            var category = A.Fake<Category>();

            A.CallTo(() => _categoryRepository.IsUniqueTitle(categoryDto.Title)).Returns(0);
            A.CallTo(() => _mapper.Map<Category>(categoryDto)).Returns(category);
            A.CallTo(() => _categoryRepository.CreateCategory(category)).Returns(true);
            
            var result = _categoryController.Add(categoryDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();

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
        public void CategoryController_UpdateCategory_ReturnsSuccess()
        {
            var categoryDtoUpdate = new CategoryDto { CategoryId = 1, Title= "The Ok Tilte"};
            var categoryUpdate = new Category { CategoryId = 1, Title= "The Ok Title", Posts = null};

            A.CallTo(() => _categoryRepository.CategoryExists(categoryDtoUpdate.CategoryId)).Returns(true);
            A.CallTo(() => _categoryRepository.IsUniqueTitle(categoryDtoUpdate.Title)).Returns(1);
            A.CallTo(() => _mapper.Map<Category>(categoryDtoUpdate)).Returns(categoryUpdate);
            A.CallTo(() => _categoryRepository.UpdateCategory(categoryUpdate)).Returns(true);

            var result = _categoryController.Update(categoryDtoUpdate);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }




    }
}