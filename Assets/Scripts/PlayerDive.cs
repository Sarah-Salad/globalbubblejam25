using UnityEngine;

public class PlayerDive : MonoBehaviour
{
    public void OnDive()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gm.inMall)
        {
            gm.TransitionToSurface();
        }
        else
        {
            gm.TransitionToMall();
        }
    }
}
