using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private GameObject cell;
    [SerializeField] [Range(10,50)] public int totalMines = 10;
    [SerializeField] [Range(10, 30)] public int fieldSizeX = 10;
    [SerializeField] [Range(10, 30)] public int fieldSizeY = 10;

    public Color GizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    private List<List<GameObject>> allCells = new List<List<GameObject>>();
    private Vector2 cellSize;
    private delegate void Delegate(int x, int j);
    private Delegate CellCommand;

    public static SpawnArea Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cellSize.y = cell.GetComponent<SpriteRenderer>().size.y * 2;
        cellSize.x = cell.GetComponent<SpriteRenderer>().size.x * 2;

        CreateGrid();
        SetMinefield();
    }

    //Set the size of the minefield
    public void SetFieldSize(int x, int y)
    {
        fieldSizeX = x;
        fieldSizeY = y;
    }

    //Instantiate the cells depending on the field size, add cells to the list of cells
    private void CreateGrid()
    {
        Vector2 currentPos;

        currentPos.y = transform.position.y + transform.localScale.y / 2;
        currentPos.x = transform.position.x - transform.localScale.x / 2;

        //Loop through the horizontal axis
        for (int x = 0; x < fieldSizeX; x++)
        {
            allCells.Add(new List<GameObject>());

            //Loop through the vertical axis
            for (int y = 0; y < fieldSizeY; y++)
            {
                //Set cell Location, Spawn cell, increment the vertical axis
                cell.transform.position = currentPos;
                allCells[x].Add(Instantiate(cell));
                allCells[x][y].GetComponent<Cell>().SetCoords(x, y);
                currentPos.y -= cellSize.y;
            }

            //Increment the horizontal vector and reset the vertical
            currentPos.x += cellSize.x;
            currentPos.y = transform.position.y + transform.localScale.y / 2;
        }
    }

    //Function to random select tiles to be mines
    private void SetMinefield()
    {
        int[] index = new int[2];
        CellCommand = IncreaseMinecounts;

        for (int i = totalMines; i >= 0; i--)
        {
            index[0] = Random.Range(0, fieldSizeX);
            index[1] = Random.Range(0, fieldSizeY);

            if (allCells[index[0]][index[1]].GetComponent<Cell>().IsMine())
                i++;
            //Set the mine then increment neighbor cells' mine count
            else
            {
                allCells[index[0]][index[1]].GetComponent<Cell>().SetMine();
                CommandNeighbor(index[0], index[1], CellCommand);
            }
        }
    }

    //Clear all empty neighboring cells, stop when a cel with a number is shown
    public void EasyClear(int x, int y)
    {
        CellCommand = CellOnMouseUp;

        CommandNeighbor(x, y, CellCommand);
    }

    //Increase mine count of one cell
    private void IncreaseMinecounts(int x, int y) => allCells[x][y].GetComponent<Cell>().IncreaseMineCount();

    //Click a cell
    private void CellOnMouseUp(int x, int y) => allCells[x][y].GetComponent<Cell>().OnMouseUp();

    //Iterate through the cell list
    private void CommandNeighbor(int x, int y, Delegate method)
    {
        //Go through all values (x - 1, x, x + 1)
        for (int i = x - 1; i <= x + 1; i++)
        {
            //Skip if i is out of the x range
            if (0 > i || i >= fieldSizeX)
                continue;
            //Go through all values (y - 1, y, y + 1)
            for (int j = y - 1; j <= y + 1; j++)
            {
                //Skip if j value is out of y range
                if (0 > j || j >= fieldSizeY)
                    continue;
                //Skip current cell if it is a mine
                if (allCells[i][j].GetComponent<Cell>().IsMine())
                    continue;
                //Else increment mine count in cell
                else
                    method(i, j);
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
