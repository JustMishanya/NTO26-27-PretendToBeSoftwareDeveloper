using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class SliderIndicator : Indicator
{
    public float maxValue;
    private float defaultWidth;
    [SerializeField] RectTransform fillRect;
    void Awake()
    {
        defaultWidth = fillRect.sizeDelta.x;
        ToIndicate(0f);
    }
    public override void ToIndicate<T>(T value)
    {
        float delta = 0f;
        if(float.TryParse(value.ToString(), out float floatValue))
        {
            if (maxValue == 0) delta = 0f;
            else if (floatValue > maxValue)
            {
                delta = 1f;
            }
            else
            {
                delta = floatValue/maxValue;
            }
        }
        fillRect.offsetMax = new(delta*defaultWidth, 0f);
    }
}
