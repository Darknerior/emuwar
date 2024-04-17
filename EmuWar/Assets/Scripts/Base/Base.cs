using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools;
public class Base : MonoBehaviour
{
    [SerializeField] private int amountOfEnemiesOnEnable;
    [SerializeField] private Vector3 enemyOffset;
    [SerializeField] private float minPatrolDistFormBase;
    [SerializeField] private float maxPatrolDistFormBase;
    [SerializeField] private ObjectivesTextHandler ObjectivesMarkers;
    private List<GameObject> enemies = new();
    private List<Patrol> patrolTargets = new();
    private Vector3[] multiples = { new (1, 0, 1), new(1, 0, -1), new (-1, 0, -1), new (-1, 0, 1) };
    private BaseController control;
    private int initialCount;
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        for (int i = 0; i < amountOfEnemiesOnEnable; i++)
        {
            GameObject obj = GameManager.Instance.Pool.Get(ObjectList.ENEMY, true);
            enemies.Add(obj);
            Vector3 newPosition;

            do newPosition = Vector3Tools.RandomVector(enemyOffset);
            while (PosOverlapsExistingPositions(newPosition, i));

            obj.transform.position = newPosition + this.transform.position;
            patrolTargets.Add(obj.gameObject.GetComponent<EnemyBT>().SetBase(this).ThisNode.NodeType<Patrol>());
            obj.SetActive(true);
        }
        var positions = GeneratePositions();
        for (int i = 0;i < patrolTargets.Count;i++)
        {
            List<Vector3> pos = new(){ positions[i % positions.Count], positions[(i + 1) % positions.Count] };
            patrolTargets[i].PatrolTargets(pos);
        }

        initialCount = enemies.Count;
        ObjectivesMarkers.EnableObjectiveBannerWithString("Defeat The Enemies!").EnableProgressMarkerWithString($"Enemies Left: {initialCount}/{initialCount}");
    }

    public void SetController(BaseController thing)
    {
        control = thing;
    }

    private bool PosOverlapsExistingPositions(Vector3 pos,int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (Vector3.Distance(pos, enemies[i].transform.position) <= 1.0f) return true;
        }
        return false;
    }

    public void RemoveEntityFromList(EnemyBT emu)
    {
        enemies.Remove(emu.gameObject);
        patrolTargets.Remove(emu.Patrol);
        CheckIfAllEnemiesAreDefeated();
    }

    private void CheckIfAllEnemiesAreDefeated()
    {
        if (enemies.Count > 0)
        {
            ObjectivesMarkers.DisableText(true, false).UpdateProgressText($"Enemies Left: {enemies.Count}/{initialCount}");
            return;
        }
        
        ObjectivesMarkers.DisableText();
        control.StartCountdown();
    }

    private List<Vector3> GeneratePositions()
    {
        Vector3 pos = transform.position;
        Vector3 offset = FindFirstPatrolPos(minPatrolDistFormBase, maxPatrolDistFormBase);
        return multiples.Select(t => offset.Multiply(t) + pos).ToList();
    }

    public void OnDrawGizmos()
    {
        foreach (var patrol in patrolTargets)
        {
            patrol.DrawGizmos();
        }
    }
    
    /// <summary>
    /// Generates a vector which is a distance between the min and max values 
    /// in the x and z axis
    /// </summary>
    /// <returns></returns>
    private Vector3 FindFirstPatrolPos(float min, float max)
    {
        //Performs an inverse pythgoras calculation.
        //creates random hypotenuse, and a side, and uses it to calculate b size
        float dist = Random.Range(min, max);
        dist.RoundToNearest(0.025f); 
        float xMovement = Random.Range(minPatrolDistFormBase, dist);
        dist *= dist;
        dist -= xMovement * xMovement;
        float zMovement = Mathf.Sqrt(dist);
        return new Vector3(xMovement, 0, zMovement);
    }
}
