using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private CellManager cellManager;

    [SerializeField] private int[] coord = new int[2];
    private bool isClicked = false;
    private bool isMine = false;
    private int neighboringMines = 0;

    public void SetCoords(int x, int y)
    {
        coord[0] = x;
        coord[1] = y;
    }

    //See if cell is a mine
    public bool IsMine() => isMine;

    //Set cell as a mine
    public void SetMine() => isMine = true;

    //Increase neighboring mine count
    public void IncreaseMineCount() => neighboringMines++;

    //Set isClicked to true for game over
    public void Clicked() => isClicked = true;

    public void Reset()
    {
        isClicked = false;
        isMine = false;
        neighboringMines = 0;
        GetComponent<SpriteRenderer>().sprite = sprites[0];
    }

    //Handle left clicks on the cell to display the count or if the cell is a mine
    public void OnMouseUp()
    {
        if (isMine)
        {
            if (!cellManager.IsGameOver())
                cellManager.GameOver();
            GetComponent<SpriteRenderer>().sprite = sprites[2];
            return;
        }            

        if (!isClicked)
        {
            isClicked = true;
            
            if (neighboringMines > 0)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[1];
                GetComponentInChildren<TextMeshPro>().SetText(neighboringMines.ToString());
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = sprites[1];
                cellManager.ClickNeighbors(coord[0], coord[1]);
            }

        }

    }

    private void PrintCoords()
    {
        print(coord[0].ToString() + ',' + coord[1].ToString());
    }

    
 }
