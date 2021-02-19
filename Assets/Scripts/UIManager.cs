using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameManager gameManagerSettings;

    public TimerManager timerManagerSettings;

    public SoundManager soundManagerSettings;

    [Header("Отображение крестьян")]
    public Text countryManText;

    [Header("Отображение воинов")]
    public Text warManText;

    [Header("Отображение запасов пшеницы")]
    public Text wheatStorageText;

    [Header("Таймер игрового цикла")]
    public Image gameCycleImage;

    [Header("Таймер (кнопка) доб воина")]
    public Button warManButton;

    [Header("Таймер (кнопка) доб крестьянина")]
    public Button countryManButton;

    [Header("Слайдер волны врагов")]
    public Slider enemyWaveSlider;

    [Header("Остаток игр цикл до волны вр")]
    public Text enemyLeftCycleText;

    [Header("Количество врагов в волне")]
    public Text enemyQuantityEnemyText;

    [Header("Панель победа/поражение/паузы")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject pausePanel;

    [Header("Статистика поражения")]
    public Text raidsSurvivedLose;
    public Text producedWheatLose,
                producedWarManLose,
                producedCountryManLose;

    [Header("Статистика победы")]
    public Text raidsSurvivedWin;
    public Text producedWheatWin,
                producedWarManWin,
                producedCountryManWin;

    [Header("Статистика паузы")]
    public Text raidsSurvivedPause;
    public Text producedWheatPause,
                producedWarManPause,
                producedCountryManPause;

    [Header("Звук")]
    public Button soundOnOff;

    private void Start()
    {
        // Отображаем стартовый набор ресурсов
        RenewInf(gameManagerSettings.countryMan, countryManText);
        RenewInf(gameManagerSettings.warMan, warManText);
        RenewInf(gameManagerSettings.wheatStorage, wheatStorageText);
        RenewInf(gameManagerSettings.gameCycleBeforeEnemy, enemyLeftCycleText);
        RenewInf(gameManagerSettings.waveQuantEnemy, enemyQuantityEnemyText);
        SetSlider(gameManagerSettings.gameCycleBeforeEnemy);
    }

    private void Update()
    {
        if (warManButton.interactable)
            if (gameManagerSettings.warManPrice > gameManagerSettings.wheatStorage)
                warManButton.interactable = false;
        if (!warManButton.interactable)
            if (gameManagerSettings.warManPrice <= gameManagerSettings.wheatStorage)
                if (!timerManagerSettings.warManIsHiring)
                    warManButton.interactable = true;

        if (countryManButton.interactable)
            if (gameManagerSettings.countryManPrice > gameManagerSettings.wheatStorage)
                countryManButton.interactable = false;
        if (!countryManButton.interactable)
            if (gameManagerSettings.countryManPrice <= gameManagerSettings.wheatStorage)
                if (!timerManagerSettings.countryManIsHiring)
                    countryManButton.interactable = true;
    }

    /// <summary>
    /// Обновляет данные в UI
    /// </summary>
    /// <param name="newValue">Новая величина</param>
    /// <param name="renewText">Обновляемый объект</param>
    public void RenewInf(int newValue, Text renewText)
    {
        renewText.text = newValue.ToString();
    }

    /// <summary>
    /// Обновляет данные в UI
    /// </summary>
    /// <param name="newValue">Новая величина</param>
    /// <param name="renewImage">Обновляемый объект</param>
    public void RenewInf(float newValue, Image renewImage)
    {
        renewImage.fillAmount = newValue;
    }

    /// <summary>
    /// Настраивает слайдер к новой волне
    /// </summary>
    /// <param name="enemyQuantity">Колво циклов до след волне</param>
    public void SetSlider(int cycleQuantity)
    {
        enemyWaveSlider.maxValue = cycleQuantity;
        enemyWaveSlider.value = 0;
    }

    /// <summary>
    /// Обновляет данные в UI
    /// </summary>
    /// <param name="newValue">Добавляемая величина</param>
    /// <param name="renewSlider">Обновляемый объект</param>
    public void RenewInf(int addValue, Slider renewSlider)
    {
        renewSlider.value += addValue;
    }

    /// <summary>
    /// Кнопка наема воина
    /// </summary>
    public void WarManHire()
    {
        StartCoroutine(timerManagerSettings.SetTimerWarMan(gameManagerSettings.hireSpeedWarMan, warManButton));
    }

    /// <summary>
    /// Кнопка наема крестьянина
    /// </summary>
    public void CountryManHire()
    {
        StartCoroutine(timerManagerSettings.SetTimerCountryMan(gameManagerSettings.hireSpeedCountryMan, countryManButton));
    }

    /// <summary>
    /// Появляется панель победы/поражения
    /// </summary>
    public void FinishGame(bool win)
    {
        if (win)
        {
            winPanel.SetActive(true);

            soundManagerSettings.PlaySound(soundManagerSettings.win);

            RenewInf(gameManagerSettings.survivedAttacks, raidsSurvivedWin);
            RenewInf(gameManagerSettings.wheatMadeInCommon, producedWheatWin);
            RenewInf(timerManagerSettings.warManHired, producedWarManWin);
            RenewInf(timerManagerSettings.countryManHired, producedCountryManWin);
        }
        else
        {
            losePanel.SetActive(true);

            soundManagerSettings.PlaySound(soundManagerSettings.lose);

            RenewInf(gameManagerSettings.survivedAttacks, raidsSurvivedLose);
            RenewInf(gameManagerSettings.wheatMadeInCommon, producedWheatLose);
            RenewInf(timerManagerSettings.warManHired, producedWarManLose);
            RenewInf(timerManagerSettings.countryManHired, producedCountryManLose);
        }
    }

    /// <summary>
    /// Кнопка рестарта
    /// </summary>
    public void TryAgainButton()
    {
        soundManagerSettings.PlaySound(soundManagerSettings.click);
        StartCoroutine(DelayBeforeReloadLevel());
    }

    /// <summary>
    /// Кнопка паузы
    /// </summary>
    public void PauseButton()
    {
        Time.timeScale = 0f;

        soundManagerSettings.PlaySound(soundManagerSettings.click);

        pausePanel.SetActive(true);

        RenewInf(gameManagerSettings.survivedAttacks, raidsSurvivedPause);
        RenewInf(gameManagerSettings.wheatMadeInCommon, producedWheatPause);
        RenewInf(timerManagerSettings.warManHired, producedWarManPause);
        RenewInf(timerManagerSettings.countryManHired, producedCountryManPause);
    }

    /// <summary>
    /// Кнопка продолжить(меню паузы)
    /// </summary>
    public void ContinueButton()
    {
        Time.timeScale = 1f;

        soundManagerSettings.PlaySound(soundManagerSettings.click);

        pausePanel.SetActive(false);
    }

    /// <summary>
    /// Кнопка вкл/выкл звука
    /// </summary>
    public void SoundOnOff()
    {
        soundManagerSettings.audioSource.mute = !soundManagerSettings.audioSource.mute;

        if (soundManagerSettings.musicSource.isPlaying)
            soundManagerSettings.musicSource.Pause();
        else
            soundManagerSettings.musicSource.Play();

        if (soundManagerSettings.audioSource.mute)
            soundOnOff.image.color = new Color(1, 1, 1, 0.5f);

        if (!soundManagerSettings.audioSource.mute)
            soundOnOff.image.color = Color.white;
    }

    private IEnumerator DelayBeforeReloadLevel()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
