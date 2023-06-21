using UnityEngine;

public class UserInpurControl : MonoBehaviour
{

    public void TapScreen()
    {
        MCCEventManager.current.ClickOnScreenTrigger();
    }

    public void ClickSpeedButton()
    {
        MCCEventManager.current.SpeedButtonClickTrigger();
    }

    public void ClickAddPipeButton()
    {
        MCCEventManager.current.AddPipeButtonClickTrigger();
    }

    public void ClickIncomeButton()
    {
        MCCEventManager.current.IncomeButtonClickTrigger();
    }

    public void ClickNewGPUButton()
    {
        MCCEventManager.current.NewGPUButtonClickTrigger();
    }

    public void MergePipesClickButton()
    {
        MCCEventManager.current.MergePipesClickTrigger();
    }
}
