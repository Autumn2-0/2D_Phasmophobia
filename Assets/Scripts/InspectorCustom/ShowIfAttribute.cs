using UnityEngine;

public class ShowIfAttribute : PropertyAttribute
{
    public string ConditionalSourceField;

    public ShowIfAttribute(string conditionalSourceField)
    {
        ConditionalSourceField = conditionalSourceField;
    }
}
