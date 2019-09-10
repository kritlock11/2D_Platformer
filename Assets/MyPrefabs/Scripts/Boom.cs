using UnityEngine;

public class Boom : MonoBehaviour
{
    public Animator boomAnim;
    public void BoomAnim()
    {
        boomAnim.SetTrigger("Boom");
    }
}
