﻿using System.Threading.Tasks;

namespace ApplicationBackuper.Components
{
    public interface IBackupComponent
    {
        Task Backup();
    }
}
