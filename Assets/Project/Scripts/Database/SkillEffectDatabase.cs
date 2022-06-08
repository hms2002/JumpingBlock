using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectDatabase : MonoBehaviour
{
    public static SkillEffectDatabase instance;

    public GameObject boySkillEffect;
    public GameObject girlSkillEffect;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
