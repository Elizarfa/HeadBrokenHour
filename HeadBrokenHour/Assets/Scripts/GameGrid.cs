using UnityEngine;
using System.Collections.Generic;
using TMPro; // Если вы используете TextMeshPro
// using UnityEngine.UI; // Если вы используете обычный Text

public class GameGrid : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridSize = 3;
    public float spacing = 1.1f;
    private Tile[,] tiles;
    private Vector2 emptyPosition;

    void Start()
    {
        tiles = new Tile[gridSize, gridSize];
        GenerateGrid();
        ShuffleGrid(100); // Перемешиваем плитки при старте
    }

    void GenerateGrid()
    {
        int tileNumber = 1;
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                if (tileNumber < gridSize * gridSize)
                {
                    Vector3 spawnPosition = new Vector3(col * spacing, -row * spacing, 0);
                    GameObject tileObject = Instantiate(tilePrefab, spawnPosition, Quaternion.identity, transform);
                    Tile tile = tileObject.GetComponent<Tile>();
                    tile.Setup(tileNumber, this);
                    tiles[row, col] = tile;
                    tileNumber++;
                }
                else
                {
                    emptyPosition = new Vector2(col, row);
                }
            }
        }
    }

    void ShuffleGrid(int shuffleSteps)
    {
        for (int i = 0; i < shuffleSteps; i++)
        {
            List<Vector2> possibleMoves = GetPossibleMoves((int)emptyPosition.x, (int)emptyPosition.y);
            if (possibleMoves.Count > 0)
            {
                int randomIndex = Random.Range(0, possibleMoves.Count);
                Vector2 move = possibleMoves[randomIndex];
                TryMoveTile(tiles[(int)move.y, (int)move.x]);
            }
        }
    }

    List<Vector2> GetPossibleMoves(int x, int y)
    {
        List<Vector2> neighbors = new List<Vector2>();
        // Проверка соседей (влево, вправо, вверх, вниз)
        if (x > 0) neighbors.Add(new Vector2(x - 1, y));
        if (x < gridSize - 1) neighbors.Add(new Vector2(x + 1, y));
        if (y > 0) neighbors.Add(new Vector2(x, y - 1));
        if (y < gridSize - 1) neighbors.Add(new Vector2(x, y + 1));

        List<Vector2> emptyNeighbors = new List<Vector2>();
        foreach (Vector2 neighbor in neighbors)
        {
            if (neighbor == emptyPosition)
            {
                // Это неправильно, нам нужны соседние клетки *вокруг* пустой
            }
            else
            {
                // Проверяем, является ли соседняя клетка смежной с пустой
                int dx = Mathf.Abs((int)neighbor.x - (int)emptyPosition.x);
                int dy = Mathf.Abs((int)neighbor.y - (int)emptyPosition.y);
                if ((dx == 1 && dy == 0) || (dx == 0 && dy == 1))
                {
                    emptyNeighbors.Add(neighbor);
                }
            }
        }
        return emptyNeighbors;
    }

    public void TryMoveTile(Tile clickedTile)
    {
        int clickedX = -1, clickedY = -1;
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                if (tiles[row, col] == clickedTile)
                {
                    clickedX = col;
                    clickedY = row;
                    break;
                }
            }
        }

        if (clickedX != -1 && clickedY != -1)
        {
            int deltaX = Mathf.Abs(clickedX - (int)emptyPosition.x);
            int deltaY = Mathf.Abs(clickedY - (int)emptyPosition.y);

            if ((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1))
            {
                // Плитка соседняя с пустой клеткой, можно двигать
                tiles[(int)emptyPosition.y, (int)emptyPosition.x] = clickedTile;
                tiles[clickedY, clickedX] = null;

                Vector3 targetPosition = new Vector3(emptyPosition.x * spacing, -emptyPosition.y * spacing, 0);
                clickedTile.transform.localPosition = targetPosition;

                emptyPosition = new Vector2(clickedX, clickedY);

                CheckWinCondition();
            }
        }
    }

    void CheckWinCondition()
    {
        int tileNumber = 1;
        bool won = true;
        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                if (tileNumber < gridSize * gridSize)
                {
                    if (tiles[row, col] == null || tiles[row, col].number != tileNumber)
                    {
                        won = false;
                        break;
                    }
                    tileNumber++;
                }
            }
            if (!won) break;
        }

        if (won)
        {
            Debug.Log("Вы победили!");
            // Здесь можно добавить UI для отображения победы
        }
    }
}