using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DocumentApi.Models;
using DocumentApi.Services.Interfaces;
using DocumentApi.Controllers;

namespace DocumentApiTests.Controllers;

[TestFixture]
public class DocumentsControllerTests
{
    private DocumentsController _controller;
    private Mock<IFileStorageService> _fileStorageServiceMock;
    private Mock<IDocumentService> _documentServiceMock;

    [SetUp]
    public void SetUp()
    {
        _fileStorageServiceMock = new Mock<IFileStorageService>();
        _documentServiceMock = new Mock<IDocumentService>();
        _controller = new DocumentsController(_fileStorageServiceMock.Object, _documentServiceMock.Object);
    }

    #region UploadDocument Tests

    [Test]
    public async Task UploadDocument_FileIsNull_ReturnsBadRequest()
    {
        // Arrange
        IFormFile file = null;

        // Act
        var result = await _controller.UploadDocument(file);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var badRequest = result as BadRequestObjectResult;
        Assert.That(badRequest, Is.Not.Null);
        Assert.That(badRequest.Value, Is.EqualTo("No file uploaded."));
    }

    [Test]
    public async Task UploadDocument_FileIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(0);

        // Act
        var result = await _controller.UploadDocument(fileMock.Object);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequest = result as BadRequestObjectResult;
        Assert.That(badRequest, Is.Not.Null);
        Assert.That(badRequest.Value, Is.EqualTo("No file uploaded."));
    }

    [Test]
    public async Task UploadDocument_ValidFile_ReturnsCreatedAtAction()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.pdf");
        fileMock.Setup(f => f.Length).Returns(1024);
        fileMock.Setup(f => f.ContentType).Returns("application/pdf");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        _fileStorageServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync("path/to/file");

        var document = new Document { Id = "123", Name = "test.pdf", StoragePath = "path/to/file" };
        _documentServiceMock.Setup(s => s.CreateDocumentAsync(It.IsAny<Document>()))
            .ReturnsAsync(document);

        // Act
        var result = await _controller.UploadDocument(fileMock.Object);

        // Assert
        Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.ActionName, Is.EqualTo("GetDocumentById"));
            Assert.That(createdResult.RouteValues["id"], Is.EqualTo("123"));
        });
    }

    [Test]
    public async Task UploadDocument_ServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.pdf");
        fileMock.Setup(f => f.Length).Returns(1024);
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        _fileStorageServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ThrowsAsync(new Exception("Upload error"));

        // Act
        var result = await _controller.UploadDocument(fileMock.Object);

        // Assert
        Assert.IsInstanceOf<ObjectResult>(result);
        var objectResult = result as ObjectResult;
        Assert.That(objectResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(objectResult.Value.ToString(), Does.Contain("Error uploading file: Upload error"));
        });
    }

    #endregion

    #region UpdateDocument Tests

    [Test]
    public async Task UpdateDocument_FileIsNull_ReturnsBadRequest()
    {
        // Arrange
        string id = "123";
        IFormFile file = null;

        // Act
        var result = await _controller.UpdateDocument(id, file);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequest = result as BadRequestObjectResult;
        Assert.That(badRequest, Is.Not.Null);
        Assert.That(badRequest.Value, Is.EqualTo("No file uploaded."));
    }

    [Test]
    public async Task UpdateDocument_FileIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        string id = "123";
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(0);

        // Act
        var result = await _controller.UpdateDocument(id, fileMock.Object);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequest = result as BadRequestObjectResult;
        Assert.That(badRequest, Is.Not.Null);
        Assert.That(badRequest.Value, Is.EqualTo("No file uploaded."));
    }

    [Test]
    public async Task UpdateDocument_ValidFile_ReturnsOk()
    {
        // Arrange
        string id = "123";
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.pdf");
        fileMock.Setup(f => f.Length).Returns(1024);
        fileMock.Setup(f => f.ContentType).Returns("application/pdf");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        _fileStorageServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync("path/to/updated/file");

        var document = new Document { Id = "123", Name = "test.pdf", StoragePath = "path/to/updated/file" };
        _documentServiceMock.Setup(s => s.UpdateDocumentAsync(It.IsAny<string>(), It.IsAny<Document>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateDocument(id, fileMock.Object);

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    [Test]
    public async Task UpdateDocument_ServiceThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        string id = "123";
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.pdf");
        fileMock.Setup(f => f.Length).Returns(1024);
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        _fileStorageServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<string>(), It.IsAny<Stream>()))
            .ThrowsAsync(new Exception("Update error"));

        // Act
        var result = await _controller.UpdateDocument(id, fileMock.Object);

        // Assert
        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(objectResult.Value.ToString().Contains("Error updating file: Update error"), Is.True);
        });
    }

    #endregion

    #region GetDocumentById Tests

    [Test]
    public async Task GetDocumentById_DocumentNotFound_ReturnsNotFound()
    {
        // Arrange
        string id = "123";
        _documentServiceMock.Setup(s => s.GetDocumentByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Document)null);

        // Act
        var result = await _controller.GetDocumentById(id);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task GetDocumentById_DocumentExists_ReturnsOk()
    {
        // Arrange
        string id = "123";
        var document = new Document { Id = "123", Name = "test.pdf", StoragePath = "path/to/file" };
        _documentServiceMock.Setup(s => s.GetDocumentByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(document);

        // Act
        var result = await _controller.GetDocumentById(id);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.Value, Is.EqualTo(document));
    }

    #endregion
}
