using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "cellManager")]
public class CellManager : ScriptableObject
{
    private delegate void Delegate(int x, int j);
    private Delegate CellCommand;
    private List<List<GameObject>> allCells;
    [SerializeField] private int totalMines;

    // Start is called before the first frame update
    public void OnEnable()
    {
        allCells = new List<List<GameObject>>();
    }

    //Add a Row to the list
    public void AddRow() => allCells.Add(new List<GameObject>());

    //Add a cell to the row
    public void AddCell(GameObject gameObject, int x) => allCells[x].Add(gameObject);

    //Set a cell's location in the cell
    public void SetCellLocation(int x, int y) => allCells[x][y].GetComponent<Cell>().SetCoords(x, y);

    //Clear all empty neighboring cells, stop when a cel with a number is shown
    public void ClickNeighbors(int x, int y)
    {
        CellCommand = CellOnMouseUp;

        CommandNeighbor(x, y, CellCommand);
    }

    //Function to random select tiles to be mines
    public void SetMinefield()
    {
        int[] index = new int[2];
        CellCommand = IncreaseMinecounts;

        for (int i = totalMines; i >= 0; i--)
        {
            index[0] = Random.Range(0, allCells[0].Count);
            index[1] = Random.Range(0, allCells.Count);

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
            if (0 > i || i >= allCells[0].Count)
                continue;
            //Go through all values (y - 1, y, y + 1)
            for (int j = y - 1; j <= y + 1; j++)
            {
                //Skip if j value is out of y range
                if (0 > j || j >= allCells.Count)
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
}
