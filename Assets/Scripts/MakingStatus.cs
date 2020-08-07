using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakingStatus : MonoBehaviour
{

    public int hp = 300;
    public int power = 50;
    public int magic = 50;

    public int[] special_pw = new int[2];
    public int[] special_mg = new int[2];
    public int[] special_su = new int[2];

    int target = 0;

    int sum = 1000;

    public Text[] statusText = new Text[7];

    public Text SumText;

    public GameObject LoginController;
    public GameObject PrepareScene;
    public GameObject BattleScene;


    // Start is called before the first frame update
    void Start()
    {
        special_pw[0] = 99;
        special_mg[0] = 99;


        special_pw[1] = 0;
        special_mg[1] = 0;
        special_su[0] = 0;
        special_su[1] = 0;

    }

    // Update is called once per frame
    void Update()
    {
        statusText[0].text = hp.ToString();
        statusText[1].text = power.ToString();
        statusText[2].text = magic.ToString();
        statusText[3].text = special_pw[1].ToString();
        statusText[4].text = special_mg[1].ToString();
        statusText[5].text = special_su[0].ToString();
        statusText[6].text = special_su[1].ToString();

        SumText.text = "残り: " + sum;
    }


    public void OnHpButton()
    {
        //TargetSelect(1);
        if (sum >= 50)
        {
                hp += 50;
                sum -= 50;
            
        }
    }

    public void OnPwButton()
    {
        if (sum >= 100)
        {
            power += 50;

            sum -= 100;
            
        }
    }

    public void OnMgButton()
    {
        if (sum >= 100)
        {
            magic += 50;
            sum -= 100;
        }
    }

    public void OnAt01Button()
    {
        //TargetSelect(4);
        if (sum >= 100)
        {

            special_pw[1]++;
            sum -= 100;
        }
    }

    public void OnAt11Button()
    {
        //TargetSelect(5);
        if (sum >= 100)
        {
            special_mg[1]++;
            sum -= 100;
        }
    }

    public void OnAt20Button()
    {
        //TargetSelect(6);
        if (sum >= 100)
        {
            special_su[0]++;
            sum -= 100;
        }
    }

    public void OnAt21Button()
    {
        //TargetSelect(7);
        if (sum >= 100)
        {
            special_su[1]++;

            sum -= 100;
        }
    }

    public void OnStartButton()
    {
        PrepareScene.SetActive(false);
        BattleScene.SetActive(true);
        LoginController.SetActive(true);
    }



    //private void TargetSelect(int a)
    //{
    //    target = a;
    //}


    //private void UpStatus()
    //{
    //    if (sum >= 50)
    //    {
    //        if (target == 1)
    //        {
    //            hp += 50;
    //            sum -= 50;

    //            return;
    //        }
    //    }

    //    if (sum >= 100)
    //    {
    //        if (target == 2)
    //        {
    //            power += 50;
    //        }

    //        else if (target == 3)
    //        {
    //            magic += 50;
    //        }

    //        else if (target == 4)
    //        {
    //            special_pw[1]++;
    //        }

    //        else if (target == 5)
    //        {
    //            special_mg[1]++;
    //        }

    //        else if (target == 6)
    //        {
    //            special_su[0]++;
    //        }

    //        else if (target == 7)
    //        {
    //            special_su[1]++;
    //        }

    //        sum -= 100;

    //    }
    //}

    //private void DownStatus()
    //{
    //    if (sum >= 50)
    //    {
    //        if (target == 1)
    //        {
    //            hp += 50;
    //            sum -= 50;

    //            return;
    //        }
    //    }

    //    if (sum >= 100)
    //    {
    //        if (target == 2)
    //        {
    //            power += 50;
    //        }

    //        else if (target == 3)
    //        {
    //            magic += 50;
    //        }

    //        else if (target == 4)
    //        {
    //            special_pw[1]++;
    //        }

    //        else if (target == 5)
    //        {
    //            special_mg[1]++;
    //        }

    //        else if (target == 6)
    //        {
    //            special_su[0]++;
    //        }

    //        else if (target == 7)
    //        {
    //            special_su[1]++;
    //        }

    //        sum -= 100;

    //    }
    //}
}
