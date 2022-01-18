using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationBackuper.Commands
{
    public interface ICommandsExecutor
    {
        Task Execute(List<string> commands);
    }
}
