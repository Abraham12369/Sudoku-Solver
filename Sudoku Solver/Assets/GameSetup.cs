using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour {

    // Use this for initialization
    public GameObject MainPanel;
    public List<int>[,] numbers = new List<int>[9,9];
    public int[,] CurrentBoard = new int[9,9];
    void Start () {
		foreach(Transform tr in gameObject.transform)
        {
            if (tr.tag == "MainPanel")
            {
                MainPanel = tr.gameObject;
            }
        }
        for (int i = 0; i < 9; i++)
        {
            for(int j=0;j<9;j++)
            {
                numbers[i, j] = new List<int>();
                for(int k = 0; k < 9; k++)
                {
                    numbers[i, j].Add(k);
                }
            }
        }
    
        for (int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                Debug.Log(MainPanel.transform.GetChild(i).gameObject.name);
                Debug.Log(MainPanel.transform.GetChild(i).GetChild(j).gameObject.name);
                InputFieldPlaceholder = MainPanel.transform.GetChild(i).GetChild(j).Find("Placeholder").gameObject;
                Debug.Log(InputFieldPlaceholder.name);
                if(InputFieldPlaceholder.GetComponent<Text>().text!="" && InputFieldPlaceholder.GetComponent<Text>().text != null)
                {
                    row = Mathf.FloorToInt(i / 3) * 3 + 1;
                    row += Mathf.FloorToInt(j / 3) - 1;
                    col = (i % 3)+1;
                    col += j % 3 - 1;
                    CurrentBoard[row, col] = int.Parse(InputFieldPlaceholder.GetComponent<Text>().text);
                }
            }
        }


        

        for(int i = 0; i < 9; i++)
        {
            for(int j = 0; j < 9; j++)
            {
                if (j == 0)
                {
                    printstring= CurrentBoard[i, j].ToString();
                }
                else
                {
                    printstring +=CurrentBoard[i,j].ToString();
                }
            }
            Debug.Log(printstring);
        }

	}
    GameObject InputFieldPlaceholder;
    public int row;
    public int col;
    string printstring;
	// Update is called once per frame
	void Update () {
		
	}



}
