using Interfaces;
using UnityEngine;
//diables unsused field warning 
#pragma warning disable 0414
public class EmuNPC : GameEntity,ICagedEmu
{
    public GameObject player;
    private bool moveTowardPlayer = false;
    private bool caged = true;
    [SerializeField]private float emuNpcStartHealth = 100f;
    [SerializeField]private float emuNpcMaxHealth = 100f;
    [SerializeField]private float targetDistance = 5f;
    [SerializeField]private float maxDistance = 20f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private void Start() {
        health = emuNpcStartHealth;
        speed = moveSpeed;
    }
    
    private void Update() {
        if (caged)
        {
            Idle();
            return;
        }
        Movement();
    }

    protected override void Movement() {
        if (player == null)return;

        // Calculate the distance between NPC and player
        var distanceToPlayer = Vector3.Distance(transform.position, new Vector3(player.transform.position.x, 0, player.transform.position.z));
        var targetDirection = (player.transform.position - transform.position).normalized;
        var moveDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0f);
        
        // If the distance is greater than maxDistance, move towards player
        if (distanceToPlayer > maxDistance) moveTowardPlayer = true;
            
        if (moveTowardPlayer) {
            if (distanceToPlayer < targetDistance) moveTowardPlayer = false;
            transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.position +=  moveSpeed * Time.deltaTime * transform.forward ;
        }
        else
        {
            Idle();
        }
        
    }
    
    
    
    /// <summary>
    ///Idle movement
    /// </summary>
    private void Idle()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    
    /// <summary>
    ///Releases the emu from the confines of prison
    /// </summary>
    public bool Release()
    {
        caged = false;
        gameObject.transform.SetParent(null);
        return true;
    }
}
