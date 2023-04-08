using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
    public Transform destTr;
    private NavMeshAgent _agent;
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Time.frameCount % 10 == 0)
        _agent.SetDestination(destTr.position);
    }
}
