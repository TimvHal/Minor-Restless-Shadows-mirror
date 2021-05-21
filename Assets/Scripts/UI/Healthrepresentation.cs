using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Healthrepresentation : MonoBehaviour
{
    public GameObject iconPrefab;
    public Sprite[] sprites;

    private Stack<KeyValuePair<GameObject, Vector3>> instantiated = new Stack<KeyValuePair<GameObject, Vector3>>();

    private int last = 1;
    // Start is called before the first frame update
    void Start()
    {
        var max = BuffManager.GetBuff(BuffManager.BuffType.health).currentValue;
        for (int i = 0; i < max/2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                var clone = Instantiate(iconPrefab);
                clone.transform.parent = transform;
                var pos =  new Vector3(0, 0, 0);
                pos.x += i * 20;
                clone.transform.localPosition = pos;
                clone.transform.localScale = new Vector3(0.3f, 0.3f, 1);
                instantiated.Push(new KeyValuePair<GameObject, Vector3>(clone, pos));
                var img = clone.GetComponent<Image>();
                img.sprite = sprites[j];
            }
        }
    }

    public void resetAll()
    {
        instantiated = new Stack<KeyValuePair<GameObject, Vector3>>();
        Start();
    } 
    public void ForceUpdate(float health)
    {
        DetermineHealth(health);
    }

    // Update is called once per frame
    public void DetermineHealth(float newHealth)
    {
        int ceiling = Convert.ToInt16(newHealth);
        if (ceiling == instantiated.Count +1)
        {
            HealOne();
            return;
        }

        if (ceiling < instantiated.Count)
        {
            TakeDamage();
            return;
        }
    }

    public void TakeDamage()
    {
        var heart = instantiated.Pop();
        last += last == 0 ? 1 : -1;
        Destroy(heart.Key);
    }

    public void HealOne()
    {
        var clone = Instantiate(iconPrefab);
        clone.transform.parent = transform;
        var pos = instantiated.Peek().Value;
        if (last == 1)
            pos.x += 20;
        clone.GetComponent<RectTransform>().localPosition = pos;
        clone.transform.localScale = new Vector3(0.3f, 0.3f, 1);
        instantiated.Push(new KeyValuePair<GameObject, Vector3>(clone, pos));
        last += last == 0 ? 1 : -1;
        clone.GetComponent<Image>().sprite = sprites[last];
    }
}
