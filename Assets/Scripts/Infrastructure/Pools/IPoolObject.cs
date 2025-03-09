using R3;

namespace Infrastructure.Pools
{
    public interface IPoolObject
    {
        public void Reset();
        public Observable<Unit> Released { get; }
    }
}