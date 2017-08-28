using System.Threading.Tasks;
using Xunit;

namespace ContosoUniversity.IntegrationTests
{
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        public virtual Task InitializeAsync() => SliceFixture.ResetCheckpoint();

        public virtual Task DisposeAsync() => Task.FromResult(0);
    }
}