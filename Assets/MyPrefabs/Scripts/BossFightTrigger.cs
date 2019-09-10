using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    public Transform trigger;
    public LayerMask WhatIsPlayer;
    public bool bossFight;
    private GameObject boss;

    private void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
    }
    void Update()
    {
        RaycastHit2D PlayerInfo = Physics2D.Raycast(trigger.position, trigger.position - transform.right * 3, 2, WhatIsPlayer);
        Debug.DrawLine(trigger.position, trigger.position - transform.right * 3, Color.yellow);
        if (PlayerInfo.collider != null)
        {
            if (PlayerInfo.collider.CompareTag("Player"))
            {
                bossFight = true;
                boss.SetActive(true);
            }
        }
    }
}
