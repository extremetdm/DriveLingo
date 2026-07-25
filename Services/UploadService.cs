using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI.WebControls;

namespace DriveLingo.Services
{
    public static class UploadService
    {
        public struct StatusOutput
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string FilePath { get; set; }
        }
        public static StatusOutput UploadImage(FileUpload file)
        {
            if (file == null || !file.HasFile)
            {
                return new StatusOutput
                {
                    Success = false,
                    Message = "No file was selected for upload."
                };
            }

            try
            {
                string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".svg" };

                string originalFileName = Path.GetFileName(file.FileName); // Strip client path info
                string extension = Path.GetExtension(originalFileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    throw new InvalidOperationException("Invalid file type uploaded.");
                }

                string uploadsDir = HostingEnvironment.MapPath("~/uploads/");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                string safeFileName = "image_" + Guid.NewGuid().ToString("N").Substring(0, 8) + extension;

                string fullPath = Path.GetFullPath(Path.Combine(uploadsDir, safeFileName));

                string canonicalUploadsDir = Path.GetFullPath(uploadsDir);
                if (!canonicalUploadsDir.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    canonicalUploadsDir += Path.DirectorySeparatorChar;
                }

                if (!fullPath.StartsWith(canonicalUploadsDir, StringComparison.OrdinalIgnoreCase))
                {
                    throw new System.Security.SecurityException("Path traversal attempt detected!");
                }

                // 7. Save file safely
                file.SaveAs(fullPath);

                return new StatusOutput
                {
                    Success = true,
                    Message = "Image uploaded successfully.",
                    FilePath = "/uploads/" + safeFileName
                };
            }
            catch (Exception ex)
            {
                return new StatusOutput
                {
                    Success = false,
                    Message = "Image upload error: " + ex.Message
                };
            }
        }
    }
}