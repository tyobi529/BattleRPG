using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviourPunCallbacks
{

    GameObject Player1;
    GameObject Player2;

    //画面サイズ
    float width = 18f;
    float height = 10f;

    //private Vector3 screenPoint;
    //private Vector3 offset;


    int hp_1 = 500;
    public int[] mon1 = new int[3] { 0, 0, 0 };
    //int power_1 = 100;
    //int magic_1 = 50;

    int hp_2 = 500;
    public int[] mon2 = new int[3] { 0, 0, 0 };

    public int nekocount = 0;

    GameObject AttackButton;
    GameObject DispatchButton;
    GameObject DecisionButton;




    Text AttackButtonText;
    Text DispatchButtonText;

    //猫をおく位置表示
    GameObject selectPos;

    //矢印
    public GameObject Yajirushi;


    //攻撃側
    //1:派遣、2:攻撃
    //防御側
    //1:待機、2:防御
    int select = 0;
    //int select_2 = 0;

    //攻撃か防御で選択する門
    int monselect_1 = 0;
    int monselect_2 = 0;

    //int power_2 = 50;
    //int magic_2 = 100;

    //０の時は選択待ち
    //attackは
    //    1~99:物理
    //    100~199:魔法
    //    200~299:補助
    //int attack_num = -1;

    //0：物理
    //1：魔法
    //2：補助
    //int defence_num = -1;
    //bool turnend = false;

    //int[] count = new int[3] { 0, 0, 0 };


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
    //public int[,] special_1 = new int[3, 2];

    //public int[,] special_2 = new int[3, 2];

    //特技カウント用テキスト
    //Text specialCount

    public int turn = 1;

    public GameObject NekoPrefab;

    //そのターンに配置した猫の数
    //public int nekocount = 0;
    //public int[] nekocount = new int[3] { 0, 0, 0 };



    Methods methods;

    private GameObject TextController;

    private void Start()
    {
        //プレイヤーのid入力
        playerId = PhotonNetwork.LocalPlayer.ActorNumber;

        //テキスト表示オブジェクト
        TextController = GameObject.Find("TextController");

        //攻撃、派遣選択ボタン
        AttackButton = GameObject.Find("AttackButton");
        DispatchButton = GameObject.Find("DispatchButton");
        DecisionButton = GameObject.Find("DecisionButton");

        AttackButton.GetComponent<Button>().onClick.AddListener(OnAttackButton);
        DispatchButton.GetComponent<Button>().onClick.AddListener(OnDispatchButton);
        DecisionButton.GetComponent<Button>().onClick.AddListener(OnDecisionButton);

        DecisionButton.SetActive(false);


        AttackButtonText = GameObject.Find("AttackButtonText").GetComponent<Text>();
        DispatchButtonText = GameObject.Find("DispatchButtonText").GetComponent<Text>();


        if (turn == playerId)
        {
            AttackButtonText.text = "こうげきする";
            DispatchButtonText.text = "派遣する";
        }
        else
        {
            AttackButtonText.text = "ぼうぎょする";
            DispatchButtonText.text = "待機する";
        }

        selectPos = GameObject.Find("selectPos");
        selectPos.SetActive(false);


        Yajirushi = GameObject.Find("Yajirushi");
        Yajirushi.SetActive(false);


        //Debug.Log(nekocount[0]);
        //Debug.Log(mon1[0]);


        //count[0] = 1;
        //Debug.Log(count[2]);
        //foreach(var a in count)
        //{
        //    Debug.Log(a);
        //}

        //Debug.Log("aa");

        //foreach(var a in mon1)
        //{
        //    Debug.Log(a);
        //}
        //関数まとめ
        //methods = this.GetComponent<Methods>();


        //攻守を表示
        //TextController.GetComponent<TextController>().ShowTurn(playerId, turn);


    }


    private void Update()
    {

        if (playerId == turn)
        {
            if (nekocount == 3)
            {
                DecisionButton.SetActive(true);
            }
            else
            {
                DecisionButton.SetActive(false);
            }
        }

        if (select == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                if (height / 6f < worldPos.y)
                {
                    Yajirushi.transform.position = new Vector3(Yajirushi.transform.position.x, height / 3f, 0f);

                    if (playerId == 1)
                    {
                        monselect_1 = 1;
                    }
                    else if (playerId == 2)
                    {
                        monselect_2 = 1;
                    }


                }
                else if (-height / 6f <= worldPos.y && worldPos.y <= height / 6f)
                {
                    Yajirushi.transform.position = new Vector3(Yajirushi.transform.position.x, 0f, 0f);

                    if (playerId == 1)
                    {
                        monselect_1 = 2;
                    }
                    else if (playerId == 2)
                    {
                        monselect_2 = 2;
                    }
                }
                else if (worldPos.y < -height / 6f)
                {
                    Yajirushi.transform.position = new Vector3(Yajirushi.transform.position.x, -height / 3f, 0f);

                    if (playerId == 1)
                    {
                        monselect_1 = 3;
                    }
                    else if (playerId == 2)
                    {
                        monselect_2 = 3;
                    }
                }

                Debug.Log(monselect_1);
            }


        }


        //テキスト表示
        //一旦Updateで定義
        //if (playerId == 1)
        //{
        //    TextController.GetComponent<TextController>().TextUpdate(hp_1, power_1, magic_1, hp_2, power_2, magic_2);
        //    TextController.GetComponent<TextController>().SpecialTextUpdate(special_1, special_2);

        //}
        //else if (playerId == 2)
        //{
        //    TextController.GetComponent<TextController>().TextUpdate(hp_2, power_2, magic_2, hp_1, power_1, magic_1);
        //    TextController.GetComponent<TextController>().SpecialTextUpdate(special_2, special_1);

        //}


        ////相手の選択待ち
        //if ((attack_num != -1 && turn == playerId) || (defence_num != -1 && turn != playerId))
        //{
        //    TextController.GetComponent<TextController>().WaitText();
        //}


        //if (attack_num != -1 && defence_num != -1)
        //{
        //    //予測判定用
        //    int judge = attack_num / 100;
        //    //予測成功
        //    if (judge == defence_num)
        //    {
        //        Debug.Log("予測成功");

        //        photonView.RPC(nameof(Expectation), RpcTarget.All);
        //    }

        //    else
        //    {
        //        //プレイヤー１なら
        //        if (turn == 1)
        //        {
        //            photonView.RPC(nameof(Domethod_1), RpcTarget.All);
        //        }

        //        //プレイヤー２なら
        //        else if (turn == 2)
        //        {
        //            photonView.RPC(nameof(Domethod_2), RpcTarget.All);
        //        }
        //    }

        //    photonView.RPC(nameof(SpecialCountDown), RpcTarget.All, attack_num);


        //    photonView.RPC(nameof(AttackSet), RpcTarget.All, -1);
        //    photonView.RPC(nameof(DefenceSet), RpcTarget.All, -1);

        //    photonView.RPC(nameof(TurnChange), RpcTarget.All);



        //}

    }

    //void OnMouseDown()
    //{
    //    this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
    //    this.offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    //}

    //派遣を選択
    public void OnDispatchButton()
    {
        select = 1;
        //攻撃ターンなら
        if (turn == playerId)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject neko = Instantiate(NekoPrefab) as GameObject;
                neko.transform.position = new Vector3(-7.0f, 3.0f - 3.0f * i, 0f);
            }

            AttackButton.SetActive(false);
            DispatchButton.SetActive(false);
            selectPos.SetActive(true);
        }

        //防御ターンなら
        else
        {

        }

    }

    //攻撃を選択
    public void OnAttackButton()
    {
        select = 2;

        selectPos.SetActive(true);
        Yajirushi.SetActive(true);


    }

    //決定ボタン
    public void OnDecisionButton()
    {

    }





    //攻撃番号を入れて関数を呼び出す。
    //プレイヤー１から攻撃
    //[PunRPC]
    //public void Domethod_1()
    //{
    //    //物理
    //    if (attack_num == 0)
    //    {
    //        methods.Power_0(ref hp_2, power_1, turn, playerId);
    //    }

    //    else if (attack_num == 1)
    //    {
    //        methods.Power_1(ref hp_2, power_1, turn, playerId);
    //    }


    //    //魔法
    //    else if (attack_num == 100)
    //    {
    //        methods.Magic_0(ref hp_2, magic_1, turn, playerId);
    //    }

    //    else if (attack_num == 101)
    //    {
    //        methods.Magic_1(ref hp_2, magic_1, turn, playerId);
    //    }


    //    //補助
    //    else if (attack_num == 200)
    //    {
    //        methods.Support_0(ref power_1, turn, playerId);
    //    }

    //    else if (attack_num == 201)
    //    {
    //        methods.Support_1(ref magic_1, turn, playerId);
    //    }
    //}

    ////プレイヤー2から攻撃
    //[PunRPC]
    //public void Domethod_2()
    //{
    //    //物理
    //    if (attack_num == 0)
    //    {
    //        methods.Power_0(ref hp_1, power_2, turn, playerId);
    //    }

    //    else if (attack_num == 1)
    //    {
    //        methods.Power_1(ref hp_1, power_2, turn, playerId);
    //    }


    //    //魔法
    //    else if (attack_num == 100)
    //    {
    //        methods.Magic_0(ref hp_1, magic_2, turn, playerId);
    //    }

    //    else if (attack_num == 101)
    //    {
    //        methods.Magic_1(ref hp_1, magic_2, turn, playerId);
    //    }

    //    //補助
    //    else if (attack_num == 200)
    //    {
    //        methods.Support_0(ref power_2, turn, playerId);
    //    }

    //    else if (attack_num == 201)
    //    {
    //        methods.Support_1(ref magic_2, turn, playerId);
    //    }
    //}

    ////予測成功時に呼び出す
    //[PunRPC]
    //public void Expectation()
    //{
    //    methods.CanExpect();
    //}


    ////ボタン

    //public void OnPowerButton()
    //{

    //    //Debug.Log("物理を選択");

    //    //自分の攻撃ターンなら
    //    if (turn == playerId)
    //    {
    //        for (int i = 0; i < 2; i++)
    //        {
    //            if (playerId == 1)
    //            {
    //                if (special_1[0, i] >= 1)
    //                {
    //                    PowerAction[i].SetActive(true);
    //                }
    //            }
    //            else if (playerId == 2)
    //            {
    //                if (special_2[0, i] >= 1)
    //                {
    //                    PowerAction[i].SetActive(true);
    //                }
    //            }

    //        }
    //    }
    //    else
    //    {
    //        photonView.RPC(nameof(DefenceSet), RpcTarget.All, 0);
    //    }
    //}

    ////通常攻撃
    //public void OnPowerButton_0()
    //{
    //    photonView.RPC(nameof(AttackSet), RpcTarget.All, 0);
    //    Debug.Log("通常攻撃");


    //    EraseButton();


    //}

    ////かえん切り
    //public void OnPowerButton_1()
    //{
    //    photonView.RPC(nameof(AttackSet), RpcTarget.All, 1);
    //    Debug.Log("火炎斬り");


    //    EraseButton();
    //}

    //public void OnMagicButton()
    //{

    //    //Debug.Log("魔法を選択");

    //    //自分の攻撃ターンなら
    //    if (turn == playerId)
    //    {
    //        for (int i = 0; i < 2; i++)
    //        {
    //            if (playerId == 1)
    //            {
    //                if (special_1[1, i] >= 1)
    //                {
    //                    MagicAction[i].SetActive(true);
    //                }
    //            }
    //            else if (playerId == 2)
    //            {
    //                if (special_2[1, i] >= 1)
    //                {
    //                    MagicAction[i].SetActive(true);
    //                }
    //            }

    //        }
    //    }
    //    else
    //    {
    //        photonView.RPC(nameof(DefenceSet), RpcTarget.All, 1);
    //    }
    //}

    ////通常攻撃
    //public void OnMagicButton_0()
    //{
    //    photonView.RPC(nameof(AttackSet), RpcTarget.All, 100);
    //    Debug.Log("通常攻撃");


    //    EraseButton();

    //}

    ////メラ
    //public void OnMagicButton_1()
    //{
    //    photonView.RPC(nameof(AttackSet), RpcTarget.All, 101);
    //    Debug.Log("メラ");


    //    EraseButton();
    //}


    //public void OnSupportButton()
    //{
    //    Debug.Log("補助を選択");

    //    //自分の攻撃ターンなら
    //    if (turn == playerId)
    //    {
    //        for (int i = 0; i < 2; i++)
    //        {
    //            if (playerId == 1)
    //            {
    //                if (special_1[2, i] >= 1)
    //                {
    //                    SupportAction[i].SetActive(true);
    //                }
    //            }
    //            else if (playerId == 2)
    //            {
    //                if (special_2[2, i] >= 1)
    //                {
    //                    SupportAction[i].SetActive(true);
    //                }
    //            }

    //        }
    //    }
    //    else
    //    {
    //        photonView.RPC(nameof(DefenceSet), RpcTarget.All, 2);
    //    }
    //}

    ////攻撃アップ
    //public void OnSupportButton_0()
    //{
    //    photonView.RPC(nameof(AttackSet), RpcTarget.All, 200);
    //    Debug.Log("攻撃アップ");


    //    EraseButton();


    //}

    ////魔法アップ
    //public void OnSupportButton_1()
    //{
    //    photonView.RPC(nameof(AttackSet), RpcTarget.All, 201);
    //    Debug.Log("魔法アップ");


    //    EraseButton();
    //}


    //public void EraseButton()
    //{
    //    for (int i = 0; i < 2; i++)
    //    {
    //        PowerAction[i].SetActive(false);
    //        MagicAction[i].SetActive(false);
    //        SupportAction[i].SetActive(false);


    //    }
    //}



    ////attack_numを送る
    //[PunRPC]
    //private void AttackSet(int select)
    //{
    //    attack_num = select;

    //}

    ////specialを減らす
    //[PunRPC]
    //private void SpecialCountDown(int select)
    //{
    //        //特技のカウントを減らす
    //        if (turn == 1)
    //        {
    //            special_1[select / 100, select % 100]--;
    //        }
    //        else if (turn == 2)
    //        {
    //            special_2[select / 100, select % 100]--;
    //        }
    //}

    ////defence_numを送る
    //[PunRPC]
    //private void DefenceSet(int select)
    //{
    //    defence_num = select;
    //}


    ////turnを送る
    //[PunRPC]
    //private void TurnChange()
    //{
    //    if (turn == 1)
    //    {
    //        turn = 2;
    //    }
    //    else if (turn == 2)
    //    {
    //        turn = 1;
    //    }
    //}



}