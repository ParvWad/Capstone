using UnityEngine;
using UnityEngine.AI;

public class SyncNavMeshAgentWithCamera : MonoBehaviour
{
    public Transform arCamera;     // Reference to the AR Camera
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (arCamera == null)
        {
            Debug.LogError("AR Camera is not assigned.");
        }
    }

    void Update()
    {
        if (arCamera != null)
        {
            // Sync the NavMeshAgent’s position with the AR Camera's position
            agent.Warp(arCamera.position);
        }
    }
}
