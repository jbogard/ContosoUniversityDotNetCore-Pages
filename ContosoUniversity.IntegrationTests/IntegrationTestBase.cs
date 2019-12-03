using System.Threading.Tasks;
using Nito.AsyncEx;
using Xunit;

namespace ContosoUniversity.IntegrationTests
{
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        private static readonly AsyncLock _mutex = new AsyncLock();

        private static bool _initialized;

        public virtual async Task InitializeAsync()
        {
            if (_initialized)
                return;

            using (await _mutex.LockAsync())
            {
                if (_initialized)
                    return;
                
                await SliceFixture.ResetCheckpoint();

                _initialized = true;
            }
        }

        public virtual Task DisposeAsync() => Task.CompletedTask;
    }
}