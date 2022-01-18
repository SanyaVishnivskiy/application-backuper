using System.IO;
using System.Threading.Tasks;

namespace ApplicationBackuper.Files
{
    public interface IFilesArchiver
    {
        Task<Stream> Archive(Stream files);
    }

    public class FilesArchiver
    {
    }
}
