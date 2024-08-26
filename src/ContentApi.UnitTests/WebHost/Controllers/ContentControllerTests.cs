using AutoFixture;
using AutoFixture.AutoMoq;
using ContentApi.Core.Abstractions.Repositories;
using ContentApi.Core.Domain.Administration;
using ContentApi.WebHost.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using FluentAssertions;

namespace ContentApi.UnitTests.WebHost.Controllers;

public class ContentControllerTests
{
    private readonly Mock<IRepository<Content>> _contentRepositoryMock;
    private readonly ContentController _contentController;

    public ContentControllerTests()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        _contentRepositoryMock = fixture.Freeze<Mock<IRepository<Content>>>();
        _contentController = fixture.Build<ContentController>().OmitAutoProperties().Create();
    }

    [Fact]
    public async void GetFileInfoByIdAsync_Success()
    {
        // Arrange
        var fileId = "66955701d629af3f39849e9a";
        var content = new Content()
        {
            Id = new ObjectId("66955701d629af3f39849e9a"),
            Info = new GridFSFileInfo([]),
        };

        _contentRepositoryMock.Setup(repo => repo.GetFileInfoByIdAsync(fileId, It.IsAny<CancellationToken>())).ReturnsAsync(content);

        // Act
        var result = await _contentController.GetFileInfoByIdAsync(fileId, It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeAssignableTo<OkObjectResult>();
    }

    [Fact]
    public async void GetFileInfoByIdAsync_NotFound()
    {
        // Arrange
        var fileId = "86955711d629af3f39449e9b";
        var content = new Content()
        {
            Id = new ObjectId("66955701d629af3f39849e9a")
        };

        _contentRepositoryMock.Setup(repo => repo.GetFileInfoByIdAsync(fileId, It.IsAny<CancellationToken>())).ReturnsAsync(content);

        // Act
        var result = await _contentController.GetFileInfoByIdAsync(fileId, It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeAssignableTo<NotFoundObjectResult>();
    }
}
