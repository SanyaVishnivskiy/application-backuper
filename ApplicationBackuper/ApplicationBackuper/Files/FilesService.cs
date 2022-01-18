using System.IO;
using System.Threading.Tasks;

namespace ApplicationBackuper.Files
{
    public interface IFilesService
    {
        Task<Stream> Read(string path);
    }
    public class FilesService
    {
    }
}
