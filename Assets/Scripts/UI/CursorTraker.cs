using UnityEngine;

public class CursorTraker : MonoBehaviour
{
    [SerializeField]
    private SO_Float so_Angle;

    private void OnEnable()
    {
        so_Angle.OnValueChanged += UpdateArrowDirection;
    }

    private void OnDisable()
    {
        so_Angle.OnValueChanged -= UpdateArrowDirection;
    }

    //Set rotation to So angle
    void UpdateArrowDirection(float angle)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
