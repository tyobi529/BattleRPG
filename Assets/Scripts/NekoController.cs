﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekoController : MonoBehaviour
{
    Vector3 thisPos;


    //画面サイズ
    float width = 5.5f;
    float height = 9.5f;


    GameController gameController;

    //配置完了かどうか
    bool[] isok = new bool[3] {false, false, false};

    Vector3 firstPos;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController(Clone)").GetComponent<GameController>();

        firstPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        thisPos = this.transform.position;

        //範囲外
        if (thisPos.x < -width/2f || thisPos.x > width/2f || thisPos.y > -0.5f || this.thisPos.y < -height/2f)
        {
            this.transform.position = firstPos;
        }

        if (-height/4f < thisPos.y && thisPos.y < 0)
        {
            if (gameController.playerId == 1)
            {
                //赤
                if (thisPos.x < -width / 6f)
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(243f / 255f, 201f / 255f, 201f / 255f);

                    if (!isok[0])
                    {
                        gameController.mon1[0]++;
                        gameController.nekocount++;

                        isok[0] = true;

                        if (isok[1])
                        {
                            gameController.mon1[1]--;
                            gameController.nekocount--;
                            isok[1] = false;
                        }

                        if (isok[2])
                        {
                            gameController.mon1[2]--;
                            gameController.nekocount--;
                            isok[2] = false;
                        }

                    }

                }
                //青
                else if (-width / 6f <= thisPos.x && thisPos.x <= width / 6f)
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(145f / 255f, 212f / 255f, 214f / 255f);

                    if (!isok[1])
                    {
                        gameController.mon1[1]++;
                        gameController.nekocount++;

                        isok[1] = true;

                        if (isok[0])
                        {
                            gameController.mon1[0]--;
                            gameController.nekocount--;
                            isok[0] = false;
                        }

                        if (isok[2])
                        {
                            gameController.mon1[2]--;
                            gameController.nekocount--;
                            isok[2] = false;
                        }

                    }
                }
                //黄色
                else if (width / 6f < thisPos.x)
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(243f / 255f, 248f / 255f, 160f / 255f);

                    if (!isok[2])
                    {
                        gameController.mon1[2]++;
                        gameController.nekocount++;

                        isok[2] = true;

                        if (isok[0])
                        {
                            gameController.mon1[0]--;
                            gameController.nekocount--;
                            isok[0] = false;
                        }

                        if (isok[1])
                        {
                            gameController.mon1[1]--;
                            gameController.nekocount--;
                            isok[1] = false;
                        }

                    }
                }

            }


            //プレイヤー２
            else if (gameController.playerId == 2)
            {
                //赤
                if (thisPos.x < -width / 6f)
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(243f / 255f, 201f / 255f, 201f / 255f);

                    if (!isok[0])
                    {
                        gameController.mon2[0]++;
                        gameController.nekocount++;

                        isok[0] = true;

                        if (isok[1])
                        {
                            gameController.mon2[1]--;
                            gameController.nekocount--;
                            isok[1] = false;
                        }

                        if (isok[2])
                        {
                            gameController.mon2[2]--;
                            gameController.nekocount--;
                            isok[2] = false;
                        }

                    }

                }
                //青
                else if (-width / 6f <= thisPos.x && thisPos.x <= width / 6f)
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(145f / 255f, 212f / 255f, 214f / 255f);

                    if (!isok[1])
                    {
                        gameController.mon2[1]++;
                        gameController.nekocount++;

                        isok[1] = true;

                        if (isok[0])
                        {
                            gameController.mon2[0]--;
                            gameController.nekocount--;
                            isok[0] = false;
                        }

                        if (isok[2])
                        {
                            gameController.mon2[2]--;
                            gameController.nekocount--;
                            isok[2] = false;
                        }

                    }
                }
                //黄色
                else if (width / 6f < thisPos.x)
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(243f / 255f, 248f / 255f, 160f / 255f);

                    if (!isok[2])
                    {
                        gameController.mon2[2]++;
                        gameController.nekocount++;

                        isok[2] = true;

                        if (isok[0])
                        {
                            gameController.mon2[0]--;
                            gameController.nekocount--;
                            isok[0] = false;
                        }

                        if (isok[1])
                        {
                            gameController.mon2[1]--;
                            gameController.nekocount--;
                            isok[1] = false;
                        }

                    }
                }
            }




        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.white;

            for (int i = 0; i < 3; i++)
            {
                if (isok[i])
                {
                    if (gameController.playerId == 1)
                    {
                        gameController.mon1[i]--;
                    }
                    else if (gameController.playerId == 2)
                    {
                        gameController.mon2[i]--;
                    }
                    gameController.nekocount--;

                    isok[i] = false;
                    break;
                }
            }

        }


        //Debug.Log("1:" + gameController.mon1[0]);
        //Debug.Log("2:" + gameController.mon1[1]);
        //Debug.Log("3:" + gameController.mon1[2]);



    }
}
