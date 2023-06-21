using UnityEngine;

public class CoinBigPipeController : MonoBehaviour
{

    public void TriggerSmallCoinStart()
    {
        MCCEventManager.current.StartSmallCoinAnimeTrigger();
    }

    public void TriggerCoinInPipeFinished()
    {
        MCCEventManager.current.CoinInPipeFinishedTrigger();
    }
}
