using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public Text MyhpText;
    public Text MyatText;
    public Text MymgText;

    public Text EnemyhpText;
    public Text EnemyatText;
    public Text EnemymgText;

    public Text ShowTurnText;

    public Text[] MyspecialText = new Text[4];
    public Text[] EnemyspecialText = new Text[4];



    public void TextUpdate(int myhp, int myat, int mymg, int enemyhp, int enemyat, int enemymg)
    {
        MyhpText.text = "HP: " + myhp;
        MyatText.text = "PW: " + myat;
        MymgText.text = "MG: " + mymg;

        EnemyhpText.text = "HP: " + enemyhp;
        EnemyatText.text = "PW: " + enemyat;
        EnemymgText.text = "MG: " + enemymg;

    }

    public void SpecialTextUpdate(int[,] myspecial, int[,] enemyspecial)
    {
        MyspecialText[0].text = myspecial[0, 1].ToString();
        MyspecialText[1].text = myspecial[1, 1].ToString();
        MyspecialText[2].text = myspecial[2, 0].ToString();
        MyspecialText[3].text = myspecial[2, 1].ToString();

        EnemyspecialText[0].text = enemyspecial[0, 1].ToString();
        EnemyspecialText[1].text = enemyspecial[1, 1].ToString();
        EnemyspecialText[2].text = enemyspecial[2, 0].ToString();
        EnemyspecialText[3].text = enemyspecial[2, 1].ToString();
    }


    public void ShowTurn(int id, int turn)
    {

        if (id == turn)
        {
            ShowTurnText.text = "攻撃のターンです";

        }
        else
        {
            ShowTurnText.text = "防御のターンです";

        }
    }


    public void WaitText()
    {
        ShowTurnText.text = "相手の選択待ちです";

    }




}
