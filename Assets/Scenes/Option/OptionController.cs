using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ELanguage
{
    Korean,
    English
}

public class OptionController : MonoBehaviour
{
    [SerializeField] Slider _MasterVolumSlider;
    [SerializeField] Slider _BgmSlider;
    [SerializeField] Slider _SoundEffectSlider;
    [SerializeField] GameObject _languageContainer;

    private TextMeshProUGUI _languageText;
    private Button _leftButton;
    private Button _rightButton;
    private Button _confirmButton;
    private SoundManager _soundManager;
    private ELanguage _currentLanguage = ELanguage.Korean;

    private void Awake()
    {
        _languageText = _languageContainer.GetComponentInChildren<TextMeshProUGUI>();
        Button[] buttons = _languageContainer.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            if (button.name.Equals("Back"))
            {
                _leftButton = button;
            }
            else if (button.name.Equals("Next"))
            {
                _rightButton = button;
            }
            else if (button.name.Equals("Confirm"))
            {
                _confirmButton = button;
            }
        }
    }
    private void Start()
    {
        _soundManager = SoundManager.instance;

        // 사운드 관련
        _MasterVolumSlider.value = _soundManager.MasterVolume;
        _BgmSlider.value = _soundManager.BgmVolume;
        _SoundEffectSlider.value = _soundManager.SoundEffectVolume;

        _BgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        _SoundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        _MasterVolumSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        // 언어 관련
        _leftButton.onClick.AddListener(() => ChangeLanguage(-1));
        _rightButton.onClick.AddListener(() => ChangeLanguage(1));

        _confirmButton.onClick.AddListener(OnConfirmButtonClicked);

        UpdateLanguageText();
    }
    private void OnMasterVolumeChanged(float value)
    {
        _soundManager.MasterVolume = value;
    }
    private void OnBgmVolumeChanged(float value)
    {
        _soundManager.BgmVolume = value;
    }
    private void OnSoundEffectVolumeChanged(float value)
    {
        _soundManager.SoundEffectVolume = value;
    }
    private void ChangeLanguage(int direction)
    {
        _currentLanguage = (ELanguage)(((int)_currentLanguage + direction + 2) % 2);    // ELanguage의 갯수
        UpdateLanguageText();
    }
    private void OnConfirmButtonClicked()
    {
        string selectedLanguage = _currentLanguage.ToString();
        Debug.Log("Selected Language: " + selectedLanguage);

        // TODO: 언어 변경하는 동작을 추가
    }
    private void UpdateLanguageText()
    {
        _languageText.text = _currentLanguage.ToString();
    }
}