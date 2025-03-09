using System.Collections.Generic;
using Gameplay.Monster;

namespace Gameplay
{
    public interface ITargetsContainer
    {
        public IReadOnlyCollection<ITarget> Targets { get; }
        public void Add(ITarget target);
        public void Remove(ITarget target);
    }
}