using ApplicationBackuper.Common;
using ApplicationBackuper.Composition;
using System;
using System.Threading;
using System.Threading.Tasks;

var logger = new ConsoleLogger();

try
{
    var tokenSource = new CancellationTokenSource();

    var composition = new CompositionRoot();
    composition.Initialize();

    while (!tokenSource.Token.IsCancellationRequested)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
catch (Exception e)
{
    logger.Log(e, "Critical error occured while execution");
}

