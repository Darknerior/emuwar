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
    
    public interface IInteractable
    {
        void Interact();
        string GetText();
    }

    public interface IStatOwner
    {
        int Health { get; }
        int MaxArmySize { get; }
        int MaxHealthIncrease { get; }
        int ArmySizeIncrease { get; }
        int ArmySize { get; }
        void AddToArmy();
        void RemoveFromArmy();
        void UpdateArmy(int army);
        void UpdateHealth(int health);
    }

    public interface IUpHealth
    {
       void UpHealth();
    }

    public interface IUpArmySize
    {
        void UpArmy();
    }

    public interface ICagedEmu
    {
        bool Release();
    }
    
}