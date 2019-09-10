using UnityEngine;
public class CamShake : MonoBehaviour
{
    public Animator camAnim;

    public void CamScreenShake()
    {
        camAnim.SetTrigger("Shake");
    }
    public void CamScreenBossShake()
    {
        camAnim.SetTrigger("BossShake");
    }
}
