using UnityEngine;

public class CursorTraker : MonoBehaviour
{
    [SerializeField]
    private SO_Float so_Angle;

    [SerializeField]
    private bool X = false;

    [SerializeField]
    private bool Y = false;

    [SerializeField]
    private bool Z = false;

    private void OnEnable()
    {
        so_Angle.OnValueChanged += UpdateArrowDirection;
    }

    private void OnDisable()
    {
        so_Angle.OnValueChanged -= UpdateArrowDirection;
    }

    //Set rotation to So angle
    public void UpdateArrowDirection(float angle)
    {
        if(X)
        {
            transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }
        else if(Y)
        {
            transform.localRotation = Quaternion.Euler(0f, -angle, 0f); // ???????????
        }
        else if(Z)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }
    }
}
