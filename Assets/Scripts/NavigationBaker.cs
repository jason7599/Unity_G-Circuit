using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour 
{
    [SerializeField] private NavMeshSurface _dick;

    private void Start()
    {
        _dick.BuildNavMesh();
    }
}