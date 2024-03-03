using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwiperSystem : MonoBehaviour
{
    public GameObject scrollbar;
    float scrollPosition = 0;
    float[] position;

    void Update()
    {
        position = new float[transform.childCount];
        float distance = 1f / (position.Length - 1f);

        for (int i = 0; i < position.Length; i++)
        {
            position[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scrollPosition = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (scrollPosition < position[i] + (distance / 2) && scrollPosition > position[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, position[i], 0.01f);
                }
            }
        }

        for (int i = 0; i < position.Length; i++)
        {
            if (scrollPosition < position[i] + (distance / 2) && scrollPosition > position[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.01f);
                for (int j = 0; j < position.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.9f, 0.9f), 0.01f);
                    }
                }
            }
        }
    }
}
