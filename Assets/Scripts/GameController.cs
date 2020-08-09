using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviourPunCallbacks
{

    GameObject Player1;
    GameObject Player2;

    //画面サイズ
    float width = 5.5f;
    float height = 9.5f;

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
    //初期値1
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

    //選択が終わるとtrue
    bool turnend_1 = false;
    bool turnend_2 = false;


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
        Yajirushi.transform.position = new Vector3(-width / 3f, Yajirushi.transform.position.y, 0f);

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

        //お互い選択終了
        //プレイヤー１のみでRPCで実行
        if (playerId == 1)
        {
            if (turnend_1 && turnend_2)
            {
                photonView.RPC(nameof(StartAction), RpcTarget.All);

            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("monselect1:" + monselect_1);
            Debug.Log("monselect2:" + monselect_2);

            for (int i = 0; i < 3; i++)
            {
                Debug.Log("mon1" + mon1[i]);
            }

            //Debug.Log(mon1);
            //Debug.Log(mon2);

            Debug.Log("turn" + turn);


        }

        //派遣
        
        if (select == 1)
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
        }


        if (select == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                if (-height / 4f - 0.5f < worldPos.y && worldPos.y < 0)
                {
                    if (worldPos.x < -width / 6f)
                    {
                        Yajirushi.transform.position = new Vector3(-width / 3f, Yajirushi.transform.position.y, 0f);

                        if (playerId == 1)
                        {
                            monselect_1 = 1;
                        }
                        else if (playerId == 2)
                        {
                            monselect_2 = 1;
                        }


                    }
                    else if (-width / 6f <= worldPos.x && worldPos.x <= width / 6f)
                    {
                        Yajirushi.transform.position = new Vector3(0f, Yajirushi.transform.position.y, 0f);

                        if (playerId == 1)
                        {
                            monselect_1 = 2;
                        }
                        else if (playerId == 2)
                        {
                            monselect_2 = 2;
                        }
                    }
                    else if (width / 6f < worldPos.x)
                    {
                        Yajirushi.transform.position = new Vector3(width / 3f, Yajirushi.transform.position.y, 0f);

                        if (playerId == 1)
                        {
                            monselect_1 = 3;
                        }
                        else if (playerId == 2)
                        {
                            monselect_2 = 3;
                        }
                    }

                    //Debug.Log(monselect_1);
                }
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
                neko.transform.position = new Vector3(-1.82f + 1.82f*i, -4f, 0f);
            }

            AttackButton.SetActive(false);
            DispatchButton.SetActive(false);
            selectPos.SetActive(true);
        }

        //防御ターンなら
        else
        {
            selectPos.SetActive(false);
            Yajirushi.SetActive(false);
            photonView.RPC(nameof(TurnEnd), RpcTarget.All, playerId);

        }

    }

    //攻撃or防御を選択
    public void OnAttackButton()
    {
        select = 2;

        AttackButton.SetActive(false);
        DispatchButton.SetActive(false);
        selectPos.SetActive(true);
        Yajirushi.SetActive(true);

        DecisionButton.SetActive(true);


        //初期値入力
        if (playerId == 1)
        {
            monselect_1 = 1;
        }
        else if (playerId == 2)
        {
            monselect_2 = 1;
        }
       
    }

    //決定ボタン
    public void OnDecisionButton()
    {
        //派遣
        if (select == 1)
        {
            if (playerId == 1)
            {
                photonView.RPC(nameof(Dispatch), RpcTarget.All, mon1);
                //Debug.Log("aaa");
            }
            else if (playerId == 2)
            {
                photonView.RPC(nameof(Dispatch), RpcTarget.All, mon2);
            }

            nekocount = 0;

        }

        //攻撃or防御
        if (select == 2)
        {
            if (playerId == 1)
            {
                photonView.RPC(nameof(MonSelect), RpcTarget.All, playerId, monselect_1);
            }
            else if (playerId == 2)
            {
                photonView.RPC(nameof(MonSelect), RpcTarget.All, playerId, monselect_2);
            }
        }

        selectPos.SetActive(false);
        Yajirushi.SetActive(false);
        DecisionButton.SetActive(false);

        photonView.RPC(nameof(TurnEnd), RpcTarget.All, playerId);



    }


    //派遣人数共有
    [PunRPC]
    public void Dispatch(int[] mon)
    {
        for (int i = 0; i < 3; i++)
        {
            if (turn == 1)
            {
                mon1[i] = mon[i];
            }
            else if (turn == 2)
            {
                mon2[i] = mon[i];
            }
        }


    }


    //攻撃or防御
    //選択共有
    [PunRPC]
    public void MonSelect(int id, int mon)
    {
        if (id == 1)
        {
            monselect_1 = mon;
        }
        else if (id == 2)
        {
            monselect_2 = mon;
        }
    }

    //turnendを送る
    [PunRPC]
    private void TurnEnd(int id)
    {
        if (id == 1)
        {
            turnend_1 = true;
        }
        else if (id == 2)
        {
            turnend_2 = true;
        }
    }

    //次のturnへ
    private void TurnChange()
    {
        if (turn == 1)
        {
            turn = 2;
            //Debug.Log("turn" + turn);

        }
        else if (turn == 2)
        {
            turn = 1;
        }




    }

    //ボタンを表示
    [PunRPC]
    private void ShowButton()
    {
        DispatchButton.SetActive(true);
        AttackButton.SetActive(true);
    }


    //ボタンテキストの変更
    [PunRPC]
    private void ButtonTextChange()
    {
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
    }




    [PunRPC]
    private void StartAction()
    {
        //攻撃側の時
        if (turn == playerId)
        {
            //プレイヤー１
            //相手が待機を選択
            if (playerId == 1 && monselect_2 == 0)
            {
                Debug.Log("相手は様子を見ている");
            }
            //相手が防御を選択
            else if (playerId == 1 && monselect_2 != 0)
            {
                Debug.Log("相手は" + monselect_2 + "の門を閉めた");
            }

            //プレイヤー２
            //相手が待機を選択
            if (playerId == 2 && monselect_1 == 0)
            {
                Debug.Log("相手は様子を見ている");
            }
            //相手が防御を選択
            else if (playerId == 2 && monselect_1 != 0)
            {
                Debug.Log("相手は" + monselect_1 + "の門を閉めた");
            }

            //兵を派遣した場合
            if (select == 1)
            {
                Debug.Log("兵を派遣した");

            }

            //攻撃した場合
            if (select == 2)
            {
                //プレイヤー１
                if (playerId == 1 && monselect_1 == monselect_2)
                {
                    Debug.Log("攻撃が防がれてしまった！");
                }
                else if (playerId == 1 && monselect_1 != monselect_2)
                {
                    Debug.Log("相手に" + mon1[monselect_1 - 1] + "のダメージを与えた！");
                }

                //攻撃した場合
                //プレイヤー２
                if (playerId == 2 && monselect_1 == monselect_2)
                {
                    Debug.Log("攻撃が防がれてしまった！");
                }
                else if (playerId == 2 && monselect_1 != monselect_2)
                {
                    Debug.Log("相手に" + mon2[monselect_2 - 1] + "のダメージを与えた！");
                }
            }




        }

        //防御側の時
        else
        {
            //プレイヤー１
            //待機を選択
            if (playerId == 1 && monselect_1 == 0)
            {
                Debug.Log("様子を見ることにした。");
            }
            //防御を選択
            else if (playerId == 1 && monselect_1 != 0)
            {
                Debug.Log(monselect_1 + "の門を閉めた");
            }

            //プレイヤー2
            //待機を選択
            if (playerId == 2 && monselect_2 == 0)
            {
                Debug.Log("様子を見ることにした。");
            }
            //防御を選択
            else if (playerId == 2 && monselect_2 != 0)
            {
                Debug.Log(monselect_2 + "の門を閉めた");
            }


            //相手が兵を派遣した場合
            //プレイヤー１
            if (playerId == 1 && monselect_2 == 0)
            {
                Debug.Log("相手は兵を派遣した");
            }
            else if (playerId == 2 && monselect_1 == 0)
            {
                Debug.Log("相手は兵を派遣した");
            }

            //相手が攻撃してきた場合

            //プレイヤー１
            if (playerId == 1 && monselect_2 != 0)
            {
                if (monselect_1 == monselect_2)
                {
                    Debug.Log("攻撃を防いだ！");
                }
                else
                {
                    Debug.Log(mon2[monselect_2 - 1] + "のダメージを受けた！");
                }
            }

            //プレイヤー２
            if (playerId == 2 && monselect_1 != 0)
            {
                if (monselect_1 == monselect_2)
                {
                    Debug.Log("攻撃を防いだ！");
                }
                else
                {
                    Debug.Log(mon1[monselect_1 - 1] + "のダメージを受けた！");
                }
            }

        }

        //初期化
        monselect_1 = 0;
        monselect_2 = 0;
        select = 0;
        turnend_1 = false;
        turnend_2 = false;



        TurnChange();

        photonView.RPC(nameof(ShowButton), RpcTarget.All);

        photonView.RPC(nameof(ButtonTextChange), RpcTarget.All);

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






}