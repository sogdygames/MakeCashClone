using UnityEngine;
using DG.Tweening;

public class MCCGameManager : MonoBehaviour
{

    [SerializeField] private Animator _bigPipeIncoinAnime;
    [SerializeField] private Animator[] _smallPipeInCoinAnimeList;
    [SerializeField] private Animator[] _moneyFloatAnimetorList;

    [Header("Generate Coin Related")]
    [SerializeField] private Transform[] _coinBirthPoints;
    [SerializeField] private GameObject[] _whiteCoinList;
    [SerializeField] private GameObject[] _orangeCoinList;

 
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

    private int _pipeCount = 4;

    private void Start()
    {
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
        {
            IndividualCoinGenerate(_coinBirthPoints[0]);
        }

        if (_pipeCount >= 2)
        {
            IndividualCoinGenerate(_coinBirthPoints[1]);
        }

        if (_pipeCount >= 3)
        {
            IndividualCoinGenerate(_coinBirthPoints[2]);
        }

        if (_pipeCount >=4)
        {
            IndividualCoinGenerate(_coinBirthPoints[3]);
        }
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



}



