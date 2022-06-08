using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonData : MonoBehaviour
{
    public static ButtonData instance;

    public Button startButton;
    public Button optionButton;
    public Button ExitButton;

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
