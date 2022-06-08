using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenningJumpImage : MonoBehaviour
{
    public RectTransform high;
    public RectTransform low;

    public Image image;

    public void JumpIn()
    {
        StartCoroutine("IJumpIn");
    }

    IEnumerator IJumpIn()
    {
        Vector2 vector2 = transform.position;
        Color color;
        for (float i = 0; i <= 1; i += 0.03f)
        {
            transform.position = Vector2.Lerp(vector2, high.position, i);

            color = image.color;
            color.a = i;
            image.color = color;

            yield return new WaitForSeconds(0.01f);
        }

        color = image.color;
        color.a = 1;
        image.color = color;

        vector2 = transform.position;

        for(float i = 0; i <= 1; i += 0.1f)
        {
            transform.position = Vector2.Lerp(vector2, low.position, i);

            color = image.color;
            color.a = 1 - i;
            image.color = color;

            yield return new WaitForSeconds(0.01f);
        }


        color = image.color;
        color.a = 0;
        image.color = color;
    }
}
