using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace TestProgram
{
    public class MassTransitHostedService : IHostedService
    {
        private readonly IBusControl _busControl;

        public MassTransitHostedService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
            => await _busControl.StartAsync(cancellationToken);

        public async Task StopAsync(CancellationToken cancellationToken)
            => await _busControl.StopAsync(cancellationToken);
    }
}
