using UnityEngine;
using UnityScene = UnityEngine.SceneManagement;
using StarterAssets;

public class floor_change : MonoBehaviour
{
    public GameObject destination;
    public bool final_tp; // FOR DEBUG PURPOSES
                          // Once FMV is implemented, this will be a button prompt. For now, walk up to the throne to activate the fight.
    public bool flip; // Since player retains rotation relative to world coords, this makes sure the player is facing the right direction
                      // May need to update this to be situational in the future
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

                // give the controller to your lil bro
                if (c != null) c.enabled = false;

                // Teleport!
                player.transform.position = destination.transform.position;

                if (flip)
                {
                    player.transform.Rotate(0f, 180f, 0f);

                    ThirdPersonController tpc = player.GetComponent<ThirdPersonController>();
                    if (tpc != null) tpc.LockCameraPosition = true;
                    
                    // Rotate the camera with respect to the pitch/yaw (too tired to figure that out rn)

                    if (tpc != null) tpc.LockCameraPosition = false;
                }

                // get the controller back
                if (c != null) c.enabled = true;

                lastTeleportTime = Time.time;
            }
            if (final_tp)
            {
                UnityScene.SceneManager.LoadScene(1);
            }
        }
    }
}
