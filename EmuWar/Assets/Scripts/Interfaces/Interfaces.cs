namespace Interfaces
{
    /// <summary>
    /// This interface is used for taking damage or healing a entity
    /// </summary>
    public interface IDamageable {
        void TakeDamage (float damage);
    }
    
    public interface IPoolable
    {
        void ReturnToPool();
    }
    
}