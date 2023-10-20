using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionController : MonoBehaviour
{
    [SerializeField] Slider _masterVolumSlider;
    [SerializeField] Slider _bgmSlider;
    [SerializeField] Slider _soundEffectSlider;
    [SerializeField] GameObject _languageContainer;

    private TextMeshProUGUI _languageText;
    private Button _leftButton;
    private Button _rightButton;
    private Button _confirmButton;
    private SoundManager _soundManager;
    private Enums.ELanguage _currentLanguage = Enums.ELanguage.KR;

    #region 세팅
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

        LoadSettings();

        // 사운드 관련
        _bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        _soundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        _masterVolumSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        // 언어 관련
        _leftButton.onClick.AddListener(() => ChangeLanguage(-1));
        _rightButton.onClick.AddListener(() => ChangeLanguage(1));

        _confirmButton.onClick.AddListener(OnConfirmButtonClicked);

        UpdateLanguageText();
    }
    // 볼륨 크기 가져오기
    private void LoadSettings()
    {
        // 마스터 볼륨 로드
        float masterVolum = _soundManager.MasterVolume;
        _masterVolumSlider.value = masterVolum;

        // BGM 볼륨 로드
        float bgmVolume = _soundManager.BgmVolume;
        _bgmSlider.value = bgmVolume;

        // 사운드 이펙트 볼륨 로드
        float soundEffectVolume = _soundManager.SoundEffectVolume;
        _soundEffectSlider.value = soundEffectVolume;
    }
    #endregion
    #region 사운드
    // 마스터 볼륨 변경
    private void OnMasterVolumeChanged(float value)
    {
        _soundManager.MasterVolume = value;
        _soundManager.SaveSettings();
    }
    // BGM 볼륨 변경
    private void OnBgmVolumeChanged(float value)
    {
        _soundManager.BgmVolume = value;
        _soundManager.SaveSettings();
    }
    // 사운드 이팩트 변경
    private void OnSoundEffectVolumeChanged(float value)
    {
        _soundManager.SoundEffectVolume = value;
        _soundManager.SaveSettings();
    }
    #endregion
    // ToDo: 언어 번역 다른 클래스로 옮기기, 값 저장만 여기서
    private void ChangeLanguage(int direction)
    {
        _currentLanguage = (Enums.ELanguage)(((int)_currentLanguage + direction + 2) % 2);    // ELanguage의 갯수
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