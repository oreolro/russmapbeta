using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class QuestQuestion
{
    public int questionId;
    public string questionRU;
    public string questionEN;
    public string questionZH;

    public string[] optionsRU = new string[4];
    public string[] optionsEN = new string[4];
    public string[] optionsZH = new string[4];

    public int correctAnswerIndex;
}

public class QuestSystemManager : MonoBehaviour
{
    public static QuestSystemManager Instance;

    [Header("UI Интерфейса квеста (TextMeshPro)")]
    public GameObject questPanel;
    public GameObject mainPanel; // Ссылка на главный экран гида (История)
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI resultText;
    public Button nextButton;

    private List<QuestQuestion> currentQuestQuestions = new List<QuestQuestion>();
    private int currentQuestionIndex = 0;
    private int score = 0;
    private QuestQuestion activeQuestion;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void LaunchQuest(int locationId)
    {
        currentQuestQuestions = QuestDatabase.GetQuestionsForLocation(locationId);
        if (currentQuestQuestions == null || currentQuestQuestions.Count == 0) return;

        score = 0;
        currentQuestionIndex = 0;

        questPanel.SetActive(true);
        if (mainPanel != null) mainPanel.SetActive(false); // Полностью выключаем фон путеводителя

        resultText.text = "";
        nextButton.gameObject.SetActive(false);

        // Сбрасываем кнопку Далее на стандартный метод переключения вопросов
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextQuestionPressed);

        foreach (var btn in answerButtons) btn.gameObject.SetActive(true);

        DisplayQuestion();
    }

    private void DisplayQuestion()
    {
        if (currentQuestionIndex >= currentQuestQuestions.Count)
        {
            EndQuest();
            return;
        }

        activeQuestion = currentQuestQuestions[currentQuestionIndex];
        resultText.text = "";
        nextButton.gameObject.SetActive(false);

        foreach (var btn in answerButtons) btn.interactable = true;

        AppLanguage currentLang = LanguageManager.Instance.CurrentLanguage;

        switch (currentLang)
        {
            case AppLanguage.RU:
                questionText.text = activeQuestion.questionRU;
                for (int i = 0; i < 4; i++) answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = activeQuestion.optionsRU[i];
                break;
            case AppLanguage.EN:
                questionText.text = activeQuestion.questionEN;
                for (int i = 0; i < 4; i++) answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = activeQuestion.optionsEN[i];
                break;
            case AppLanguage.ZH:
                questionText.text = activeQuestion.questionZH;
                for (int i = 0; i < 4; i++) answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = activeQuestion.optionsZH[i];
                break;
        }
    }

    public void OnAnswerSelected(int selectedIndex)
    {
        foreach (var btn in answerButtons) btn.interactable = false;

        AppLanguage currentLang = LanguageManager.Instance.CurrentLanguage;

        if (selectedIndex == activeQuestion.correctAnswerIndex)
        {
            score++;
            resultText.color = Color.green;
            resultText.text = currentLang == AppLanguage.RU ? "Правильно! +10 баллов" :
                              currentLang == AppLanguage.EN ? "Correct! +10 points" : "正确! +10 积分";
        }
        else
        {
            resultText.color = Color.red;
            resultText.text = currentLang == AppLanguage.RU ? "Неверно." :
                              currentLang == AppLanguage.EN ? "Incorrect." : "错误。";
        }

        nextButton.gameObject.SetActive(true);

        var nextBtnText = nextButton.GetComponentInChildren<TextMeshProUGUI>();
        if (nextBtnText != null)
        {
            nextBtnText.text = currentLang == AppLanguage.RU ? "Далее" :
                               currentLang == AppLanguage.EN ? "Next" : "下一步";
        }
    }

    public void OnNextQuestionPressed()
    {
        currentQuestionIndex++;
        DisplayQuestion();
    }

    private void EndQuest()
    {
        AppLanguage currentLang = LanguageManager.Instance.CurrentLanguage;
        string msg = "";

        switch (currentLang)
        {
            case AppLanguage.RU: msg = $"Квест пройден! Результат: {score}/{currentQuestQuestions.Count}."; break;
            case AppLanguage.EN: msg = $"Quest completed! Score: {score}/{currentQuestQuestions.Count}."; break;
            case AppLanguage.ZH: msg = $"任务完成！您的得分: {score}/{currentQuestQuestions.Count}。"; break;
        }

        questionText.text = msg;
        resultText.text = "";

        nextButton.gameObject.SetActive(true);
        var nextBtnText = nextButton.GetComponentInChildren<TextMeshProUGUI>();
        if (nextBtnText != null)
        {
            nextBtnText.text = currentLang == AppLanguage.RU ? "Выйти" :
                               currentLang == AppLanguage.EN ? "Exit" : "退出";
        }

        // Перенаправляем кнопку Далее, чтобы на финальном клике она закрывала квест и возвращала на карту
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(CloseQuestPanel);

        foreach (var btn in answerButtons) btn.gameObject.SetActive(false);
    }

    public void CloseQuestPanel()
    {
        questPanel.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(false); // Выключаем путеводитель

        // 🗺️ СИСТЕМНЫЙ ВОЗВРАТ НА КАРТУ ОСТРОВА С ОБНОВЛЕНИЕМ ПОЗИЦИИ ИГРОКА
        var gpsManager = FindFirstObjectByType<GPSQuestManager>();
        if (gpsManager != null)
        {
            if (gpsManager.mapPanel != null) gpsManager.mapPanel.SetActive(true); // Включаем карту
            if (gpsManager.routeLineRenderer != null) gpsManager.routeLineRenderer.gameObject.SetActive(false); // Скрываем старую линию дорог
            if (gpsManager.routeStatusPanel != null) gpsManager.routeStatusPanel.SetActive(false); // Скрываем нижнюю плашку навигации

            // Логический перенос точки геопозиции игрока в новый пройденный форт!
            if (gpsManager.activeLocationSaved != null && gpsManager.playerMarkerOnMap != null)
            {
                int index = gpsManager.locations.IndexOf(gpsManager.activeLocationSaved);
                if (index >= 0 && index < gpsManager.locationButtonsOnMap.Length && gpsManager.locationButtonsOnMap[index] != null)
                {
                    // Маркер перемещается на координаты только что изученной кнопки
                    gpsManager.playerMarkerOnMap.anchoredPosition = gpsManager.locationButtonsOnMap[index].GetComponent<RectTransform>().anchoredPosition;
                }
            }
        }

        foreach (var btn in answerButtons) btn.gameObject.SetActive(true);
    }
}
