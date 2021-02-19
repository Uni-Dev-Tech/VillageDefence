using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public UIManager uiManagerSettings;

    public GameManager gameManagerSettings;

    public SoundManager soundManagerSettings;

    [Header("Время игрового цикла")]
    [SerializeField]
    private float maxTimeGameCycle;

    // оставшееся время цикла
    private float currentTimeGameCycle;

    [HideInInspector]
    public bool warManIsHiring, countryManIsHiring;

    [HideInInspector]
    public int warManHired, countryManHired;

    private void Start()
    {
        warManIsHiring = false;
        countryManIsHiring = false;

        warManHired = 0;
        countryManHired = 0;

        //инициализируем счетчик
        currentTimeGameCycle = maxTimeGameCycle;
    }
    private void Update()
    {
        GameCycleTimer();
    }

    /// <summary>
    /// Игровой цикл (день)
    /// </summary>
    private void GameCycleTimer()
    {
        // вычитаем время прошедшее за один кадр 
        currentTimeGameCycle -= Time.deltaTime;
        
        // в случае обнуления счетчика
        if(currentTimeGameCycle <= 0)
        {
            // обновляем счетчик
            currentTimeGameCycle = maxTimeGameCycle;

            // добавляем количество произведенной пшеницы
            gameManagerSettings.ChangeQuantity(ref gameManagerSettings.wheatStorage, gameManagerSettings.wheatAddQuantity * gameManagerSettings.countryMan);
            gameManagerSettings.wheatMadeInCommon += gameManagerSettings.wheatAddQuantity * gameManagerSettings.countryMan;

            // вычитаем количество потребляемой пшеницы
            gameManagerSettings.ChangeQuantity(ref gameManagerSettings.wheatStorage, -(gameManagerSettings.containCostWarMan * gameManagerSettings.warMan));

            // Обновляем данные о пшенице
            uiManagerSettings.RenewInf(gameManagerSettings.wheatStorage, uiManagerSettings.wheatStorageText);

            // Обновляем данные о волне врагов
            uiManagerSettings.RenewInf(1, uiManagerSettings.enemyWaveSlider);
            gameManagerSettings.gameCycleBeforeEnemy--;
            uiManagerSettings.RenewInf(gameManagerSettings.gameCycleBeforeEnemy, uiManagerSettings.enemyLeftCycleText);

            soundManagerSettings.PlaySound(soundManagerSettings.newDay);
            gameManagerSettings.WinCheck(gameManagerSettings.wheatStorage, gameManagerSettings.countryMan);
        }

        // обновляем данные счетчика
        uiManagerSettings.RenewInf(currentTimeGameCycle / maxTimeGameCycle, uiManagerSettings.gameCycleImage);
    }

    /// <summary>
    /// Таймер наема воина
    /// </summary>
    /// <param name="maxTime">Время необходимое для наема</param>
    /// <param name="timer">Кнопка наема</param>
    /// <returns></returns>
    public IEnumerator SetTimerWarMan(float maxTime, Button timer)
    {
        // Вычитаем стоимость воина
        gameManagerSettings.ChangeQuantity(ref gameManagerSettings.wheatStorage, -gameManagerSettings.warManPrice);

        warManIsHiring = true;

        // Обновляем инф количества пшеницы
        uiManagerSettings.RenewInf(gameManagerSettings.wheatStorage, uiManagerSettings.wheatStorageText);

        // инициализируем счетчик
        float currentTime = maxTime;

        //отключаем кнопку
        timer.interactable = false;

        // запускаем счетчик
        do
        {
            // обновление каждый Update
            yield return new WaitForSeconds(Time.deltaTime);

            // вычитаем время прошедшее за один кадр
            currentTime -= Time.deltaTime;

            // в случае обнуления счетчика
            if (currentTime <= 0)
            {
                // добавляем нанятого воина
                gameManagerSettings.ChangeQuantity(ref gameManagerSettings.warMan, 1);

                // обновляем инф отображающую количество воинов
                uiManagerSettings.RenewInf(gameManagerSettings.warMan, uiManagerSettings.warManText);

                // Восстанавливаем заполненность кнопки
                currentTime = maxTime;
                uiManagerSettings.RenewInf(currentTime / maxTime, timer.image);

                // Включаем кнопку и выходим из корутины
                timer.interactable = true;
                warManIsHiring = false;
                soundManagerSettings.PlaySound(soundManagerSettings.hiredWarMan);
                warManHired++;
                break;
            }

            // обновляем инф отображающую остаток времени
            uiManagerSettings.RenewInf(currentTime / maxTime, timer.image);
        } while (true);
    }

    /// <summary>
    /// Таймер наема крестьянина
    /// </summary>
    /// <param name="maxTime">Время необходимое для наема</param>
    /// <param name="timer">Кнопка наема</param>
    /// <returns></returns>
    public IEnumerator SetTimerCountryMan(float maxTime, Button timer)
    {
        // вычитаем стоимость крестьянина
        gameManagerSettings.ChangeQuantity(ref gameManagerSettings.wheatStorage, -gameManagerSettings.countryManPrice);

        countryManIsHiring = true;

        // Обновляем инф количества пшеницы
        uiManagerSettings.RenewInf(gameManagerSettings.wheatStorage, uiManagerSettings.wheatStorageText);

        //инициализируем счетчик
        float currentTime = maxTime;

        // отключаем кнопку
        timer.interactable = false;

        // запускаем счетчик
        do
        {
            // обновляем каждый Update
            yield return new WaitForSeconds(Time.deltaTime);

            // вычитаем время прошедшее за один кадр
            currentTime -= Time.deltaTime;

            //в случае обнуления счетчика
            if (currentTime <= 0)
            {
                // добавляем нанятого крестьянина
                gameManagerSettings.ChangeQuantity(ref gameManagerSettings.countryMan, 1);

                //обновляем инф отображающую количество крестьян
                uiManagerSettings.RenewInf(gameManagerSettings.countryMan, uiManagerSettings.countryManText);

                // восстанавливаем заполненность кнопки
                currentTime = maxTime;
                uiManagerSettings.RenewInf(currentTime / maxTime, timer.image);

                // включаем кнопку и выходим из корутины
                timer.interactable = true;
                countryManIsHiring = false;
                soundManagerSettings.PlaySound(soundManagerSettings.hiredCountryMan);
                countryManHired++;
                break;
            }

            // обновляем инф отображающую остаток времени
            uiManagerSettings.RenewInf(currentTime / maxTime, timer.image);
        } while (true);
    }
}
