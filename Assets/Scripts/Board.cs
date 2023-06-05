using UnityEngine;
using System;
using TMPro;

public class Board : MonoBehaviour
{
    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[] {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
        KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7,
        KeyCode.Alpha8, KeyCode.Alpha9
        };

    private Row[] rows;
    private GameObject board;

    private int rowCount;
    private int rowIndex;
    private int columnIndex;

    private string correctNumber;

    [Header("Tiles")]
    public Tile.State emptyState;
    public Tile.State occupiedState;
    public Tile.State correctState;
    public Tile.State wrongSpotState;
    public Tile.State incorrectState;

    [Header("UI")]
    public GameObject statisticsGameObject;
    public GameObject difficultyGameObject;
    public GameObject buttonsGameObject;
    private Statistics statistics;
    private Audio audioManager;
    public GameObject newNumberButton;
    public GameObject rowPrefab;
    public GameObject availableText;
    public GameObject keyboard;
    private void Start()
    {
        rows = GetComponentsInChildren<Row>();
        rowCount = rows.Length;
        NewGame();
        statistics = statisticsGameObject.GetComponent<Statistics>();
        board = GameObject.FindGameObjectWithTag("Board");
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<Audio>();
        statistics.LoadStatistics();
    }

    public void NewGame()
    {
        rows = GetComponentsInChildren<Row>();
        ClearBoard();
        SetRandomNumber();
        enabled = true;
        newNumberButton.SetActive(false);
        keyboard.SetActive(true);
    }

    private void SetRandomNumber()
    {
        string randomNumber = Convert.ToString(UnityEngine.Random.Range(0, MaxNumberRange()));
        correctNumber = GetCorrectNumber(randomNumber);
        correctNumber = correctNumber.ToLower().Trim();
        print("Right answer is: " + correctNumber);
    }

    public void AddRow()
    {
        Instantiate(rowPrefab, rows[0].transform.parent);
        rowCount++;
    }
    public void DeleteRow()
    {
        Destroy(rows[rowCount-1].gameObject);
        --rowCount;
        NewGame();
    }

    public void SetTile(string digit)
    {
        Row currentRow = rows[rowIndex];
        currentRow.tiles[columnIndex].SetDigit(digit[0]);
        audioManager.PlaySound("type");
        currentRow.tiles[columnIndex].SetState(occupiedState);
        columnIndex++;
    }

    public void BackspaceTile()
    {
        Row currentRow = rows[rowIndex];
        columnIndex = Mathf.Max(columnIndex - 1, 0);
        currentRow.tiles[columnIndex].SetDigit('\0');
        currentRow.tiles[columnIndex].SetState(emptyState);
    }


    private string GetCorrectNumber(string number)
    {
        while(number.Length != rows[0].tiles.Length)
        {
            number = '0' + number;
        }
        return number;
    }
    private int MaxNumberRange()
    {
        return (int)Math.Pow(10, rows[0].tiles.Length);
    }

    private void Update()
    {
        Row currentRow = rows[rowIndex];

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            BackspaceTile();
        }
        else if (columnIndex >= currentRow.tiles.Length)
        {
            SubmitRow(currentRow);
        }
        else
        {
            for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
            {
                if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
                {
                    SetTile(Convert.ToString(SUPPORTED_KEYS[i])[Convert.ToString(SUPPORTED_KEYS[i]).Length-1].ToString());
                    break;
                }
            }
        }
    }

    private void SubmitRow(Row row)
    {
        string remaining = correctNumber;

        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];

            if (tile.digit == correctNumber[i])
            {
                tile.SetState(correctState);

                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if (!correctNumber.Contains(tile.digit))
            {
                tile.SetState(incorrectState);
            }
        }

        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];

            if (tile.state != correctState && tile.state != incorrectState)
            {
                if (remaining.Contains(tile.digit))
                {
                    tile.SetState(wrongSpotState);

                    int index = remaining.IndexOf(tile.digit);
                    remaining = remaining.Remove(index, 1);
                    remaining = remaining.Insert(index, " ");
                }
                else
                {
                    tile.SetState(incorrectState);
                }
            }
        }

        //print("rowIndex = " + rowIndex + ", rowCount = " + rowCount + ", rows.Length = " + rows.Length);

        if (HasWon(row))
        {
            OnGameEnd(true);
            audioManager.PlaySound("win");
            return;
        }

        if (rowIndex == rowCount-1 || rowIndex >= rows.Length) {
            OnGameEnd(false);
            audioManager.PlaySound("lose");
            return;
        }
        else
        {
            audioManager.PlaySound("submit row");
        }
        rowIndex++;
        columnIndex = 0;
        statistics.OnRowSubmit();
    }

    private void OnGameEnd(bool isWin)
    {
        statistics.OnGameEnd(isWin);
        statistics.SaveStatistics();
        enabled = false;
        newNumberButton.SetActive(true);
        availableText.SetActive(false);
        keyboard.SetActive(false);
    }

    private bool HasWon(Row row)
    {
        for (int i = 0; i < row.tiles.Length; i++)
        {
            if (row.tiles[i].state != correctState) {
                return false;
            }
        }

        return true;
    }

    private void ClearBoard()
    {
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                rows[row].tiles[col].SetDigit('\0');
                rows[row].tiles[col].SetState(emptyState);
            }
        }

        rowIndex = 0;
        columnIndex = 0;
    }

    public void EnableStatistics()
    {
        if ((rowIndex == 0 && columnIndex == 0) || (newNumberButton.activeSelf))
        {
            NewGame();
            availableText.SetActive(false);
            board.SetActive(false);
            buttonsGameObject.SetActive(false);
            keyboard.SetActive(false);
            statisticsGameObject.SetActive(true);
        }
        else
        {
            availableText.SetActive(true);
        }


    }
    
    public void EnableDifficultyMenu()
    {
        if((rowIndex == 0 && columnIndex == 0) || (newNumberButton.activeSelf))
        {
            NewGame();
            availableText.SetActive(false);
            board.SetActive(false);
            if (statisticsGameObject.activeSelf)
            {
                statisticsGameObject.SetActive(false);
            }
            buttonsGameObject.SetActive(false);
            keyboard.SetActive(false);
            difficultyGameObject.SetActive(true);
        }
        else
        {
            availableText.SetActive(true);
        }
    }
    

    public void ChangeDifficulty(int newRowCount)
    {
        if(newRowCount > rowCount && newRowCount < 7)
        {
            while(newRowCount != rowCount)
                AddRow();
            rows = GetComponentsInChildren<Row>();
        }
        else if(newRowCount < rowCount && newRowCount > 2)
        {
            while (newRowCount != rowCount)
                DeleteRow();
            rows = GetComponentsInChildren<Row>();
        }
        board.SetActive(true);
        statisticsGameObject.SetActive(false);
        difficultyGameObject.SetActive(false);
        buttonsGameObject.SetActive(true);
        NewGame();
    }




}
