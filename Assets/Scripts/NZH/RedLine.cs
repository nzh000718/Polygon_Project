using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
项目：图形合成
修改者：又又
修改时间：2021.12/28
*/
///<summary>
///RedLine 触发器
///<summary>
public class RedLine : MonoBehaviour
{
    /// <summary>
    /// 石是否移动
    /// </summary>
    public bool isMove = false;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float seep = 0.05f;
    /// <summary>
    /// 移动范围
    /// </summary>
    public float inmit_y = -4f;
    private void Update()
    {
        if(isMove)
        {
            if (this.transform.position.y> inmit_y)//限制
            {
                this.transform.Translate(Vector3.down * seep);//移动
            }
            else
            {
                isMove = false;
                Invoke("ReLoadScene", 1f);
                //重新加载
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Polygon"))//触碰图形
        {
            if ((int)GameManager.gameManagerInstance.gameState<(int)GameState.GameOver)
                //游戏状态小于Over
            {  
                if (collision.gameObject.GetComponent<NZHPolygon>().PolygonState == PolygonState.Collision)//并且是Collision状态的图形
                {
                    Debug.Log("红线向下");
                    //GameOver
                    GameManager.gameManagerInstance.gameState = GameState.GameOver;
                    Invoke("OpenMoveAndCalculateScore", 0.5f);//延迟移动
                }
            }
            //计算分数                                                    //销毁剩余的图形，计算分数
            if (GameManager.gameManagerInstance.gameState==GameState.CalculateScore)
            {
                float currentScore = collision.GetComponent<NZHPolygon>().PolygonScore;//获取分数
                GameManager.gameManagerInstance.TotalScore += currentScore;//添加剩余分数
                GameManager.gameManagerInstance.totalScore.text = GameManager.gameManagerInstance.TotalScore.ToString();//显示分数
                Destroy(collision.gameObject);//销毁图形
            }
        }
    }
    /// <summary>
    /// 打开移动状态并且gameState状态变为CalculateScore
    /// </summary>
    void OpenMoveAndCalculateScore()
    {
        isMove = true;
        GameManager.gameManagerInstance.gameState = GameState.CalculateScore;
    }
    /// <summary>
    /// 加载场景
    /// </summary>
    void ReLoadScene()
    {
        float highestScore = PlayerPrefs.GetFloat("HighestScore");//本地储存 键值对
        if (highestScore< GameManager.gameManagerInstance.TotalScore)
        {
            PlayerPrefs.SetFloat("HighestScore", GameManager.gameManagerInstance.TotalScore);
        }
        SceneManager.LoadScene("NZHGame");//场景加载
    }
}
