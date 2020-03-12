using MassTransit;
using System.Threading.Tasks;

namespace TestProgram
{
    public class TestConsumer : IConsumer<TestMessage>
    {
        public Task Consume(ConsumeContext<TestMessage> context)
        {
            return Task.CompletedTask;
        }
    }
}
