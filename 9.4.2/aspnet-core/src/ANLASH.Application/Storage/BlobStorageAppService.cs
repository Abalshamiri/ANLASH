using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using ANLASH.Storage.Dto;
using Microsoft.EntityFrameworkCore;

namespace ANLASH.Storage
{
    /// <summary>
    /// Blob Storage Application Service
    /// خدمة تطبيق تخزين الملفات
    /// </summary>
    public class BlobStorageAppService : ApplicationService, IBlobStorageAppService
    {
        private readonly IRepository<AppBinaryObject, Guid> _binaryObjectRepository;

        // File size limits (in bytes)
        private const long MaxImageSize = 5 * 1024 * 1024; // 5 MB
        private const long MaxDocumentSize = 10 * 1024 * 1024; // 10 MB

        // Allowed file types
        private static readonly string[] AllowedImageTypes = { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        private static readonly string[] AllowedDocumentTypes = { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };

        public BlobStorageAppService(IRepository<AppBinaryObject, Guid> binaryObjectRepository)
        {
            _binaryObjectRepository = binaryObjectRepository;
        }

        /// <summary>
        /// Upload file
        /// </summary>
        public async Task<FileDto> UploadAsync(UploadFileInput input)
        {
            // Validate input
            ValidateFile(input);

            // Convert base64 string to byte array
            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(input.FileBytes);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Invalid file data: {ex.Message}");
            }

            // Create binary object
            var binaryObject = new AppBinaryObject
            {
                TenantId = AbpSession.TenantId,
                FileName = input.FileName,
                ContentType = input.ContentType,
                FileSize = fileBytes.Length,
                Bytes = fileBytes,
                Description = input.Description,
                Category = input.Category,
                EntityType = input.EntityType,
                EntityId = input.EntityId
            };

            // If image, extract dimensions
            if (IsImage(input.ContentType))
            {
                try
                {
                    var dimensions = GetImageDimensions(fileBytes);
                    binaryObject.Width = dimensions.width;
                    binaryObject.Height = dimensions.height;
                }
                catch
                {
                    // Ignore dimension extraction errors
                }
            }

            // Save to database
            var id = await _binaryObjectRepository.InsertAndGetIdAsync(binaryObject);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Return file info
            return ObjectMapper.Map<FileDto>(binaryObject);
        }

        /// <summary>
        /// Download file
        /// </summary>
        public async Task<DownloadFileOutput> DownloadAsync(Guid id)
        {
            var binaryObject = await _binaryObjectRepository.GetAsync(id);

            return new DownloadFileOutput
            {
                FileBytes = binaryObject.Bytes,
                FileName = binaryObject.FileName,
                ContentType = binaryObject.ContentType
            };
        }

        /// <summary>
        /// Get file info
        /// </summary>
        public async Task<FileDto> GetFileInfoAsync(Guid id)
        {
            var binaryObject = await _binaryObjectRepository.GetAsync(id);
            return ObjectMapper.Map<FileDto>(binaryObject);
        }

        /// <summary>
        /// Delete file
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            await _binaryObjectRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Get files by entity
        /// </summary>
        public async Task<List<FileDto>> GetFilesByEntityAsync(string entityType, long entityId)
        {
            var files = await _binaryObjectRepository.GetAll()
                .Where(f => f.EntityType == entityType && f.EntityId == entityId)
                .OrderByDescending(f => f.CreationTime)
                .ToListAsync();

            return ObjectMapper.Map<List<FileDto>>(files);
        }

        /// <summary>
        /// Get files by category
        /// </summary>
        public async Task<List<FileDto>> GetFilesByCategoryAsync(string category)
        {
            var files = await _binaryObjectRepository.GetAll()
                .Where(f => f.Category == category)
                .OrderByDescending(f => f.CreationTime)
                .ToListAsync();

            return ObjectMapper.Map<List<FileDto>>(files);
        }

        #region Private Methods

        private void ValidateFile(UploadFileInput input)
        {
            if (input == null)
                throw new UserFriendlyException("File input is required");

            if (string.IsNullOrWhiteSpace(input.FileBytes))
                throw new UserFriendlyException("File is empty");

            if (string.IsNullOrWhiteSpace(input.FileName))
                throw new UserFriendlyException("File name is required");

            if (string.IsNullOrWhiteSpace(input.ContentType))
                throw new UserFriendlyException("Content type is required");

            // Convert base64 to get actual size
            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(input.FileBytes);
            }
            catch
            {
                throw new UserFriendlyException("Invalid file data");
            }

            // Validate file type and size
            if (IsImage(input.ContentType))
            {
                if (!AllowedImageTypes.Contains(input.ContentType.ToLower()))
                    throw new UserFriendlyException($"Image type '{input.ContentType}' is not allowed");

                if (fileBytes.Length > MaxImageSize)
                    throw new UserFriendlyException($"Image size exceeds maximum allowed size of {MaxImageSize / 1024 / 1024}MB");
            }
            else if (IsDocument(input.ContentType))
            {
                if (!AllowedDocumentTypes.Contains(input.ContentType.ToLower()))
                    throw new UserFriendlyException($"Document type '{input.ContentType}' is not allowed");

                if (fileBytes.Length > MaxDocumentSize)
                    throw new UserFriendlyException($"Document size exceeds maximum allowed size of {MaxDocumentSize / 1024 / 1024}MB");
            }
            else
            {
                throw new UserFriendlyException($"File type '{input.ContentType}' is not supported");
            }
        }

        private bool IsImage(string contentType)
        {
            return contentType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true;
        }

        private bool IsDocument(string contentType)
        {
            return contentType?.StartsWith("application/", StringComparison.OrdinalIgnoreCase) == true;
        }

        private (int width, int height) GetImageDimensions(byte[] imageBytes)
        {
            // Simple implementation - could be enhanced with ImageSharp
            // For now, return default values
            return (0, 0);
        }

        #endregion
    }
}
