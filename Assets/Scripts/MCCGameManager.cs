using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class MCCGameManager : MonoBehaviour
{

    [SerializeField] private Animator _bigPipeIncoinAnime;
    [SerializeField] private Animator[] _smallPipeInCoinAnimeList;
    [SerializeField] private Animator[] _moneyFloatAnimetorList;

    [Header("Generate Coin Related")]
    [SerializeField] private Transform[] _coinBirthPoints;
    [SerializeField] private GameObject[] _whiteCoinList;
    [SerializeField] private GameObject[] _orangeCoinList;

    [Header("Buttons")]
    [SerializeField] private Button _speedButton;
    [SerializeField] private Button _addPipeButton;
    [SerializeField] private Button _incomeButton;
    [SerializeField] private GameObject _mergePipePanelButton;
    [SerializeField] private GameObject _AddNewGpuPanelButton;

    private MCCUIManager _uiManager;

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

    private int _pipeCount = 1;
    private int _speedLevelCounter = 1;
    private int _incomeLevelCounter = 1;

    private List<float> _speedAndIncomePriceList = new List<float> {7.0f, 7.3f, 7.7f, 8.1f, 8.5f, 8.9f, 12.1f, 16.3f, 21.1f, 27.5f};
    private List<float> _pipePriceList = new List<float> { 50.0f, 100.0f };

    private void Start()
    {
        _uiManager = GetComponent<MCCUIManager>();
        #region Events Registration
        MCCEventManager.current.ClickOnScreen += OnScreenTapped;
        MCCEventManager.current.SpeedButtonClick += OnSpeedButtonClicked;
        MCCEventManager.current.AddPipeButtonClick += OnAddPipeButtonClicked;
        MCCEventManager.current.IncomeButtonClick += OnIncomeButtonClicked;
        MCCEventManager.current.NewGPUButtonClick += OnNewGPUButtonClicked;
        MCCEventManager.current.MergePipesClick += OnMergePipeButtonClicked;
        MCCEventManager.current.StartSmallCoinAnime += OnStartSmallCoinAnimeTriggered;
        MCCEventManager.current.CoinInPipeFinished += OnCoinInPipeFinishedTriggered;
        #endregion
    }

    private void Update()
    {
        UpdateButtonActivePassive();
    }

    private void OnDisable()
    {
        MCCEventManager.current.ClickOnScreen -= OnScreenTapped;
        MCCEventManager.current.SpeedButtonClick -= OnSpeedButtonClicked;
        MCCEventManager.current.AddPipeButtonClick -= OnAddPipeButtonClicked;
        MCCEventManager.current.IncomeButtonClick -= OnIncomeButtonClicked;
        MCCEventManager.current.NewGPUButtonClick -= OnNewGPUButtonClicked;
        MCCEventManager.current.MergePipesClick -= OnMergePipeButtonClicked;
        MCCEventManager.current.StartSmallCoinAnime -= OnStartSmallCoinAnimeTriggered;
    }


    #region - User Input Handlings
    private void OnScreenTapped()
    {
        Debug.Log("OnScreenTapped");
        _bigPipeIncoinAnime.SetTrigger(AnimeConst.sendCoinToBigPipe);
        
    }

    private void OnSpeedButtonClicked()
    {
        Debug.Log("OnSpeedButtonClicked");
        _score -= _upgradeSpeedPrice;
        _speedLevelCounter++;
        _uiManager.UpdateScore(_score);
        _uiManager.UpdateSpeedButtonView(_speedLevelCounter, _speedAndIncomePriceList[_speedLevelCounter - 1]);
        //TODO: DO Something with Speed!
    }

    private void OnAddPipeButtonClicked()
    {
        Debug.Log("OnAddPipeButtonClicked");
        _score -= _upgradeAppPipePrice;
        _pipeCount++;
        _uiManager.UpdateScore(_score);
        _uiManager.UpdateAddPipeButtonView(_pipePriceList[1]);
        //TODO: DO Something with Pipe!

    }

    private void OnIncomeButtonClicked()
    {
        Debug.Log("OnIncomeButtonClicked");
        _score -= _upgradeIncomePrice;
        _incomeLevelCounter++;
        _uiManager.UpdateScore(_score);
        _uiManager.UpdateIncomeButtonView(_incomeLevelCounter, _speedAndIncomePriceList[_speedLevelCounter - 1]);
        //TODO: DO SOmething with Income!
    }

    private void OnNewGPUButtonClicked()
    {
        Debug.Log("OnNewGPUButtonClicked");
    }

    private void OnMergePipeButtonClicked()
    {
        Debug.Log("OnMergePipeButtonClicked");
    }

    private void OnStartSmallCoinAnimeTriggered()
    {
        foreach(Animator animator in _smallPipeInCoinAnimeList)
        {
            animator.SetTrigger(AnimeConst.sendCoinToSmallPipe);
        }
        
    }

    public void OnCoinInPipeFinishedTriggered()
    {
        Debug.Log("OnCoinInPipeFinishedTriggered");
        GenerateCoin();

        foreach (Animator animator in _moneyFloatAnimetorList)
        {
            animator.SetTrigger(AnimeConst.moneyFloat);
        }

    }
    #endregion

    private void GenerateCoin()
    {
        if (_pipeCount >= 1)
           IndividualCoinGenerate(_coinBirthPoints[0]);

        if (_pipeCount >= 2)
           IndividualCoinGenerate(_coinBirthPoints[1]);

        if (_pipeCount >= 3)
           IndividualCoinGenerate(_coinBirthPoints[2]);

        if (_pipeCount >=4)
           IndividualCoinGenerate(_coinBirthPoints[3]);
    }

    private void IndividualCoinGenerate(Transform newTransform)
    {
        int value = Random.Range(1, 3);
        foreach (GameObject gO in (value == 1 ? _whiteCoinList : _orangeCoinList))
        {
            if (!gO.activeInHierarchy)
            {
                gO.GetComponent<Transform>().transform.position = newTransform.position;
                gO.SetActive(true);
                DOVirtual.DelayedCall(20, () => {
                    gO.SetActive(false);
                });
                break;
            }
        } 
    }

    private void UpdateButtonActivePassive()
    {
        ///Speed Button
        if (_score >= _upgradeSpeedPrice && !_speedButton.enabled)
        {
            _speedButton.interactable = true;
        } else if (_score < _upgradeSpeedPrice && _speedButton.enabled)
        {
            _speedButton.interactable = false;
        }

        ///Add Pipe Button
        if (_score >= _upgradeAppPipePrice && !_addPipeButton.enabled && _pipeCount < 4)
        {
            _addPipeButton.interactable = true;
        }
        else if ((_score < _upgradeAppPipePrice || _pipeCount == 4) && _addPipeButton.enabled)
        {
            _addPipeButton.interactable = false;
        }

        ///Income Button
        if (_score >= _upgradeIncomePrice && !_incomeButton.enabled)
        {
            _incomeButton.interactable = true;
        }
        else if (_score < _upgradeIncomePrice && _incomeButton.enabled)
        {
            _incomeButton.interactable = false;
        }

        ///Merge Pipe Button
        if (_pipeCount == 4 && !_mergePipePanelButton.activeInHierarchy)
        {
            _mergePipePanelButton.SetActive(true);
        }
        else if (_pipeCount < 4 && _mergePipePanelButton.activeInHierarchy)
        {
            _mergePipePanelButton.SetActive(false);
        }

        /// Add New GPU Panel
        if (_speedLevelCounter + _incomeLevelCounter == 10 && !_AddNewGpuPanelButton.activeInHierarchy) 
        {
            _AddNewGpuPanelButton.SetActive(true);
        }
        else if (_speedLevelCounter + _incomeLevelCounter < 10 && _AddNewGpuPanelButton.activeInHierarchy)
        {
            _AddNewGpuPanelButton.SetActive(false);
        }
    }

}



