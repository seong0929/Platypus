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

    #region ����
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

        // ���� ����
        _bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        _soundEffectSlider.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        _masterVolumSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        // ��� ����
        _leftButton.onClick.AddListener(() => ChangeLanguage(-1));
        _rightButton.onClick.AddListener(() => ChangeLanguage(1));

        _confirmButton.onClick.AddListener(OnConfirmButtonClicked);

        UpdateLanguageText();
    }
    // ���� ũ�� ��������
    private void LoadSettings()
    {
        // ������ ���� �ε�
        float masterVolum = _soundManager.MasterVolume;
        _masterVolumSlider.value = masterVolum;

        // BGM ���� �ε�
        float bgmVolume = _soundManager.BgmVolume;
        _bgmSlider.value = bgmVolume;

        // ���� ����Ʈ ���� �ε�
        float soundEffectVolume = _soundManager.SoundEffectVolume;
        _soundEffectSlider.value = soundEffectVolume;
    }
    #endregion
    #region ����
    // ������ ���� ����
    private void OnMasterVolumeChanged(float value)
    {
        _soundManager.MasterVolume = value;
        _soundManager.SaveSettings();
    }
    // BGM ���� ����
    private void OnBgmVolumeChanged(float value)
    {
        _soundManager.BgmVolume = value;
        _soundManager.SaveSettings();
    }
    // ���� ����Ʈ ����
    private void OnSoundEffectVolumeChanged(float value)
    {
        _soundManager.SoundEffectVolume = value;
        _soundManager.SaveSettings();
    }
    #endregion
    // ToDo: ��� ���� �ٸ� Ŭ������ �ű��, �� ���常 ���⼭
    private void ChangeLanguage(int direction)
    {
        _currentLanguage = (Enums.ELanguage)(((int)_currentLanguage + direction + 2) % 2);    // ELanguage�� ����
        UpdateLanguageText();
    }
    private void OnConfirmButtonClicked()
    {
        string selectedLanguage = _currentLanguage.ToString();
        Debug.Log("Selected Language: " + selectedLanguage);

        // TODO: ��� �����ϴ� ������ �߰�
    }
    private void UpdateLanguageText()
    {
        _languageText.text = _currentLanguage.ToString();
    }
}