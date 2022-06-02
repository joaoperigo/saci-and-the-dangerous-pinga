using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myTextElement;

    public void ButtonPress()
    {
        myTextElement.text = "This is my new text";
        //myTextElement.textStyle = TMP_Style.NormalStyle;
        //.fontStyle = FontStyles.Bold;
        //.color = Color.red;
        //myTextElement.fontSize = 200f;
    }
}
