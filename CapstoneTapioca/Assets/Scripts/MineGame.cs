using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineGame : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
    public int mineCount = 32;

    private Minesweeper minesweeper;
    private Cells[,] state;

    private void Awake ()
    {
        minesweeper = GetComponentInChildren<Minesweeper>();
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        state = new Cells[width, height];
        GenerateCells();
        GenerateMines();
        GenerateNumbers();
        Camera.main.transform.position = new Vector3(width / 2, height / 2, -15);
        minesweeper.Draw(state);
    }

    private void GenerateCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cells cell = new Cells();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cells.Type.Empty;
                state[x, y] = cell;
            }
        }

    }

    private void GenerateMines()
    {
        for (int i = 0; i < mineCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            while (state[x,y].type == Cells.Type.Mine)
            {
                x++;
                if (x >= width)
                {
                    x = 0;
                    y++;
                    if (y >= height)
                    {
                        y = 0;
                    }
                }
            }
            state[x, y].type = Cells.Type.Mine;
            //state[x, y].revealed = true; testing to see if mines generate correctly
        }
    }

    private void GenerateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cells cell = state[x, y];
                if(cell.type == Cells.Type.Mine)
                {
                    continue;
                }
                cell.number = CountMines(x,y);
                if (cell.number > 0)
                {
                    cell.type = Cells.Type.Number;
                }
                //cell.revealed = true; testing to see if numbers generate correctly
                state[x, y] = cell;
            }
        }
    }
    
    private int CountMines(int cellX, int cellY)
    {
        int count = 0;
        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if(adjacentX == 0 && adjacentY == 0)
                {
                    continue;
                }
                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if (GetCell(x,y).type == Cells.Type.Mine)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Flag();
        }
    }

    private void Flag()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = minesweeper.tilemap.WorldToCell(worldPosition);
        Cells cell = GetCell(cellPosition.x, cellPosition.y);
        if (cell.type == Cells.Type.Invalid || cell.revealed)
        {
            return;
        }

        cell.flagged = !cell.flagged;
        state[cellPosition.x, cellPosition.y] = cell;
        minesweeper.Draw(state);
    }

    private Cells GetCell(int x, int y)
    {
        if (IsValid(x,y))
        {
            return state[x, y];
        } else
        {
            return new Cells();
        }
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
