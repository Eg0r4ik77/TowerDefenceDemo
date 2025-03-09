using VContainer;
using VContainer.Unity;

namespace Gameplay
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<TargetsContainer>(Lifetime.Singleton).As<ITargetsContainer>();
        }
    }
}