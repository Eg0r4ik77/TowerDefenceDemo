using Gameplay.Monster;

namespace Gameplay.Attack
{
    public abstract class AttackStrategy
    {
        protected ITarget target;
        
        public void SetTarget(ITarget target)
        {
            this.target = target;
        }

        public virtual bool ReadyToAttack() => !ReferenceEquals(target, null);
        public abstract void Attack();
    }
}