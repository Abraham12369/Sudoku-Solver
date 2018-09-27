using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameSetup : MonoBehaviour {

    // Use this for initialization
    public GameObject MainPanel;
    public List<int>[,] numbers = new List<int>[9, 9];  //establish a 9X9 array of 9 length list
    public List<int>[] rows = new List<int>[9];         //rows info of 9 lenght
    public List<int>[] columns = new List<int>[9];      //col info of 9 length
    public List<int>[] square = new List<int>[9];       //square info of 9 length

    public int[,] CurrentBoard = new int[9,9];          //live input of the board
    public Text[] rowText = new Text[9];                //list of numbers avaliable per row
    public Text[] colText = new Text[9];                //list of numbers avalaible per column
    public Text[] squareText = new Text[9];             //list of numbers avaliable per square

    public Text[] IndText = new Text[81];
    public List<Vector3>[] squareChildCoords = new List<Vector3>[9];

    public GameObject InputFieldPanel;

    void Start () {
        #region initialize lists
        for (int i = 0; i < 9; i++) //write 1-9 to each of 81 list
        {
            rows[i] = new List<int>();
            columns[i] = new List<int>();
            square[i] = new List<int>();
            squareChildCoords[i] = new List<Vector3>();
            for (int j=0;j<9;j++)
            {
                numbers[i, j] = new List<int>();
                for(int k = 0; k < 9; k++)
                {
                    numbers[i, j].Add(k+1);
                }
                rows[i].Add(j+1);
                columns[i].Add(j+1);
                square[i].Add(j+1);
            }
            //rows[i].Remove(0);
            //columns[i].Remove(0);
            //square[i].Remove(0);
        }
        #endregion

        #region read and remove from list
        InputFieldPanel = transform.Find("InputField Panel").gameObject; //grab the inputfield group
        for (int i = 0; i < 81; i++)
        {
            string placeholderText = InputFieldPanel.transform.GetChild(i).Find("Placeholder").GetComponent<Text>().text;
            int rowNumber = Mathf.FloorToInt(i / 9);
            int colNumber = (i % 9);
            int squareNumber = Mathf.FloorToInt(rowNumber / 3) * 3 + Mathf.FloorToInt(colNumber / 3);
            squareChildCoords[squareNumber].Add(new Vector3(rowNumber, colNumber, i));

            if (placeholderText != "") //check if the palceholder value is filled
            {
                

                CurrentBoard[rowNumber, colNumber] = int.Parse(placeholderText);
                for (int j = 0; j < 9; j++) //remove option from row and column
                {
                    numbers[rowNumber, j].Remove(int.Parse(placeholderText));
                    numbers[j, colNumber].Remove(int.Parse(placeholderText));
                }
                for(int n = Mathf.FloorToInt(rowNumber / 3) * 3; n < Mathf.FloorToInt(rowNumber / 3) * 3+3; n++)
                {
                    for(int m = Mathf.FloorToInt(colNumber / 3) * 3; m < Mathf.FloorToInt(colNumber / 3) * 3+3; m++)
                    {
                        numbers[n, m].Remove(int.Parse(placeholderText));
                    }
                }
                numbers[rowNumber, colNumber].Clear();
                rows[rowNumber].Remove(int.Parse(placeholderText));
                columns[colNumber].Remove(int.Parse(placeholderText));
                square[squareNumber].Remove(int.Parse(placeholderText));

                if (int.Parse(placeholderText) == 1)
                {
                    //Debug.Log(i);
                    //Debug.Log(Mathf.FloorToInt(i / 9));
                    //Debug.Log(i % 9);
                }
            }
        }
        #endregion

        for(int i = 0; i < 9; i++)
        {
            rowText[i] = transform.Find("Row Info").GetChild(i).GetComponent<Text>();
            colText[i] = transform.Find("Column Info").GetChild(i).GetComponent<Text>();
            squareText[i] = transform.Find("Square Info").GetChild(i).GetComponent<Text>();
        }
        for(int i = 0; i < 81; i++)
        {
            IndText[i] = transform.Find("Individual info").GetChild(i).GetComponent<Text>();
        }

        

        
        
    }
    public int colorIndex=-1;

    public void FillNumbers()
    {
        Debug.ClearDeveloperConsole();
        if (colorIndex!=-1)
        {
            InputFieldPanel.transform.GetChild(colorIndex).GetComponent<Image>().color = Color.white;
        }
        for (int i = 0; i < 9; i++)
        {
            for (int squareNumber = 0; squareNumber < 8; squareNumber++)
            {
                //Debug.Log("checking number: " + i);
                int count = 0;
                int rowCheck = Mathf.FloorToInt(squareNumber / 3) * 3;
                int columnCheck = Mathf.FloorToInt(squareNumber % 3) * 3;

                for (int y=rowCheck;y<rowCheck+3;y++)
                {
                    for(int x=columnCheck;x<columnCheck+3;x++)
                    {
                        if (numbers[y,x].Contains(i))
                        {
                        count++;
                        }
                    }
                }
                
                //Debug.Log("current number count: " + count);
                if (count == 1)
                {
                    for (int y = rowCheck; y < rowCheck + 3; y++) //check through 3x3
                    {
                        for (int x = columnCheck; x < columnCheck + 3; x++)
                        {
                            if (numbers[y, x].Contains(i)) //if number is present
                            {
                                CurrentBoard[y, x] = i;
                                int indexNumber = y * 9 + x;
                                InputFieldPanel.transform.GetChild(indexNumber).Find("Placeholder").GetComponent<Text>().text = i.ToString();
                                numbers[y, x].Clear();                      //clear current tile
                                for (int z = 0; z < 9; z++)                 //clear column and row
                                {
                                    numbers[y, z].Remove(i);
                                    numbers[z, x].Remove(i);
                                }

                                for (int y2 = rowCheck; y2 < rowCheck + 3; y2++) //check through 3x3 and clear
                                {
                                    for (int x2 = columnCheck; x2 < columnCheck + 3; x2++)
                                    {
                                        numbers[y2, x2].Remove(i);
                                    }
                                }

                                rows[y].Remove(i);          //remove row and column and square numbers
                                columns[x].Remove(i);
                                square[squareNumber].Remove(i);
                                //Debug.Log("square:" + squareNumber);
                                //Debug.Log("row:" + rowCheck + "\t col" + columnCheck);
                                colorIndex = indexNumber;
                                InputFieldPanel.transform.GetChild(colorIndex).GetComponent<Image>().color = Color.green;
                                Debug.Log("condition checked");
                                return;
                                //Debug.Log("found at " + l.x + "\t" + l.y + "\t Index Number: " + indexNumber);
                            }
                        }
                    }                    
                }
            }

        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (numbers[i, j].Count == 1)
                {
                    int number = numbers[i, j][0];
                    CurrentBoard[i, j] = number;
                    int indexNumber = i * 9 + j;
                    InputFieldPanel.transform.GetChild(indexNumber).Find("Placeholder").GetComponent<Text>().text = number.ToString();
                    numbers[i, j].Clear();
                    for (int z = 0; z < 9; z++)
                    {
                        numbers[i, z].Remove(number);
                        numbers[z, j].Remove(number);
                    }
                    int rowCheck = Mathf.FloorToInt(i / 3) * 3;
                    int columnCheck = Mathf.FloorToInt(j / 3) * 3;
                    for (int y2 = rowCheck; y2 < rowCheck + 3; y2++) //check through 3x3 and clear
                    {
                        for (int x2 = columnCheck; x2 < columnCheck + 3; x2++)
                        {
                            numbers[y2, x2].Remove(number);
                        }
                    }
                    rows[i].Remove(number);
                    columns[j].Remove(number);
                    colorIndex = i * 9 + j;
                    InputFieldPanel.transform.GetChild(colorIndex).GetComponent<Image>().color = Color.green;
                    Debug.Log("single checked");
                    Debug.Log(rowCheck);
                    Debug.Log(columnCheck);
                    return;
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {

        FillNumbers();
        for (int i = 0; i < 9; i++)
        {
            string rowstring = "";
            string columnstring = "";
            string squarestring = "";
            foreach (int num in rows[i])
            {
                rowstring += num;
            }
            foreach (int num in columns[i])
            {
                columnstring += num;
            }
            foreach (int num in square[i])
            {
                squarestring += num;
            }
            //Debug.Log("row" + i + rowstring);
            //Debug.Log("col" + i + columnstring);
            //Debug.Log("square" + i + squarestring);
            rowText[i].text = rowstring;
            colText[i].text = columnstring;
            squareText[i].text = squarestring;
            for (int j = 0; j < 9; j++)
            {
                string indString = "";
                foreach (int num in numbers[i, j])
                {
                    indString += num;
                }
                IndText[i * 9 + j].text = indString;
            }
        }
    }



}
