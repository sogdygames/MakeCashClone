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

    [Header("FUNS")]
    [SerializeField] private GameObject _funLevel1;
    [SerializeField] private GameObject[] _funLeven2List;

    private MCCUIManager _uiManager;
    private AudioSource _coinAudioSource;

    //Score and Target
    private float _score = 20.0f;
    private float _target = 1000.0f;

    //Fun Rotation Speed
    private float _funRotationSpeed = 1.0f;

    //Sliders
    private float _heatAmount = 0f;
    private float _targetAmount = 0f;
    private float _newGpuAmount = 0f;

    private int _pipeCount = 1;
    private int _speedLevelCounter = 1;
    private int _incomeLevelCounter = 1;

    private List<float> _speedAndIncomePriceList = new List<float> {7.0f, 7.3f, 7.7f, 8.1f, 8.5f, 8.9f, 12.1f, 16.3f, 21.1f, 27.5f};
    private List<float> _pipePriceList = new List<float> { 50.0f, 100.0f, 150.0f, 200.0f };

    private int _gpuLevel = 2;

    private void Start()
    {
        _uiManager = GetComponent<MCCUIManager>();
        _coinAudioSource = GetComponent<AudioSource>();
        _uiManager.UpdateScore(_score, false);
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
        _score -= _speedAndIncomePriceList[_speedLevelCounter - 1];
        _speedLevelCounter++;
        _uiManager.UpdateScore(_score, false);
        _uiManager.UpdateSpeedButtonView(
            _speedLevelCounter,
            _speedAndIncomePriceList[_speedLevelCounter - 1],
            _speedLevelCounter + _incomeLevelCounter
            );
        //TODO: DO Something with Speed!
    }

    private void OnAddPipeButtonClicked()
    {
        Debug.Log("OnAddPipeButtonClicked");
        _score -= _pipePriceList[_pipeCount - 1];
        _pipeCount++;
        _uiManager.UpdateScore(_score, false);
        _uiManager.UpdateAddPipeButtonView(_pipePriceList[1]);
        //TODO: DO Something with Pipe!

    }

    private void OnIncomeButtonClicked()
    {
        Debug.Log("OnIncomeButtonClicked");
        _score -= _speedAndIncomePriceList[_incomeLevelCounter - 1];
        _incomeLevelCounter++;
        _uiManager.UpdateScore(_score, false);
        _uiManager.UpdateIncomeButtonView(
            _incomeLevelCounter,
            _speedAndIncomePriceList[_incomeLevelCounter - 1],
            _speedLevelCounter + _incomeLevelCounter
            );
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
        for(int x = 1; x < _pipeCount; x++)
        {
            _smallPipeInCoinAnimeList[x - 1].SetTrigger(AnimeConst.sendCoinToSmallPipe);
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
        _score += _pipeCount;
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
            _AddNewGpuPanelButton.SetActive(true);
        }
        else if (_speedLevelCounter + _incomeLevelCounter < 10 && _AddNewGpuPanelButton.activeInHierarchy)
        {
            _AddNewGpuPanelButton.SetActive(false);
        }
    }

    private void RotateFun()
    {
        if (_gpuLevel == 1)
        {
            _funLevel1.transform.Rotate(0, 0, 50 * _funRotationSpeed * Time.deltaTime);
        }
        else if (_gpuLevel == 2)
        {
            foreach(GameObject gO in _funLeven2List)
            {
                gO.transform.Rotate(0, 0, 50 * _funRotationSpeed * Time.deltaTime);
            }
        }
    }

}



