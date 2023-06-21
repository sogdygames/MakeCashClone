using System;
using UnityEngine;

public class MCCEventManager : MonoBehaviour
{   

    public static MCCEventManager current;

    public event Action ClickOnScreen;

    //Add Buttons
    public event Action SpeedButtonClick;
    public event Action AddPipeButtonClick;
    public event Action IncomeButtonClick;
    public event Action NewGPUButtonClick;

    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void ClickOnScreenTrigger()
    {
        ClickOnScreen?.Invoke();
    }

    public void SpeedButtonClickTrigger()
    {
        SpeedButtonClick?.Invoke();
    }

    public void AddPipeButtonClickTrigger()
    {
        AddPipeButtonClick?.Invoke();
    }

    public void IncomeButtonClickTrigger()
    {
        IncomeButtonClick?.Invoke();
    }

    public void NewGPUButtonClickTrigger()
    {
        NewGPUButtonClick?.Invoke();
    }
}
