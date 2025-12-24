using System.IO;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using ANLASH.Storage;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace ANLASH.Tests.Storage
{
    public class LocalFileStorageService_Tests : ANLASHTestBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ITestOutputHelper _output;

        public LocalFileStorageService_Tests(ITestOutputHelper output)
        {
            _output = output;
            _hostEnvironment = Substitute.For<IHostEnvironment>();
            _hostEnvironment.ContentRootPath.Returns(Directory.GetCurrentDirectory());
            
            _fileStorageService = new LocalFileStorageService(_hostEnvironment);
        }

        [Fact]
        public async Task Should_Save_File_Successfully()
        {
            // Arrange
            var content = "Test file content";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            var fileName = "test.txt";
            var folder = "profiles";

            // Act
            var filePath = await _fileStorageService.SaveFileAsync(stream, fileName, folder);

            // Assert
            filePath.ShouldNotBeNullOrEmpty();
            filePath.ShouldContain("uploads");
            filePath.ShouldContain(folder);

            // Cleanup
            if (File.Exists(Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", filePath)))
            {
                File.Delete(Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", filePath));
            }
        }

        [Fact]
        public async Task Should_Throw_Exception_When_File_Is_Empty()
        {
            // Arrange
            var emptyStream = new MemoryStream();
            _output.WriteLine($"Empty stream Length: {emptyStream.Length}");
            _output.WriteLine($"Empty stream CanRead: {emptyStream.CanRead}");
            
            var fileName = "test.txt";
            var folder = "profiles";

            // Act & Assert
            var exception = await Record.ExceptionAsync(async () =>
            {
                _output.WriteLine("About to call SaveFileAsync with empty stream");
                await _fileStorageService.SaveFileAsync(emptyStream, fileName, folder);
                _output.WriteLine("SaveFileAsync completed without exception!");
            });

            _output.WriteLine($"Exception caught: {exception?.GetType().Name ?? "NULL"}");
            _output.WriteLine($"Exception message: {exception?.Message ?? "N/A"}");

            // Assert
            exception.ShouldNotBeNull("Expected an exception for empty file");
            exception.ShouldBeOfType<UserFriendlyException>();
        }

        [Fact]
        public async Task Should_Throw_Exception_When_FileName_Is_Invalid()
        {
            // Arrange
            var content = "Test content";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            var fileName = ""; // Invalid
            var folder = "profiles";

            _output.WriteLine($"Testing with empty fileName");

            // Act & Assert
            var exception = await Record.ExceptionAsync(async () =>
            {
                _output.WriteLine("About to call SaveFileAsync with empty filename");
                await _fileStorageService.SaveFileAsync(stream, fileName, folder);
                _output.WriteLine("SaveFileAsync completed without exception!");
            });

            _output.WriteLine($"Exception caught: {exception?.GetType().Name ?? "NULL"}");
            _output.WriteLine($"Exception message: {exception?.Message ?? "N/A"}");

            // Assert
            exception.ShouldNotBeNull("Expected an exception for invalid filename");
            exception.ShouldBeOfType<UserFriendlyException>();
        }

        [Fact]
        public async Task Should_Prevent_Path_Traversal_Attack()
        {
            // Arrange
            var content = "Malicious content";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            var fileName = "../../malicious.txt";  // Path traversal attempt
            var folder = "profiles";

            // Act
            var filePath = await _fileStorageService.SaveFileAsync(stream, fileName, folder);

            // Assert
            filePath.ShouldNotContain("..");
            filePath.ShouldContain("uploads");

            // Cleanup
            if (File.Exists(Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", filePath)))
            {
                File.Delete(Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", filePath));
            }
        }

        [Fact]
        public async Task Should_Check_File_Exists()
        {
            // Arrange - Create a test file first
            var content = "Test content";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            var savedPath = await _fileStorageService.SaveFileAsync(stream, "test.txt", "profiles");

            // Act
            var exists = await _fileStorageService.FileExistsAsync(savedPath);

            // Assert
            exists.ShouldBeTrue();

            // Cleanup
            await _fileStorageService.DeleteFileAsync(savedPath);
        }

        [Fact]
        public async Task Should_Delete_File_Successfully()
        {
            // Arrange - Create a test file first
            var content = "Test content";
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            var savedPath = await _fileStorageService.SaveFileAsync(stream, "test.txt", "profiles");

            // Act
            await _fileStorageService.DeleteFileAsync(savedPath);

            // Assert
            var exists = await _fileStorageService.FileExistsAsync(savedPath);
            exists.ShouldBeFalse();
        }
    }
}
