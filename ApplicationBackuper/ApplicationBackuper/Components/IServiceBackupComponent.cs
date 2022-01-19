using System.Threading.Tasks;

namespace ApplicationBackuper.Components
{
    public interface IServiceBackupComponent
    {
        Task Backup();
    }
}
