using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SO_float", menuName = "ScriptableObjects/SO_Float", order = 1)]
public class SO_Float : ScriptableObject
{
    [SerializeField]
    private float so_value;

    public event Action<float> OnValueChanged;

    // When value is modified trigger event to force update value where needed
    public void Set(float p_value)
    {
        if(so_value != p_value)
        {
            so_value = p_value;
            OnValueChanged?.Invoke(so_value);
        }
    }

    public float Get()
    {
        return so_value;
    }
}
