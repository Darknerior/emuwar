using UnityEngine;

public class EmuNPC : GameEntity
{
    public GameObject player;
    private bool moveTowardPlayer = false;
    [SerializeField]private float emuNPCStartHealth = 100f;
    [SerializeField]private float emuNPCMaxHealth = 100f;
    [SerializeField]private float targetDistance = 5f;
    [SerializeField]private float maxDistance = 20f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private void Start() {
        health = emuNPCStartHealth;
        speed = moveSpeed;
    }
    
    public void Damage(float damage){
        TakeDamage(damage); // Call base class method
        if (health > emuNPCMaxHealth) health = emuNPCMaxHealth;//Ensure health does no go abovemax
        else if(health <= 0)Die();//Die 
    }
    
    private void Update() {
        Movement();
    }

    protected override void Movement() {
        if (player == null)return;

        // Calculate the distance between NPC and player
        var distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        var targetDirection = (player.transform.position - transform.position).normalized;
        var moveDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0f);
        
        // If the distance is greater than maxDistance, move towards player
        if (distanceToPlayer > maxDistance) moveTowardPlayer = true;
            
        if (moveTowardPlayer) {
            if (distanceToPlayer < targetDistance) moveTowardPlayer = false;
            transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.position +=  moveSpeed * Time.deltaTime * transform.forward ;
        }
        else { 
            //idle movement
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        
    }
}
