using UnityEngine;

public class MCCGameManager : MonoBehaviour
{

    //Score and Target
    private float _score = 20.0f;
    private float _target = 1000.0f;

    //Buttons
    private float _upgradeSpeedPrice = 7.0f;
    private float _upgradeAppPipePrice = 50.0f;
    private float _upgradeIncomePrice = 7.0f;

    //Sliders
    private float _heatAmount = 0f;
    private float _targetAmount = 0f;
    private float _newGpuAmount = 0f;


    private void Start()
    {
        #region Events Registration
        MCCEventManager.current.ClickOnScreen += OnScreenTapped;
        MCCEventManager.current.SpeedButtonClick += OnSpeedButtonClicked;
        MCCEventManager.current.AddPipeButtonClick += OnAddPipeButtonClicked;
        MCCEventManager.current.IncomeButtonClick += OnIncomeButtonClicked;
        MCCEventManager.current.NewGPUButtonClick += OnNewGPUButtonClicked;
        MCCEventManager.current.MergePipesClick += OnMergePipeButtonClicked;
        #endregion
    }

    private void OnDisable()
    {
        MCCEventManager.current.ClickOnScreen -= OnScreenTapped;
        MCCEventManager.current.SpeedButtonClick -= OnSpeedButtonClicked;
        MCCEventManager.current.AddPipeButtonClick -= OnAddPipeButtonClicked;
        MCCEventManager.current.IncomeButtonClick -= OnIncomeButtonClicked;
        MCCEventManager.current.NewGPUButtonClick -= OnNewGPUButtonClicked;
        MCCEventManager.current.MergePipesClick -= OnMergePipeButtonClicked;
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

    private void OnMergePipeButtonClicked()
    {
        Debug.Log("OnMergePipeButtonClicked");
    }
    #endregion



}



