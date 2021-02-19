using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public UIManager uiManagerSettings;

    public SoundManager soundManagerSettings;

    [Header("Количество крестьян")]
    public int countryMan;

    [Header("Количество войнов")]
    public int warMan;

    [Header("Запас пшеницы")]
    public int wheatStorage;

    [Header("Добыча пшеницы одним юнитом")]
    public int wheatAddQuantity;

    [Header("Стоимость покупки одного воина")]
    public int warManPrice;

    [Header("Скорость наема воина (сек.)")]
    public float hireSpeedWarMan;

    [Header("Стоимость содержание 1 воина")]
    public int containCostWarMan;

    [Header("Стоимость покупки одного крестьянина")]
    public int countryManPrice;

    [Header("Скорость наема крестьянина (сек.)")]
    public float hireSpeedCountryMan;

    [Header("Игр цикл до первой волны")]
    public int gameCycleBeforeEnemy;
    // пред колво игр циклов перед нападением
    private int gameCyclesEnemyPrevious;

    [Header("Колво врагов в 1 волне")]
    public int waveQuantEnemy;
    // пред колво врагов в волне
    private int waveQuantEnemyPrivious;

    [HideInInspector]
    public int survivedAttacks = 0;
    [HideInInspector]
    public int wheatMadeInCommon = 0;
    private void Start()
    {
        Time.timeScale = 1f;
        // присваиваем начальные значения
        gameCyclesEnemyPrevious = gameCycleBeforeEnemy;
        waveQuantEnemyPrivious = waveQuantEnemy;
    }

    private void Update()
    {
        if (gameCycleBeforeEnemy <= 0)
            EnemyAttack();
    }

    /// <summary>
    /// Увеличивает переменную на значение
    /// </summary>
    /// <param name="categoryToAdd">Переменная</param>
    /// <param name="quantityToAdd">Значение</param>
    public void ChangeQuantity(ref int categoryToAdd, int quantityToAdd)
    {
        categoryToAdd += quantityToAdd;
    }

    public void EnemyAttack()
    {
        warMan -= waveQuantEnemy;
        soundManagerSettings.PlaySound(soundManagerSettings.battle);
        if(warMan >= 0)
        {
            survivedAttacks++;

            uiManagerSettings.RenewInf(warMan, uiManagerSettings.warManText);

            waveQuantEnemyPrivious++;
            waveQuantEnemy = waveQuantEnemyPrivious;
            if (waveQuantEnemy / 5 == Convert.ToDouble(waveQuantEnemy) / 5 && gameCyclesEnemyPrevious > 0)
                gameCyclesEnemyPrevious--;
            gameCycleBeforeEnemy = gameCyclesEnemyPrevious;

            uiManagerSettings.SetSlider(gameCycleBeforeEnemy);
            uiManagerSettings.RenewInf(gameCycleBeforeEnemy, uiManagerSettings.enemyLeftCycleText);
            uiManagerSettings.RenewInf(waveQuantEnemy, uiManagerSettings.enemyQuantityEnemyText);
        }
        else
        {
            warMan = 0;
            countryMan = 0;
            wheatStorage = 0;

            uiManagerSettings.RenewInf(warMan, uiManagerSettings.warManText);
            uiManagerSettings.RenewInf(countryMan, uiManagerSettings.countryManText);
            uiManagerSettings.RenewInf(wheatStorage, uiManagerSettings.wheatStorageText);

            Time.timeScale = 0f;

            uiManagerSettings.FinishGame(false);
        }
    }

    /// <summary>
    /// Проверка условий победы
    /// </summary>
    /// <param name="wheatQuantity">Кол-во зерна</param>
    /// <param name="countyMan">Кол-во селян</param>
    public void WinCheck(int wheatQuantity, int countyMan)
    {
        if (wheatQuantity >= 1000 || countryMan >= 40)
        {
            Time.timeScale = 0f;
            uiManagerSettings.FinishGame(true);
        }
    }
}
