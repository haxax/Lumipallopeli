using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldFormat : MonoBehaviour
{
    [SerializeField] private float minValue = 0;
    [SerializeField] private float maxValue = 0;
    [SerializeField] private float defaultValue = 0;
    [SerializeField] private bool roundNumbers = true;
    private InputField field;

    void Start()
    {
        field = GetComponent<InputField>();
        field.onValueChanged.AddListener(OnValueChanged);
        field.text = "" + defaultValue;
    }

    //Keeps the value of the InputField between min and max.
    public void OnValueChanged(string value)
    {
        float newValue = 0;
        if (float.TryParse(value, out newValue))
        {
            if (roundNumbers)
            { newValue = Mathf.RoundToInt(newValue); }
            newValue = Mathf.Clamp(newValue, minValue, maxValue);
            field.text = "" + newValue;
        }
        else
        {
            field.text = "" + defaultValue;
        }
    }
}
