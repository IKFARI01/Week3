using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;


namespace BattleShip
{

    public class GameManager : MonoBehaviour
    {

        [SerializeField]
        private int[,] grid = new int[,]
        {
            //4x4 grid
            {1,1,0,0,1 },
            {0,0,0,0,0 },
            {0,0,1,0,1 },
            {1,0,1,0,0 },
            {1,0,1,0,1 }

        };

        //Where player has fired
        private bool[,] hits;

        //total row/column we have
        private int nRows;
        private int nCols;

        //Current row/column we are on
        private int row;
        private int col;

        //Correctly hits ship
        private int score;

        //Total time game has been running
        private int time;

        //parent of all cells
        [SerializeField] Transform gridRoot;

        //Template used to populate the grid
        [SerializeField] GameObject cellPrefab;
        [SerializeField] GameObject winLabel;
        [SerializeField] TextMeshProUGUI timeLabel;
        [SerializeField] TextMeshProUGUI scoreLabel;

       

        Transform GetCurrentCell()
        {

            //You can figure out the child index
            //of the cell that is a part of the grid
            //By calculating (rows*Cols) + col
            int index = (row * nCols) + col;

            //Return the child by index
            return gridRoot.GetChild(index);

        }

        void SelectCurrentCell()
        {

            //Get the current cell
            Transform cell = GetCurrentCell();

            //Set the "curser" image on
            Transform curser = cell.Find("Curser");
            curser.gameObject.SetActive(true);

        }

        void UnselectCurrentCell()
        {

            //Get the current cell
            Transform cell = GetCurrentCell();

            //Set the "curser" image on
            Transform curser = cell.Find("Curser");
            curser.gameObject.SetActive(false);

        }


        public void MoveHorizontal(int amt)
        {

            //Must unselect previous one
            UnselectCurrentCell();

            //update the column
            row += amt;

            //Make sure the column stays within the bounds of the grid
            row = Mathf.Clamp(row, 0, nCols - 1);

            //Select the new cell
            SelectCurrentCell();

        }

        public void MoveVertical(int amt)
        {

            //Must unselect previous one
            UnselectCurrentCell();

            //update the column
            row += amt;

            //Make sure the column stays within the bounds of the grid
            row = Mathf.Clamp(row, 0, nRows - 1);

            //Select the new cell
            SelectCurrentCell();

        }

        void Awake()
        {

            //Intialize rows/cols
            nRows = grid.GetLength(0);
            nCols = grid.GetLength(1);

            //Create identical 2d ray to grid
            hits = new bool[nRows, nCols];

            //populate the grid using a loop
            for (int i = 0; i < nRows * nCols; i++)
            {

                Instantiate(cellPrefab, gridRoot);

            }

            SelectCurrentCell();
            InvokeRepeating("IncrementTime", 1f, 1f);

        }

        void ShowHit()
        {

            //get the current cell
            Transform cell = GetCurrentCell();
            //set on hit image
            Transform hit = cell.Find("Hit");
            hit.gameObject.SetActive(true);

        }

        void ShowMiss()
        {

            //get the current cell
            Transform cell = GetCurrentCell();
            //set on hit image
            Transform hit = cell.Find("Miss");
            hit.gameObject.SetActive(false);

        }

        void IncrementScore()
        {

            //add 1 to the score
            score++;
            //Update the score label with current score
            scoreLabel.text = string.Format("Score: {0}", score);

        }

        public void Fire()
        {

            if (hits[row, col]) return;

            hits[row, col] = true;

            if (grid[row, col] == 1)
            {

                ShowHit();
                IncrementScore();

            }

            else
            {

                ShowMiss();

            }

        }

        void TryEndGame()
        {

            for (int row = 0; row < nRows; row++)
            {

                for (int col = 0; col < nCols; col++)
                {

                    if (grid[row, col] == 0) continue;

                    if (hits[row, col] == false) return;

                }

            }

            winLabel.SetActive(true);
            CancelInvoke("IncrementTime");

        }

        void IncrementTime()
        {

            time++;
            timeLabel.text = string.Format("{0}:{1}", time / 60, (time % 60).ToString("00"));

        }

    }

}

