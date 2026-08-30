using UnityEngine;
using System;

// Перечисление всех поддерживаемых языков в нашем MVP путеводителе
public enum AppLanguage
{
    RU,
    EN,
    ZH
}

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    // Событие, на которое подписываются все текстовые поля для мгновенной смены языка на экране
    public static event Action OnLanguageChanged;

    private AppLanguage currentLanguage = AppLanguage.RU;

    public AppLanguage CurrentLanguage
    {
        get { return currentLanguage; }
    }

    [Header("Стартовая панель выбора языка (UI)")]
    public GameObject languageSelectPanel;

    private void Awake()
    {
        // Настройка Singleton-патерна, чтобы менеджер был доступен из любого скрипта проекта
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Метод переключения приложения на РУССКИЙ язык
    public void ChangeLanguageToRU()
    {
        currentLanguage = AppLanguage.RU;
        TriggerLanguageUpdate();
    }

    // Метод переключения приложения на АНГЛИЙСКИЙ язык
    public void ChangeLanguageToEN()
    {
        currentLanguage = AppLanguage.EN;
        TriggerLanguageUpdate();
    }

    // Метод переключения приложения на КИТАЙСКИЙ язык
    public void ChangeLanguageToZH()
    {
        currentLanguage = AppLanguage.ZH;
        TriggerLanguageUpdate();
    }

    // Внутренняя функция, которая обновляет тексты и переводит пользователя на карту
    private void TriggerLanguageUpdate()
    {
        // 1. Оповещаем все скрипты и поля, что язык изменился (перевод кнопок карты и текстов)
        if (OnLanguageChanged != null)
        {
            OnLanguageChanged.Invoke();
        }

        // 2. 🗺️ СИСТЕМНОЕ АВТОЗАКРЫТИЕ СТАРТОВОГО ОКНА И ОТКРЫТИЕ КАРТЫ
        var gpsManager = FindFirstObjectByType<GPSQuestManager>();
        if (gpsManager != null && gpsManager.mapPanel != null)
        {
            gpsManager.mapPanel.SetActive(true); // Включаем готовую карту острова Русский
        }

        // 3. Выключаем саму панель выбора языка, так как пользователь уже определился с выбором
        if (languageSelectPanel != null)
        {
            languageSelectPanel.SetActive(false);
        }
        else if (transform.gameObject.name == "LanguageSelectPanel")
        {
            transform.gameObject.SetActive(false);
        }
    }
}