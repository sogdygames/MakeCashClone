using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MCCUIManager : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _scoreTMPro;

    [SerializeField] private TextMeshProUGUI _speedLevel;
    [SerializeField] private TextMeshProUGUI _speedPrice;

    [SerializeField] private TextMeshProUGUI _addPipePrice;

    [SerializeField] private TextMeshProUGUI _incomeLevel;
    [SerializeField] private TextMeshProUGUI _incomePrice;

    [SerializeField] private TextMeshProUGUI _target;

    [Header("Buttons")]
    [SerializeField] private Button _speedButton;
    [SerializeField] private Button _addPipeButton;
    [SerializeField] private Button _incomeButton;

    [SerializeField] private GameObject _mergePipesPanel;
    [SerializeField] private GameObject _addNewGpuButtonPanel;

    [Header("SLiders")]
    [SerializeField] private Slider _targetSlider;
    [SerializeField] private Slider _newGpuSlider;
    [SerializeField] private Slider _burnSlider;

    [Header("Animators")]
    [SerializeField] private Animator _scoreAnimator;

    private void Start()
    {
        SetNewGpuSlider(0.2f);
    }

    public void UpdateScore(float value, bool animate)
    {
        _scoreTMPro.text = "$" + string.Format("{0:0.0}", value);
        if (animate)
        {
            _scoreAnimator.SetTrigger(AnimeConst.scoreAnimate);
        }

        SetTargetSlider(value / 1000);
    }

    public void UpdateSpeedButtonView(int level, float price, int totalLevel)
    {
        _speedLevel.text = "Level " + level.ToString();
        _speedPrice.text = "$" + string.Format("{0:0.0}", price);

        SetNewGpuSlider((float)totalLevel / 10.0f);
    }

    public void UpdateAddPipeButtonView(float price)
    {
        _addPipePrice.text = "$" + string.Format("{0:0.0}", price);
    }

    public void UpdateIncomeButtonView(int level, float price, int totalLevel)
    {
        _incomeLevel.text = "Level " + level.ToString();
        _incomePrice.text = "$" + string.Format("{0:0.0}", price);

        SetNewGpuSlider((float) totalLevel / 10.0f);
    }

    public void UpdateTarget(float value)
    {
        string target = "1.0K";
        if (value == 10000)
        {
            target = "10.0K";
        }
        _target.text = "Target: $" + target;
    }

    private void SetTargetSlider(float value)
    {
        if (value <=1)
        {
            _targetSlider.value = value;
        }
    }

    private void SetNewGpuSlider(float value)
    {
        Debug.Log("MCCUIManager, SetNewGpuSlider, value: " + value.ToString());
        if (value <= 1)
        {
            _newGpuSlider.value = value;
        }
    }

    public void SetBurnSlider(float value)
    {
        if (value <= 1)
        {
            _burnSlider.value = value;
        }
    }

}