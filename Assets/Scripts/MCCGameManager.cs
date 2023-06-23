using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class MCCGameManager : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] private Animator _bigPipeIncoinAnimator;
    [SerializeField] private Animator _burnPipeAnimator;
    [SerializeField] private Animator[] _smallPipeInCoinAnimatorList;
    [SerializeField] private Animator[] _moneyFloatAnimetorList;

    [Header("Generate Coin Related")]
    [SerializeField] private Transform[] _coinBirthPoints;
    [SerializeField] private GameObject[] _whiteCoinList;
    [SerializeField] private GameObject[] _orangeCoinList;

    [Header("Small Pipes")]
    [SerializeField] private GameObject[] _pipesList;

    [Header("Buttons")]
    [SerializeField] private Button _speedButton;
    [SerializeField] private Button _addPipeButton;
    [SerializeField] private Button _incomeButton;
    [SerializeField] private GameObject _mergePipePanelButton;
    [SerializeField] private GameObject _AddNewGpuPanelButton;

    [Header("FUNS")]
    [SerializeField] private GameObject _funLevel1;
    [SerializeField] private GameObject[] _funLeven2List;

    [Header("GPUs")]
    [SerializeField] private GameObject _gpu1;
    [SerializeField] private GameObject _gpu2;

    [Header("Effects")]
    [SerializeField] private GameObject _smallFireEfffect;
    [SerializeField] private GameObject _bigFireEffect;
    [SerializeField] private GameObject _newGpuEffect;

    private MCCUIManager _uiManager;
    private AudioSource _coinAudioSource;

    //Score and Target
    private float _score = 60.0f;
    private float _target = 1000.0f;

    //Fun Rotation Speed
    private float _funRotationSpeed = 1.0f;

    //Sliders
    //private float _targetAmount = 0f;
    //private float _newGpuAmount = 0f;

    private int _pipeCount = 1;
    private int _speedLevelCounter = 1;
    private int _incomeLevelCounter = 1;

    private List<float> _speedAndIncomePriceList = new List<float> {7.0f, 7.3f, 7.7f, 8.1f, 8.5f, 8.9f, 12.1f, 16.3f, 21.1f, 27.5f};
    private List<float> _pipePriceList = new List<float> { 50.0f, 100.0f, 150.0f, 200.0f };

    private int _gpuLevel = 1;

    private float _winAmount = 1.0f;

    bool _pipeMerged = false;

    private void Start()
    {
        _uiManager = GetComponent<MCCUIManager>();
        _coinAudioSource = GetComponent<AudioSource>();
        _uiManager.UpdateScore(_score, false);
        _newGpuEffect.SetActive(false);
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
        RotateFun();
        IncreaseHeatSlider();
        _uiManager.SetBurnSliderFillColor(_currentValue);
        ControlBurnEffects();
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
        if (!_gpuBurning) {
            _bigPipeIncoinAnimator.SetTrigger(AnimeConst.sendCoinToBigPipe);
            if (_currentValue < 0.5)
                _burnPipeAnimator.SetTrigger(AnimeConst.normalBurnAnimate);
            else
                _burnPipeAnimator.SetTrigger(AnimeConst.hardBurnAnimate);
            _tappedScreen = true;
        }
    }

    private void OnSpeedButtonClicked()
    {
        Debug.Log("OnSpeedButtonClicked");
        _score -= _speedAndIncomePriceList[_speedLevelCounter - 1];
        _speedLevelCounter++;
        _uiManager.UpdateScore(_score, false);
        _uiManager.UpdateSpeedButtonView(
            _speedLevelCounter,
            _speedAndIncomePriceList[_speedLevelCounter - 1],
            _speedLevelCounter + _incomeLevelCounter
            );
        UpgradeSpeed(_speedLevelCounter);
    }

    private void OnAddPipeButtonClicked()
    {
        Debug.Log("OnAddPipeButtonClicked");
        _score -= _pipePriceList[_pipeCount - 1];
        _pipeCount++;
        _uiManager.UpdateScore(_score, false);
        _uiManager.UpdateAddPipeButtonView(_pipePriceList[1]);
        AddPipe(_pipeCount - 2);

    }

    private void OnIncomeButtonClicked()
    {
        Debug.Log("OnIncomeButtonClicked");
        _score -= _speedAndIncomePriceList[_incomeLevelCounter - 1];
        _incomeLevelCounter++;
        _uiManager.UpdateScore(_score, false);
        _winAmount = (1.0f + ((float) _incomeLevelCounter * 3 / 10)) * _gpuLevel;
        _uiManager.UpdateIncomeButtonView(
            _incomeLevelCounter,
            _speedAndIncomePriceList[_incomeLevelCounter - 1],
            _speedLevelCounter + _incomeLevelCounter,
            _winAmount
            );
    }

    private void OnNewGPUButtonClicked()
    {
        Debug.Log("OnNewGPUButtonClicked");
        _gpuLevel++;
        _gpu1.SetActive(false);
        _gpu2.SetActive(true);
        _newGpuEffect.SetActive(true);
        DOVirtual.DelayedCall(1.0f, () => {
            _newGpuEffect.SetActive(false);
        });

        _AddNewGpuPanelButton.SetActive(false);
        _incomeLevelCounter = 1;
        _uiManager.SetNewGpuSlider((float)_incomeLevelCounter / 10.0f);



    }

    private void OnMergePipeButtonClicked()
    {
        Debug.Log("OnMergePipeButtonClicked");
        foreach(GameObject smallPipe in _pipesList) {
            smallPipe.SetActive(false);
        }
        _pipeMerged = true;
        _pipeCount = 1;
        UpgradePipe1();
    }

    private void OnStartSmallCoinAnimeTriggered()
    {
        for(int x = 1; x < _pipeCount; x++)
        {
            _smallPipeInCoinAnimatorList[x - 1].SetTrigger(AnimeConst.sendCoinToSmallPipe);
        }        
    }

    public void OnCoinInPipeFinishedTriggered()
    {
        Debug.Log("OnCoinInPipeFinishedTriggered");
        GenerateCoin();

        for (int x = 0; x < _pipeCount; x++)
        {
            _moneyFloatAnimetorList[x].SetTrigger(AnimeConst.moneyFloat);
        }

        _coinAudioSource.Play();

        //Update score
        _score += _pipeCount * _gpuLevel * _winAmount;
        _uiManager.UpdateScore(_score, true);

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
        if (_score >= _speedAndIncomePriceList[_speedLevelCounter - 1] && !_speedButton.interactable)
        {
            _speedButton.interactable = true;
        } else if (_score < _speedAndIncomePriceList[_speedLevelCounter - 1] && _speedButton.interactable)
        {
            _speedButton.interactable = false;
        }

        ///Add Pipe Button
        if (_score >= _pipePriceList[_pipeCount - 1] && !_addPipeButton.interactable && _pipeCount < 4)
        {
            _addPipeButton.interactable = true;
        }
        else if ((_score < _pipePriceList[_pipeCount - 1] || _pipeCount == 4) && _addPipeButton.interactable)
        {
            _addPipeButton.interactable = false;
        }

        ///Income Button
        if (_score >= _speedAndIncomePriceList[_incomeLevelCounter - 1] && !_incomeButton.interactable)
        {
            _incomeButton.interactable = true;
        }
        else if (_score < _speedAndIncomePriceList[_incomeLevelCounter - 1] && _incomeButton.interactable)
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
            if (_gpuLevel == 1)
                _AddNewGpuPanelButton.SetActive(true);
        }
        else if (_speedLevelCounter + _incomeLevelCounter < 10 && _AddNewGpuPanelButton.activeInHierarchy)
        {
            _AddNewGpuPanelButton.SetActive(false);
        }
    }


    int _funSpeedMultiplier = 50;

    private void RotateFun()
    {
        if (_currentValue <= 0.5) _funSpeedMultiplier = 50;
        if (_currentValue > 0.5) _funSpeedMultiplier = 150;
        if (_currentValue > 0.75) _funSpeedMultiplier = 300; 

        if (_gpuLevel == 1)
        {
            _funLevel1.transform.Rotate(0, 0, _funSpeedMultiplier * _funRotationSpeed * Time.deltaTime);
        }
        else if (_gpuLevel == 2)
        {
            foreach(GameObject gO in _funLeven2List)
            {
                gO.transform.Rotate(0, 0, _funSpeedMultiplier * _funRotationSpeed * Time.deltaTime);
            }
        }
    }

    private float _currentValue = 0;
    private float _targetValue = 0;
    private bool _targetReached = false;
    private bool _tappedScreen = false;
    private bool _gpuBurning = false;

    private void IncreaseHeatSlider() {
        if (_tappedScreen) {
            _targetReached = false;
            _tappedScreen = false;
            _targetValue = _currentValue + 0.1f;
            if (_targetValue > 1) {
                _targetValue = 1.0f;
                _gpuBurning = true;
            }
        }

        if (_currentValue <0) {
            _currentValue = 0;
            _gpuBurning = false;
        }

        if (_currentValue >= 0) {
            if (_targetReached) {
                _currentValue -= 0.3f * Time.deltaTime;
                _uiManager.SetBurnSlider(_currentValue);
            } else {
                _currentValue += 0.3f * Time.deltaTime;
                _uiManager.SetBurnSlider(_currentValue);
            }
        }

        if (_currentValue >= _targetValue) {
            _targetReached = true;
        }
    }

    private void ControlBurnEffects() {

        if (_currentValue <= 0.5) {

            if (_smallFireEfffect.activeInHierarchy)
                _smallFireEfffect.SetActive(false);

            if (_bigFireEffect.activeInHierarchy)
                _bigFireEffect.SetActive(false);

        } else if (_currentValue > 0.5 && !_smallFireEfffect.activeInHierarchy) {

            _smallFireEfffect.SetActive(true);
            if (_bigFireEffect.activeInHierarchy)
                _bigFireEffect.SetActive(false);

        } else if (_currentValue > 0.75 && !_bigFireEffect.activeInHierarchy) {

            _bigFireEffect.SetActive(true);
            if (_smallFireEfffect.activeInHierarchy)
                _smallFireEfffect.SetActive(false);
        }
    }

    private void UpgradeSpeed(int level) {
        _bigPipeIncoinAnimator.speed = 1.0f + ((float)level / 10.0f);
        
        foreach (Animator animator in _smallPipeInCoinAnimatorList) {
            animator.speed = 1.0f + ((float)level / 10.0f);
        }
    }

    private void AddPipe(int index) {
        _pipesList[index].SetActive(true);
    }

    private void UpgradePipe1() {
        _winAmount *= 4;
        _uiManager.UpgradeMultiplierOfPipe1();

    }

}



