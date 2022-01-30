using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationBackuper.Commands
{
    public interface ICommandsExecutor
    {
        Task<int> Execute(List<string> commands);
    }
}
