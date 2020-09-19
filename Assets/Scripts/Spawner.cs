using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject cell;
    [SerializeField] private CellManager cellManager;
    [SerializeField] private int fieldSizeX = 10;
    [SerializeField] private int fieldSizeY = 10;

    public Color GizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
    private Vector2 cellSize;

    private void Start()
    {

        cellSize.y = cell.GetComponent<SpriteRenderer>().size.y * 2;
        cellSize.x = cell.GetComponent<SpriteRenderer>().size.x * 2;

        SpawnCells();
        cellManager.SetMinefield();
    }

    //Instantiate the cells depending on the field size, add cells to the list of cells
    private void SpawnCells()
    {
        Vector2 currentPos;

        currentPos.y = transform.position.y + transform.localScale.y / 2;
        currentPos.x = transform.position.x - transform.localScale.x / 2;

        //Loop through the horizontal axis
        for (int x = 0; x < fieldSizeX; x++)
        {
            cellManager.AddRow();

            //Loop through the vertical axis
            for (int y = 0; y < fieldSizeY; y++)
            {
                //Set cell Location, Spawn cell, increment the vertical axis
                cell.transform.position = currentPos;
                cellManager.AddCell(Instantiate(cell), x);
                cellManager.SetCellLocation(x, y);
                currentPos.y -= cellSize.y;
            }

            //Increment the horizontal vector and reset the vertical
            currentPos.x += cellSize.x;
            currentPos.y = transform.position.y + transform.localScale.y / 2;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
