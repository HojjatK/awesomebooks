using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AwesomeBooks.Utilities.Web
{
    public interface IFileUploadUtility
    {
        Task<string> GetUploadFileContent(HttpRequest Request);
    }
}
