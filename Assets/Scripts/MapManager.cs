using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
    private int[,] _maze;

    [SerializeField] private MazeConfiguration _mazeConfig;
    [SerializeField] private NavMeshSurface _surface;

    private void Awake()
    {
        _maze = MapGenerator.InitMap(_mazeConfig);
        _surface.BuildNavMesh();
    }

    private void Start()
    {
        Player.Position = (Vector3.forward + Vector3.right) * _mazeConfig.mapSize * _mazeConfig.wallSize;
    }
}
