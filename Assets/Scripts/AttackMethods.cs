using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class AttackMethods : MonoBehaviour
{

    //攻撃と防御用のメソッドまとめ


    public void PowerAttack(bool estimate, int attack, int hp)
    {

    }





    //HPを減らすメソッド
    void Attack(int hp, int attack, bool estimate)
    {
        //予測が外れる
        if (!estimate)
        {

        }
        else
        {
            hp -= attack;

        }
    }

}
