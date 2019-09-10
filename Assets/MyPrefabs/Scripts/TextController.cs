using UnityEngine;
public class TextController : MonoBehaviour
{
    public Transform textRotationChange;
    private void Start()
    {
        textRotationChange = GetComponent<RectTransform>();
    }
    public void Update()
    {
        textRotationChange.rotation = Quaternion.identity;
    }
}
