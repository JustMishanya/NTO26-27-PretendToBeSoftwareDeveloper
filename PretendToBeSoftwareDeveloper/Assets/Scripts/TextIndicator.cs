using System;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class TextIndicator : Indicator
{
    private TMP_Text tmp_text;
    public String prefix, postfix;
    void Awake(){
        tmp_text = GetComponent<TMP_Text>();
        ToIndicate(0);
    }
    public override void ToIndicate(float value)
    {
        tmp_text.text = prefix+value.ToString()+postfix;
    }
}