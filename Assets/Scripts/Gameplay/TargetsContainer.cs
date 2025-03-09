using System.Collections.Generic;
using Gameplay.Monster;

namespace Gameplay
{
    public class TargetsContainer : ITargetsContainer
    {
        private readonly List<ITarget> _targets = new();
        
        public IReadOnlyCollection<ITarget> Targets => _targets;
        
        public void Add(ITarget target)
        {
            if (_targets.Contains(target))
                return;
            
            _targets.Add(target);
        }

        public void Remove(ITarget target)
        {
            if (!_targets.Contains(target))
                return;
            
            _targets.Remove(target);
        }
    }
}