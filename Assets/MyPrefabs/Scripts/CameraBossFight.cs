using Cinemachine;
using UnityEngine;

public class CameraBossFight : MonoBehaviour
{
    public Transform cameraFollowPos;
    public BossFightTrigger trigger;

    private void Start()
    {
        //trigger = GetComponent<BossFightTrigger>();
    }
    void Update()
    {
        //Debug.Log($"{trigger.bossFight}");
        var vcam = GetComponent<CinemachineVirtualCamera>();
        if (trigger.bossFight)
        {
            vcam.Follow = cameraFollowPos;

        }
    }
}
