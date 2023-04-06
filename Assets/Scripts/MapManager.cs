using UnityEngine;

public class MapManager : MonoBehaviour
{
    private int[,] _maze;

    [SerializeField] private MazeConfiguration _mazeConfig;
    [SerializeField] private Player _player;

    private void Awake()
    {
        _maze = MapGenerator.InitMap(_mazeConfig);
        Vector3 playerPos = new Vector3(_mazeConfig.mapSize * _mazeConfig.wallSize, 0, _mazeConfig.mapSize * _mazeConfig.wallSize);
        _player.transform.position = playerPos;
    }
}
