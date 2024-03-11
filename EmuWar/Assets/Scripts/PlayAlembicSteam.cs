using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;
using UnityEngine.Playables; 

public class PlayAlembicSteam : MonoBehaviour
{
    private AlembicStreamPlayer alembicPlayer ; // Reference to the PlayableDirector component

    // Start is called before the first frame update
    void Start()
    {
        alembicPlayer = gameObject.GetComponent<AlembicStreamPlayer>(); // Get the PlayableDirector component from the same GameObject
        
    }

    // Update is called once per frame
    void Update()
    {
        alembicPlayer.CurrentTime = Time.time;
    }
}