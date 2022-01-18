using ApplicationBackuper.Commands;
using ApplicationBackuper.Common;
using ApplicationBackuper.Configuration;
using System;
using System.Threading.Tasks;

namespace ApplicationBackuper.Components
{
    public interface IBackupComponent
    {
        Task Backup();
    }
}
