using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// gif图运动
/// </summary>
public class GIF1 : MonoBehaviour
{
    public Sprite[] frames;
    public bool loop = false;
    public float frequency = 0.2f;
    float calTime = 0f;
    int index = 0;
    SpriteRenderer img_spritr;

    void Start()
    {
        img_spritr = GetComponent<SpriteRenderer>();
        img_spritr.sprite = frames[index];

    }
    void Update()
    {
        calTime += Time.deltaTime;
        if (calTime > frequency)
        {
            calTime = 0;

            if (!loop)
            {
                if (index == frames.Length - 1)
                {
                    index = 0;

                }
                img_spritr.sprite = frames[++index];
            }

            else
            {
                img_spritr.sprite = frames[index++];
                if (index == frames.Length - 1)
                {
                    // Destroy(this.gameObject);
                    index = 0;
                }
            }
        }
    }
    IEnumerator Animation()
    {
        while (true)
        {
            yield return new WaitForSeconds(frequency);
            img_spritr.sprite = frames[++index];
            if (index == frames.Length - 1)
            {
                if (!loop)
                {
                    yield break;
                }
                else
                {
                    index = 0;
                }
            }
        }
    }
}
