using UnityEngine;

public class MenuText : MonoBehaviour  //ремуваем ченджЛог
{
    public Transform textRotationChange;

    private void Start()
    {
        textRotationChange = GetComponent<Transform>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            textRotationChange.gameObject.SetActive(false);
        }// Remove Text

    }

}
