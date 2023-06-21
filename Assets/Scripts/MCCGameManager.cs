using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCCGameManager : MonoBehaviour
{

    private void Start()
    {
        MCCEventManager.current.ClickOnScreen += OnScreenTapped;
        MCCEventManager.current.SpeedButtonClick += OnSpeedButtonClicked;
        MCCEventManager.current.AddPipeButtonClick += OnAddPipeButtonClicked;
        MCCEventManager.current.IncomeButtonClick += OnIncomeButtonClicked;
        MCCEventManager.current.NewGPUButtonClick += OnNewGPUButtonClicked;
    }

    private void OnDisable()
    {
        MCCEventManager.current.ClickOnScreen -= OnScreenTapped;
        MCCEventManager.current.SpeedButtonClick -= OnSpeedButtonClicked;
        MCCEventManager.current.AddPipeButtonClick -= OnAddPipeButtonClicked;
        MCCEventManager.current.IncomeButtonClick -= OnIncomeButtonClicked;
        MCCEventManager.current.NewGPUButtonClick -= OnNewGPUButtonClicked;
    }


    #region - User Input Handlings

    private void OnScreenTapped()
    {
        Debug.Log("OnScreenTapped");
    }

    private void OnSpeedButtonClicked()
    {
        Debug.Log("OnSpeedButtonClicked");
    }

    private void OnAddPipeButtonClicked()
    {
        Debug.Log("OnAddPipeButtonClicked");
    }

    private void OnIncomeButtonClicked()
    {
        Debug.Log("OnIncomeButtonClicked");
    }

    private void OnNewGPUButtonClicked()
    {
        Debug.Log("OnNewGPUButtonClicked");
    }


    #endregion

}



