using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorDatabase : MonoBehaviour
{
    public static AnimatorDatabase instance;

    //public AnimatorControllerParameter[] charactorAnim;

    public RuntimeAnimatorController[] charactorAnim;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
