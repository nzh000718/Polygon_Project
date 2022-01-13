using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
  项目：图形合成
  修改者：又又
  修改时间：2021.12/28 
 
     */
//添加枚举类型 图形
public enum PolygonType
{
    One=0,
    Two=1,
    Three=2,
    Four=3,
    Five=4,
    Six=5,
    Seven=6,
    Eight=7,
    Nine=8,
    The=9,
    Eleven=10
}
/// <summary>
/// 图形状态
/// </summary>
public enum PolygonState
{
    /// <summary>
    /// 准备
    /// </summary>
    Ready=0,
    /// <summary>
    /// 待命
    /// </summary>
    Standby = 1,
    /// <summary>
    /// 掉落
    /// </summary>
    Dropping=2,
    /// <summary>
    /// 碰撞
    /// </summary>
    Collision=3,
}
/// <summary>
/// Polygon图形管理类
/// </summary>
public class NZHPolygon : MonoBehaviour
{ 
    /// <summary>
    /// 边界数值
    /// </summary>
    public float Imit_x = 2f;
    /// <summary>
    /// 水果枚举数值
    /// </summary>
    public PolygonType PolygonType = PolygonType.One;  //给PolygonType添加数值
    /// <summary>
    /// 是否移动
    /// </summary>
    private bool isMove = false;
    /// <summary>
    /// 水果状态
    /// </summary>
    public PolygonState PolygonState = PolygonState.Ready;
    /// <summary>
    /// 初始尺寸
    /// </summary>
    public Vector3 cinitialScale = Vector3.zero;
    /// <summary>
    /// 变换速度
    /// </summary>
    public float scaleSpeed = 0.1f;
    /// <summary>
    /// 图形分数
    /// </summary>
    public float PolygonScore = 1f;
    private void Awake()
    {
        cinitialScale = new Vector3(1f, 1f, 1f);//初始尺寸
    }
    private void Update()
    {
        //游戏状态为Standby&图形状态Standby，可以说鼠标点击移动，以及松开鼠标跌落
        if(GameManager.gameManagerInstance.gameState==GameState.Standby&&PolygonState== PolygonState.Standby)
        {

            if (Input.GetMouseButtonDown(0))//点击鼠标
            {
                isMove = true;
            }
            if (Input.GetMouseButtonUp(0) && isMove == true)//松开按键
            {
                isMove = false;
                //改变重力，让图形掉落
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                //图形状态
                PolygonState = PolygonState.Dropping;
                //游戏状态
                GameManager.gameManagerInstance.gameState = GameState.InProgress;
                //创建待命图形
                GameManager.gameManagerInstance.InvokeCreatepolygon(0.6f);
            }
            if (isMove == true)
            {
                //移动位置
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//屏幕着标转换为unity世界坐标
                this.gameObject.GetComponent<Transform>().position = new Vector3(mousePos.x, this.gameObject.GetComponent<Transform>().position.y, this.gameObject.GetComponent<Transform>().position.z);
            }
        }
        //x方向做限制 使Fruit不越过两边
        if (this.transform.position.x > Imit_x)
        {
            this.transform.position = new Vector3(Imit_x, this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.x < -Imit_x)
        {
            this.transform.position = new Vector3(-Imit_x, this.transform.position.y, this.transform.position.z);
        }
        //尺寸回复
        if (this.transform.localScale.x< cinitialScale.x)
        {
            this.transform.localScale += new Vector3(1, 1, 1) * scaleSpeed;//恢复数度
        }
        if (this.transform.localScale.x > cinitialScale.x)
        {
            this.transform.localScale = cinitialScale;//保持初始尺寸
        }
    }
    /// <summary>
    /// 触碰反馈
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PolygonState == PolygonState.Dropping)
        {
            Debug.Log("collider");
            if (collision.gameObject.tag.Contains("Floor"))//接触地面 tag
            {
                GameManager.gameManagerInstance.gameState = GameState.Standby;//游戏状态 待命
                PolygonState = PolygonState.Collision;//图形状态 待命

                GameManager.gameManagerInstance.hitSoure.Play();
            }
            if (collision.gameObject.tag.Contains("Polygon"))//接触图形 tag
            {
                GameManager.gameManagerInstance.gameState = GameState.Standby;//游戏状态 待命
                PolygonState = PolygonState.Collision;//图形状态 待命
            }
        }
        //Dropping Collision,可以进行融合
        if ((int)PolygonState>=(int)PolygonState.Dropping)
        {
            if (collision.gameObject.tag.Contains("Polygon"))
            {
                if (PolygonType==collision.gameObject.GetComponent<NZHPolygon>().PolygonType&&PolygonType!=PolygonType.Eleven)
                {
                    //限制只执行一次合成
                    float thisPosxy = this.transform.position.x + this.transform.position.y;//this x+y 对比
                    float collisionPosxy = collision.transform.position.x + collision.transform.position.y;//collision x+y 对比
                    if (thisPosxy>collisionPosxy)
                    {
                        //合成，生成新的水果(大一号)，尺寸由小变大
                        //两个位置信息
                        GameManager.gameManagerInstance.CombineNewPolygon(PolygonType, this.transform.position, collision.transform.position);//创建
                        GameManager.gameManagerInstance.TotalScore += PolygonScore;//加分
                        GameManager.gameManagerInstance.totalScore.text = GameManager.gameManagerInstance.TotalScore.ToString();//显示加分
                        Destroy(this.gameObject);//清除当前对象
                        Destroy(collision.gameObject);//清除碰撞对象
                    }
                }
            }
        }
    }
}
