using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviourPunCallbacks
{

    GameObject Player1;
    GameObject Player2;


    int hp_1 = 500;
    int power_1 = 100;
    int magic_1 = 50;

    int hp_2 = 500;
    int power_2 = 50;
    int magic_2 = 100;

    //０の時は選択待ち
    //attackは
    //    1~99:物理
    //    100~199:魔法
    //    200~299:補助
    int attack_num = -1;

    //0：物理
    //1：魔法
    //2：補助
    int defence_num = -1;
    //bool turnend = false;


    //攻撃の種類

    public int playerId;

    //常に表示
    private GameObject PowerButton;
    private GameObject MagicButton;
    private GameObject SupportButton;


    private GameObject[] PowerAction = new GameObject[3];
    private GameObject[] MagicAction = new GameObject[3];
    private GameObject[] SupportAction = new GameObject[3];


    //特技
    //１行目は物理
    //２行目は魔法
    //３行目は補助
    public int[,] special_1 = new int[3, 2];

    public int[,] special_2 = new int[3, 2];

    //特技カウント用テキスト
    //Text specialCount

    public int turn = 1;


    Methods methods;

    private GameObject TextController;

    private void Start()
    {
        playerId = PhotonNetwork.LocalPlayer.ActorNumber;

        TextController = GameObject.Find("TextController");


        //ステータス入力
        Player1 = GameObject.Find("Player1(Clone)");
        Player2 = GameObject.Find("Player2(Clone)");

        PlayerStatus status_1 = Player1.GetComponent<PlayerStatus>();
        PlayerStatus status_2 = Player2.GetComponent<PlayerStatus>();

        hp_1 = status_1.hp;
        power_1 = status_1.power;
        magic_1 = status_1.magic;
        for (int i = 0; i < 2; i++)
        {
            special_1[0, i] = status_1.special_pw[i];
            special_1[1, i] = status_1.special_mg[i];
            special_1[2, i] = status_1.special_su[i];
        }

        

        hp_2 = status_2.hp;
        power_2 = status_2.power;
        magic_2 = status_2.magic;
        for (int i = 0; i < 2; i++)
        {
            special_2[0, i] = status_2.special_pw[i];
            special_2[1, i] = status_2.special_mg[i];
            special_2[2, i] = status_2.special_su[i];
        }



        PowerButton = GameObject.Find("PowerButton");
        PowerButton.GetComponent<Button>().onClick.AddListener(OnPowerButton);
        //AttackButton.SetActive(true);
        MagicButton = GameObject.Find("MagicButton");
        MagicButton.GetComponent<Button>().onClick.AddListener(OnMagicButton);
        SupportButton = GameObject.Find("SupportButton");
        SupportButton.GetComponent<Button>().onClick.AddListener(OnSupportButton);



        PowerAction[0] = GameObject.Find("Power_0");
        PowerAction[0].GetComponent<Button>().onClick.AddListener(OnPowerButton_0);
        PowerAction[1] = GameObject.Find("Power_1");
        PowerAction[1].GetComponent<Button>().onClick.AddListener(OnPowerButton_1);

        PowerAction[0].SetActive(false);
        PowerAction[1].SetActive(false);


        MagicAction[0] = GameObject.Find("Magic_0");
        MagicAction[0].GetComponent<Button>().onClick.AddListener(OnMagicButton_0);
        MagicAction[1] = GameObject.Find("Magic_1");
        MagicAction[1].GetComponent<Button>().onClick.AddListener(OnMagicButton_1);

        MagicAction[0].SetActive(false);
        MagicAction[1].SetActive(false);


        SupportAction[0] = GameObject.Find("Support_0");
        SupportAction[0].GetComponent<Button>().onClick.AddListener(OnSupportButton_0);
        SupportAction[1] = GameObject.Find("Support_1");
        SupportAction[1].GetComponent<Button>().onClick.AddListener(OnSupportButton_1);

        SupportAction[0].SetActive(false);
        SupportAction[1].SetActive(false);





        //関数まとめ
        methods = this.GetComponent<Methods>();


        //攻守を表示
        TextController.GetComponent<TextController>().ShowTurn(playerId, turn);


    }


    private void Update()
    {

        //テキスト表示
        //一旦Updateで定義
        if (playerId == 1)
        {
            TextController.GetComponent<TextController>().TextUpdate(hp_1, power_1, magic_1, hp_2, power_2, magic_2);
            TextController.GetComponent<TextController>().SpecialTextUpdate(special_1, special_2);

        }
        else if (playerId == 2)
        {
            TextController.GetComponent<TextController>().TextUpdate(hp_2, power_2, magic_2, hp_1, power_1, magic_1);
            TextController.GetComponent<TextController>().SpecialTextUpdate(special_2, special_1);

        }


        //相手の選択待ち
        if ((attack_num != -1 && turn == playerId) || (defence_num != -1 && turn != playerId))
        {
            TextController.GetComponent<TextController>().WaitText();
        }


        if (attack_num != -1 && defence_num != -1)
        {
            //予測判定用
            int judge = attack_num / 100;
            //予測成功
            if (judge == defence_num)
            {
                Debug.Log("予測成功");

                photonView.RPC(nameof(Expectation), RpcTarget.All);
            }

            else
            {
                //プレイヤー１なら
                if (turn == 1)
                {
                    photonView.RPC(nameof(Domethod_1), RpcTarget.All);
                }

                //プレイヤー２なら
                else if (turn == 2)
                {
                    photonView.RPC(nameof(Domethod_2), RpcTarget.All);
                }
            }

            photonView.RPC(nameof(SpecialCountDown), RpcTarget.All, attack_num);


            photonView.RPC(nameof(AttackSet), RpcTarget.All, -1);
            photonView.RPC(nameof(DefenceSet), RpcTarget.All, -1);

            photonView.RPC(nameof(TurnChange), RpcTarget.All);



        }

    }

    //攻撃番号を入れて関数を呼び出す。
    //プレイヤー１から攻撃
    [PunRPC]
    public void Domethod_1()
    {
        //物理
        if (attack_num == 0)
        {
            methods.Power_0(ref hp_2, power_1, turn, playerId);
        }

        else if (attack_num == 1)
        {
            methods.Power_1(ref hp_2, power_1, turn, playerId);
        }


        //魔法
        else if (attack_num == 100)
        {
            methods.Magic_0(ref hp_2, magic_1, turn, playerId);
        }

        else if (attack_num == 101)
        {
            methods.Magic_1(ref hp_2, magic_1, turn, playerId);
        }


        //補助
        else if (attack_num == 200)
        {
            methods.Support_0(ref power_1, turn, playerId);
        }

        else if (attack_num == 201)
        {
            methods.Support_1(ref magic_1, turn, playerId);
        }
    }

    //プレイヤー2から攻撃
    [PunRPC]
    public void Domethod_2()
    {
        //物理
        if (attack_num == 0)
        {
            methods.Power_0(ref hp_1, power_2, turn, playerId);
        }

        else if (attack_num == 1)
        {
            methods.Power_1(ref hp_1, power_2, turn, playerId);
        }


        //魔法
        else if (attack_num == 100)
        {
            methods.Magic_0(ref hp_1, magic_2, turn, playerId);
        }

        else if (attack_num == 101)
        {
            methods.Magic_1(ref hp_1, magic_2, turn, playerId);
        }

        //補助
        else if (attack_num == 200)
        {
            methods.Support_0(ref power_2, turn, playerId);
        }

        else if (attack_num == 201)
        {
            methods.Support_1(ref magic_2, turn, playerId);
        }
    }

    //予測成功時に呼び出す
    [PunRPC]
    public void Expectation()
    {
        methods.CanExpect();
    }


    //ボタン

    public void OnPowerButton()
    {

        //Debug.Log("物理を選択");

        //自分の攻撃ターンなら
        if (turn == playerId)
        {
            for (int i = 0; i < 2; i++)
            {
                if (playerId == 1)
                {
                    if (special_1[0, i] >= 1)
                    {
                        PowerAction[i].SetActive(true);
                    }
                }
                else if (playerId == 2)
                {
                    if (special_2[0, i] >= 1)
                    {
                        PowerAction[i].SetActive(true);
                    }
                }

            }
        }
        else
        {
            photonView.RPC(nameof(DefenceSet), RpcTarget.All, 0);
        }
    }

    //通常攻撃
    public void OnPowerButton_0()
    {
        photonView.RPC(nameof(AttackSet), RpcTarget.All, 0);
        Debug.Log("通常攻撃");


        EraseButton();

        
    }

    //かえん切り
    public void OnPowerButton_1()
    {
        photonView.RPC(nameof(AttackSet), RpcTarget.All, 1);
        Debug.Log("火炎斬り");

   
        EraseButton();
    }

    public void OnMagicButton()
    {

        //Debug.Log("魔法を選択");

        //自分の攻撃ターンなら
        if (turn == playerId)
        {
            for (int i = 0; i < 2; i++)
            {
                if (playerId == 1)
                {
                    if (special_1[1, i] >= 1)
                    {
                        MagicAction[i].SetActive(true);
                    }
                }
                else if (playerId == 2)
                {
                    if (special_2[1, i] >= 1)
                    {
                        MagicAction[i].SetActive(true);
                    }
                }

            }
        }
        else
        {
            photonView.RPC(nameof(DefenceSet), RpcTarget.All, 1);
        }
    }

    //通常攻撃
    public void OnMagicButton_0()
    {
        photonView.RPC(nameof(AttackSet), RpcTarget.All, 100);
        Debug.Log("通常攻撃");


        EraseButton();

    }

    //メラ
    public void OnMagicButton_1()
    {
        photonView.RPC(nameof(AttackSet), RpcTarget.All, 101);
        Debug.Log("メラ");


        EraseButton();
    }


    public void OnSupportButton()
    {
        Debug.Log("補助を選択");

        //自分の攻撃ターンなら
        if (turn == playerId)
        {
            for (int i = 0; i < 2; i++)
            {
                if (playerId == 1)
                {
                    if (special_1[2, i] >= 1)
                    {
                        SupportAction[i].SetActive(true);
                    }
                }
                else if (playerId == 2)
                {
                    if (special_2[2, i] >= 1)
                    {
                        SupportAction[i].SetActive(true);
                    }
                }

            }
        }
        else
        {
            photonView.RPC(nameof(DefenceSet), RpcTarget.All, 2);
        }
    }

    //攻撃アップ
    public void OnSupportButton_0()
    {
        photonView.RPC(nameof(AttackSet), RpcTarget.All, 200);
        Debug.Log("攻撃アップ");


        EraseButton();


    }

    //魔法アップ
    public void OnSupportButton_1()
    {
        photonView.RPC(nameof(AttackSet), RpcTarget.All, 201);
        Debug.Log("魔法アップ");


        EraseButton();
    }


    public void EraseButton()
    {
        for (int i = 0; i < 2; i++)
        {
            PowerAction[i].SetActive(false);
            MagicAction[i].SetActive(false);
            SupportAction[i].SetActive(false);


        }
    }



    //attack_numを送る
    [PunRPC]
    private void AttackSet(int select)
    {
        attack_num = select;

    }

    //specialを減らす
    [PunRPC]
    private void SpecialCountDown(int select)
    {
            //特技のカウントを減らす
            if (turn == 1)
            {
                special_1[select / 100, select % 100]--;
            }
            else if (turn == 2)
            {
                special_2[select / 100, select % 100]--;
            }
    }

    //defence_numを送る
    [PunRPC]
    private void DefenceSet(int select)
    {
        defence_num = select;
    }


    //turnを送る
    [PunRPC]
    private void TurnChange()
    {
        if (turn == 1)
        {
            turn = 2;
        }
        else if (turn == 2)
        {
            turn = 1;
        }
    }



}