using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SO_bool", menuName = "ScriptableObjects/SO_Bool", order = 1)]
public class SO_bool : ScriptableObject
{
    [SerializeField]
    private bool so_value;

    public event Action<bool> OnValueChanged;

    // When value is modified trigger event to force update value where needed
    public void Set(bool p_value)
    {
        if (so_value != p_value)
        {
            so_value = p_value;
            OnValueChanged?.Invoke(so_value);
        }
    }

    public bool Get()
    {
        return so_value;
    }
}
