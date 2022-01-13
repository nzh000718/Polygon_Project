using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
项目：图形合成
修改者：又又
修改时间：2021.12/28

*/
/// <summary>
/// 游戏状态
/// </summary>
public enum GameState
{
    /// <summary>
    /// 准备
    /// </summary>
    Ready = 0,
    /// <summary>
    /// 待命
    /// </summary>
    Standby = 1,//Standby-InProgress
    /// <summary>
    /// 正在进行
    /// </summary>
    InProgress = 2,//InProgress-Standby
    /// <summary>
    /// 游戏结束
    /// </summary>
    GameOver = 3,
    /// <summary>
    /// 计算分数
    /// </summary>
    CalculateScore = 4,
}
///<summary>
///游戏管理类
///<summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 产生图形位置
    /// </summary>
    public GameObject bornPolygonPosition;
    /// <summary>
    /// 游戏图形对象集合
    /// </summary>
    public GameObject[] polygonList;
    /// <summary>
    /// 开始按钮
    /// </summary>
    public GameObject startBotton;
    /// <summary>
    /// 静态的实例，可以在直接别的类中使用
    /// </summary>
    public static GameManager gameManagerInstance;
    /// <summary>
    /// 游戏状态
    /// </summary>
    public GameState gameState = GameState.Ready;
    /// <summary>
    /// 合成尺寸
    /// </summary>
    public Vector3 combineScale = new Vector3(0, 0, 0);//0.3
    /// <summary>
    /// 总分
    /// </summary>
    public float TotalScore = 0f;
    /// <summary>
    /// 显示分数
    /// </summary>
    public Text totalScore;
    /// <summary>
    /// 历史最高分
    /// </summary>
    public Text highestScoreText;
    /// <summary>
    /// 合成音效
    /// </summary>
    public AudioSource combimeSource;
    /// <summary>
    /// 落地音效
    /// </summary>
    public AudioSource hitSoure;
    /// <summary>
    /// 获取物体
    /// </summary>
    public GameObject gifBotton0, gifBotton1,yY;
    /// <summary>
    /// 按钮状态
    /// </summary>
    public bool isButton = false;
    private void Awake()
    {
        gameManagerInstance = this;
    }
    /// <summary>
    /// 游戏开始
    /// </summary>
    public void StartGame()
    {
        Debug.Log("start");
        float highestScore = PlayerPrefs.GetFloat("HighestScore");
        highestScoreText.text = "历史最高：" + highestScore;
        CreatePolygon();
        gameState = GameState.Standby;//点击鼠标后
        startBotton.SetActive(false);//隐藏开始图标
    }
    /// <summary>
    /// 延迟调用
    /// </summary>
    /// <param name="invokeTime">延迟时间</param>
    public void InvokeCreatepolygon(float invokeTime)
    {
        Invoke("CreatePolygon", invokeTime);
    }
    /// <summary>
    ///创建随机图形
    /// </summary>
    public void CreatePolygon()
    {
        int index = Random.Range(0, 5);//随机0 1 2 3 4
        if (polygonList.Length >= index && polygonList[index] != null)
        {
            GameObject PolygonObj = polygonList[index];//随机图形对象
            //实例化克隆体对象
            var currentPolygon = Instantiate(PolygonObj, bornPolygonPosition.transform.position, PolygonObj.transform.rotation);
            currentPolygon.GetComponent<NZHPolygon>().PolygonState = PolygonState.Standby;//图形状态
        }
    }
    /// <summary>
    /// 合成图形创建
    /// </summary>
    /// <param name="currentFruitTye">当前碰撞图形类型</param>
    /// <param name="currentPos">图形位置</param>
    /// <param name="collisionPos">触发图形位置</param>
    public void CombineNewPolygon(PolygonType currentPolygonTye, Vector3 currentPos, Vector3 collisionPos)
    {
        Vector3 centerpos = (currentPos + collisionPos) / 2;//合成图形中心位置
        int index = (int)currentPolygonTye + 1;//大一号索引
        GameObject combinePolygonObj = polygonList[index];//大一号
        var combinePolygon = Instantiate(combinePolygonObj, centerpos, combinePolygonObj.transform.rotation);//创建合成水果
        combinePolygon.GetComponent<Rigidbody2D>().gravityScale = 1f;//合成水果重力添加
        combinePolygon.GetComponent<NZHPolygon>().PolygonState = PolygonState.Collision;//水果状态
        combinePolygon.transform.localScale = combineScale;//合成变小
        combimeSource.Play();//合成音效开启
    }
    public void GifButton0()//GifButton
    {
        if (gameState == GameState.Ready && isButton == false)
        {
            isButton = true;
            Debug.Log("gif1");
            yY.SetActive(true);//显示图标
            startBotton.SetActive(false);//隐藏开始图标  
            gifBotton1.SetActive(true);
            gifBotton0.SetActive(false);
        }
    }
    public void GifButton1()//GifButton
    {
        if (gameState == GameState.Ready && isButton == true)
        {
            isButton = false;
            Debug.Log("gif2");
            yY.SetActive(false);//显示图标
            startBotton.SetActive(true);//隐藏开始图标
            gifBotton0.SetActive(true);
            gifBotton1.SetActive(false);
        }
    }
}
