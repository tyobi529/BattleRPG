using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerStatus : MonoBehaviour, IPunObservable
{

    public int hp;
    public int power;
    public int magic;

    //public int[,] special = new int[3, 2];

    public int[] special_pw = new int[2];
    public int[] special_mg = new int[2];
    public int[] special_su = new int[2];

    public GameObject MakingStatus;

    void Start()
    {
        MakingStatus = GameObject.Find("MakingStatus");

        MakingStatus status = MakingStatus.GetComponent<MakingStatus>();

        hp = status.hp;
        power = status.power;
        magic = status.magic;

        special_pw = status.special_pw;
        special_mg = status.special_mg;
        special_su = status.special_su;

    }


    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hp);
            stream.SendNext(power);
            stream.SendNext(magic);
            stream.SendNext(special_pw);
            stream.SendNext(special_mg);
            stream.SendNext(special_su);

        }
        else
        {
            hp = (int)stream.ReceiveNext();
            power = (int)stream.ReceiveNext();
            magic = (int)stream.ReceiveNext();
            special_pw = (int[])stream.ReceiveNext();
            special_mg = (int[])stream.ReceiveNext();
            special_su = (int[])stream.ReceiveNext();

        }
    }
}
