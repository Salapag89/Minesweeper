using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;

    private int[] coord = new int[2];
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
            GetComponent<SpriteRenderer>().sprite = sprites[2];
        else if (neighboringMines > 0)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[1];
            GetComponentInChildren<TextMeshPro>().SetText(neighboringMines.ToString());
        }
        else if (!isClicked)
        {
            isClicked = true;
            GetComponent<SpriteRenderer>().sprite = sprites[1];
            SpawnArea.Instance.EasyClear(coord[0], coord[1]);
            return;
        }

        isClicked = true;

    }

    
    }
