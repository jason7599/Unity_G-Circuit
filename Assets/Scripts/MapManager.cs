using UnityEngine;

public class MapManager : MonoBehaviour
{
    private int[,] _maze;

    [SerializeField] private MazeConfiguration _mazeConfig;

    private void Awake()
    {
        _maze = MapGenerator.InitMap(_mazeConfig);
    }
}
