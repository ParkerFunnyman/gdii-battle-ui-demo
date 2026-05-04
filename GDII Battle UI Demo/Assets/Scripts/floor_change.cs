using UnityEngine;

public class floor_change : MonoBehaviour
{
    public GameObject destination;
    private static float lastTeleportTime = -10f;
    private float cooldown = 1.50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player") && Time.time - lastTeleportTime > cooldown)
        {
            if (destination != null)
            {
                CharacterController c = player.GetComponent<CharacterController>();

                if (c != null) c.enabled = false;
                player.transform.position = destination.transform.position;
                if (c != null) c.enabled = true;

                lastTeleportTime = Time.time;
            }
        }
    }
}
