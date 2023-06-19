using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace AspNetSamples.Business.Tests
{
    public class ArticleServiceTest
    {
        private readonly Mock<IUnitOfWork> _uowMock = new Mock<IUnitOfWork>();
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<ISourceService> _sourceServiceMock = new Mock<ISourceService>();
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();


        private ArticleService CreateService()
        {

            var articleService = new ArticleService(
                _uowMock.Object,
                _mapperMock.Object,
                _configMock.Object,
                _sourceServiceMock.Object,
                _mediatorMock.Object);

            return articleService;
        }


        [Fact]
        public async Task GetTotalArticlesCountAsync_CorrectData_CorrectCount()
        {

            //arrange
           _uowMock.Setup(uow
                    => uow.Articles.CountAsync())
                .ReturnsAsync(2);

           var articleService = CreateService();

            //act
            var result = await articleService.GetTotalArticlesCountAsync();

            //assert
            Assert.Equal(2, result);
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 15)]
        [InlineData(10, 15000)]
        public async Task GetArticlesByPageAsync_WithCorrectData_ReturnCorrectPage(int page, int pageSize)
        {
           _mapperMock.Setup(mapper => mapper.Map<ArticleDto>(It.IsAny<Article>()))
                .Returns(() => new ArticleDto());

            _uowMock.Setup(uow => uow.Articles.GetArticlesByPageAsync(It.IsAny<int>(),
                It.IsAny<int>())).ReturnsAsync(() => new List<Article>
            {
                new Article(),
                new Article()
            });

            var articleService = CreateService();

            //act
            var data = await articleService.GetArticlesByPageAsync(page, pageSize);

            //assert 
            Assert.Equal(2, data.Count);

        }


        [Theory]
        [InlineData(0, -2)]
        [InlineData(-2, 15)]
        [InlineData(-10, -10)]
        public async Task GetArticlesByPageAsync_WithIncorrectPageAndPageSize_ReturnCorrectError(int page, int pageSize)
        {
            var uowMock = new Mock<IUnitOfWork>();
            var configMock = new Mock<IConfiguration>();
            var mapperMock = new Mock<IMapper>();
            var sourceServiceMock = new Mock<ISourceService>();
            var mediatorMock = new Mock<IMediator>();
            //var loggerMock = new Mock<ILogger>();

            var articleService = new ArticleService(
                uowMock.Object,
                mapperMock.Object,
                configMock.Object,
                sourceServiceMock.Object,
                mediatorMock.Object);

            //act
            var result = async () => await articleService.GetArticlesByPageAsync(page, pageSize);

            //assert 
            await Assert.ThrowsAnyAsync<ArgumentException>(result);

        }


        [Fact]
        public async Task AddAsync_CorrectData_InsertedCorrectly()
        {
            var insertedArticleDto = new ArticleDto();
            var list = new List<Article>();

            _mapperMock.Setup(mapper => mapper.Map<Article>(It.IsAny<ArticleDto>()))
                .Returns(() => new Article());

        }
    }
}