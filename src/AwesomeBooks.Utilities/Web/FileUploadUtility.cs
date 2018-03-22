using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace AwesomeBooks.Utilities.Web
{
    public class FileUploadUtility : IFileUploadUtility
    {
        public async Task<string> GetUploadFileContent(HttpRequest Request)
        {
            var boundary = GetBoundary(Request.ContentType);
            var reader = new MultipartReader(boundary, Request.Body, 512 * 1024);
            var valuesByKey = new Dictionary<string, string>();
            MultipartSection section;

            var result = new StringBuilder();
            while ((section = await reader.ReadNextSectionAsync()) != null)
            {
                var contentDispo = section.GetContentDispositionHeader();

                if (contentDispo.IsFileDisposition())
                {
                    var fileSection = section.AsFileSection();
                    var bufferSize = 128 * 1024;
                    var s = await ReadStream(fileSection.FileStream, bufferSize);
                    result.Append(s);
                }
                else if (contentDispo.IsFormDisposition())
                {
                    var formSection = section.AsFormDataSection();
                    var s = await formSection.GetValueAsync();
                    result.Append(s);
                }
            }
            return result.ToString();
        }

        private async Task<string> ReadStream(Stream stream, int bufferSize)
        {
            var buffer = new byte[bufferSize];

            int bytesRead;
            var content = new StringBuilder();
            do
            {
                bytesRead = await stream.ReadAsync(buffer, 0, bufferSize);
                if (bytesRead > 0)
                {
                    var readContent = Encoding.Default.GetString(buffer, 0, bytesRead);
                    content.Append(readContent);
                }

            } while (bytesRead > 0);

            var contentText = content.ToString();
            return contentText;
        }

        private string GetBoundary(string contentType)
        {
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var elements = contentType.Split(' ');
            var element = elements.First(entry => entry.StartsWith("boundary="));
            var boundary = element.Substring("boundary=".Length);

            boundary = HeaderUtilities.RemoveQuotes(boundary).ToString();

            return boundary;
        }
    }
}
