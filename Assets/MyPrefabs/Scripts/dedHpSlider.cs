using UnityEngine;

public class dedHpSlider : MonoBehaviour
{
    public Transform textRotationChange;
    public void Update()
    {
        textRotationChange.rotation = Quaternion.identity;
    }

}
