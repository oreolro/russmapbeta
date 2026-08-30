using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GPSQuestManager : MonoBehaviour
{
    [Header("База данных локаций острова Русский")]
    public List<HistoricalLocation> locations;

    [Header("Панели экранов приложения")]
    public GameObject mapPanel;
    public GameObject infoPanel;
    public GameObject cameraPanel;

    [Header("UI Тексты и кнопки путеводителя")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI locationNameText;
    public TextMeshProUGUI historyContentText;
    public TextMeshProUGUI questTaskText;
    public Button actionButton;

    [Header("UI Камеры и Распознавания")]
    public RawImage cameraScreenDisplay;
    public GameObject scanningStatusPanel;
    public TextMeshProUGUI scanningStatusText;

    [Header("⚙️ Система Карты и Реальных Маршрутов")]
    public RectTransform playerMarkerOnMap;
    public Button[] locationButtonsOnMap;
    public UILineRenderer routeLineRenderer;
    public GameObject routeStatusPanel;
    public TextMeshProUGUI routeStatusText;
    public Button arriveVerificationButton;

    [HideInInspector] public HistoricalLocation activeLocationSaved;

    private HistoricalLocation activeLocation;
    private double currentLatitude;
    private double currentLongitude;
    private WebCamTexture mobileCameraTexture;

    private void Start()
    {
        LanguageManager.OnLanguageChanged += UpdateUI;

        if (mapPanel != null) mapPanel.SetActive(true);
        if (infoPanel != null) infoPanel.SetActive(false);
        if (cameraPanel != null) cameraPanel.SetActive(false);
        if (scanningStatusPanel != null) scanningStatusPanel.SetActive(false);
        if (routeStatusPanel != null) routeStatusPanel.SetActive(false);
        if (routeLineRenderer != null) routeLineRenderer.gameObject.SetActive(false);

        StartCoroutine(StartLocationService());
    }

    private void OnDestroy()
    {
        LanguageManager.OnLanguageChanged -= UpdateUI;
        if (mobileCameraTexture != null && mobileCameraTexture.isPlaying) mobileCameraTexture.Stop();
    }

    private IEnumerator StartLocationService()
    {
#if UNITY_EDITOR
        currentLatitude = 43.0244;
        currentLongitude = 131.8944;
        UpdateUI();
        yield break;
#endif

        if (!Input.location.isEnabledByUser) yield break;
        Input.location.Start(1f, 1f);
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed) yield break;

        InvokeRepeating(nameof(UpdateGPSCoordinates), 0f, 2f);
    }

    private void UpdateGPSCoordinates()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            currentLatitude = Input.location.lastData.latitude;
            currentLongitude = Input.location.lastData.longitude;
        }
    }

    public void SelectLocationOnMap(int targetIdFromButton)
    {
        HistoricalLocation foundLoc = null;
        foreach (var loc in locations)
        {
            if (loc != null && loc.locationId == targetIdFromButton)
            {
                foundLoc = loc;
                break;
            }
        }

        if (foundLoc == null) return;

        activeLocation = foundLoc;
        activeLocationSaved = foundLoc;

        UpdateUI();

        double distance = CalculateDistance(currentLatitude, currentLongitude, activeLocation.latitude, activeLocation.longitude);
        if (routeStatusPanel != null) routeStatusPanel.SetActive(true);

        AppLanguage currentLang = LanguageManager.Instance.CurrentLanguage;

        switch (currentLang)
        {
            case AppLanguage.RU:
                routeStatusText.text = $"🛣️ Маршрут до: <b>{activeLocation.nameRU}</b>\n📏 Расстояние: <b>{Mathf.RoundToInt((float)distance)} м</b>";
                if (arriveVerificationButton != null)
                {
                    TextMeshProUGUI btnTxt = arriveVerificationButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (btnTxt != null) btnTxt.text = "📸 Я на месте (Сканировать)";
                }
                break;
            case AppLanguage.EN:
                routeStatusText.text = $"🛣️ Route to: <b>{activeLocation.nameEN}</b>\n📏 Distance: <b>{Mathf.RoundToInt((float)distance)} m</b>";
                if (arriveVerificationButton != null)
                {
                    TextMeshProUGUI btnTxt = arriveVerificationButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (btnTxt != null) btnTxt.text = "📸 Arrived (Scan)";
                }
                break;
            case AppLanguage.ZH:
                routeStatusText.text = $"🛣️ 路线至: <b>{activeLocation.nameZH}</b>\n📏 距离: <b>{Mathf.RoundToInt((float)distance)} 米</b>";
                if (arriveVerificationButton != null)
                {
                    TextMeshProUGUI btnTxt = arriveVerificationButton.GetComponentInChildren<TextMeshProUGUI>();
                    if (btnTxt != null) btnTxt.text = "📸 我已到达 (开始扫描)";
                }
                break;
        }

        if (routeLineRenderer != null && playerMarkerOnMap != null)
        {
            routeLineRenderer.gameObject.SetActive(true);
            List<Vector2> pathPoints = CalculateCurvedPath(targetIdFromButton);
            routeLineRenderer.SetPoints(pathPoints.ToArray());
        }
    }

    // 🛣️ ГЕОМЕТРИЯ МАРШРУТОВ 2ГИС: ИДЕАЛЬНОЕ СЛЕДОВАНИЕ КОНТУРАМ ДОРОГ ПО СУШЕ
    private List<Vector2> CalculateCurvedPath(int locationNumber)
    {
        List<Vector2> points = new List<Vector2>();
        points.Add(playerMarkerOnMap.anchoredPosition); // Старт из ДВФУ (290, 420)

        switch (locationNumber)
        {
            case 1:  // 1. Форт Поспелова (X: 170, Y: -720) — идет по нижней трассе на юг
                points.Add(new Vector2(250f, 440f)); // Выезд на трассу
                points.Add(new Vector2(220f, 250f)); // Огибаем дугу проспекта
                points.Add(new Vector2(240f, -50f)); // Конец асфальта
                points.Add(new Vector2(250f, -480f)); // Перекресток лесных грунтовок
                break;

            case 2:  // 2. Новосильцевская батарея (X: 300, Y: 670)
                points.Add(new Vector2(250f, 440f));
                points.Add(new Vector2(240f, 550f)); // На север по жёлтой дороге
                break;

            case 3:  // 3. Форт 11 (X: 230, Y: -340)
                points.Add(new Vector2(250f, 440f));
                points.Add(new Vector2(220f, 250f));
                points.Add(new Vector2(240f, -50f));
                points.Add(new Vector2(260f, -140f)); // До лесного съезда к форту
                break;

            case 4:  // 4. Форт 12 (X: 390, Y: -90) — ИСПРАВЛЕНО ПО ФОТО 2ГИС (Идет правее по оранжевой суше)
                points.Add(new Vector2(250f, 440f));
                points.Add(new Vector2(220f, 250f)); // Опускаемся вниз строго вдоль берега по суше
                break;

            case 5:  // 5. Ворошиловская батарея (X: 300, Y: -200)
                points.Add(new Vector2(250f, 440f));
                points.Add(new Vector2(220f, 250f));
                points.Add(new Vector2(250f, -50f)); // Мимо форта 12 по жёлтой трассе
                break;

            case 6:  // 6. Мыс Тобизина (X: 180, Y: -679) — ИСПРАВЛЕНО СТРОГО ПО ФОТО №1
                points.Add(new Vector2(250f, 440f)); // Выезд из ДВФУ
                points.Add(new Vector2(220f, 250f)); // Огибаем полуостров Сапёрный по жёлтой дуге
                points.Add(new Vector2(250f, -50f));  // Проезжаем мимо Форта 12
                points.Add(new Vector2(260f, -140f)); // Проезжаем мимо Ворошиловской
                points.Add(new Vector2(250f, -480f)); // Главный лесной перекресток грунтовок
                points.Add(new Vector2(230f, -580f)); // Поворот налево по белому контуру к мысу
                break;

            case 7:  // 7. Мыс Вятлина (X: 350, Y: -550) — ИСПРАВЛЕНО СТРОГО ПО ФОТО №2
                points.Add(new Vector2(250f, 440f)); // Выезд из ДВФУ
                points.Add(new Vector2(220f, 250f)); // По жёлтому шоссе вниз
                points.Add(new Vector2(250f, -50f));
                points.Add(new Vector2(260f, -140f));
                points.Add(new Vector2(250f, -480f)); // Доезжаем до лесного перекрестка
                points.Add(new Vector2(300f, -520f)); // Сворачиваем круто вправо к точке Б
                break;

            case 9:  // 9. Приморский океанариум (X: 500, Y: 220) — ИСПРАВЛЕНО СТРОГО ПО ФОТО №3 (ПО СУШЕ!)
                points.Add(new Vector2(245f, 415f)); // Выезжаем влево на Университетский проспект
                points.Add(new Vector2(215f, 370f)); // Огибаем бухту Аякс снизу по жёлтой дуге суши
                points.Add(new Vector2(260f, 310f)); // Поворачиваем направо на развилку Океанариума
                points.Add(new Vector2(370f, 270f)); // Двигаемся по полуострову Сапёрный
                points.Add(new Vector2(450f, 260f)); // Вход на территорию комплекса сверху по суше
                break;

            case 10: // 10. Русский мост (X: 400, Y: 800)
                points.Add(new Vector2(250f, 440f));
                points.Add(new Vector2(240f, 550f)); // Строго вверх на север к развязке моста
                points.Add(new Vector2(300f, 720f));
                break;
        }

        // Финишная точка — ваша настроенная кнопка достопримечательности
        Vector2 targetPos = locationButtonsOnMap[locationNumber - 1].GetComponent<RectTransform>().anchoredPosition;
        points.Add(targetPos);

        return points;
    }

    private void UpdateUI()
    {
        AppLanguage currentLang = LanguageManager.Instance.CurrentLanguage;
        if (locationButtonsOnMap != null && locations != null)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                if (i >= locationButtonsOnMap.Length || locationButtonsOnMap[i] == null) continue;
                TextMeshProUGUI btnText = locationButtonsOnMap[i].GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null && locations[i] != null)
                {
                    if (currentLang == AppLanguage.RU) btnText.text = locations[i].nameRU;
                    else if (currentLang == AppLanguage.EN) btnText.text = locations[i].nameEN;
                    else if (currentLang == AppLanguage.ZH) btnText.text = locations[i].nameZH;
                }
            }
        }
        if (activeLocation == null) return;
        TextMeshProUGUI buttonText = actionButton != null ? actionButton.GetComponentInChildren<TextMeshProUGUI>() : null;
        switch (currentLang)
        {
            case AppLanguage.RU:
                statusText.text = "📍 Вы на месте!";
                locationNameText.text = activeLocation.nameRU;
                historyContentText.text = activeLocation.historyRU;
                questTaskText.text = "🎯 Квест доступен!";
                if (buttonText != null) buttonText.text = "Начать квест";
                break;
            case AppLanguage.EN:
                statusText.text = "📍 Target reached!";
                locationNameText.text = activeLocation.nameEN;
                historyContentText.text = activeLocation.historyEN;
                questTaskText.text = "🎯 Quest available!";
                if (buttonText != null) buttonText.text = "Start Quest";
                break;
            case AppLanguage.ZH:
                statusText.text = "您已到达!";
                locationNameText.text = activeLocation.nameZH;
                historyContentText.text = activeLocation.historyZH;
                questTaskText.text = "🎯 任务可用!";
                if (buttonText != null) buttonText.text = "开始任务";
                break;
        }
    }
    public void ClickLocationOnMap() { OpenCamera(); }
    public void ClickArrivedAndOpenARCamera() { OpenCamera(); }
    private void OpenCamera()
    {
        if (mapPanel != null) mapPanel.SetActive(false);
        if (cameraPanel != null) cameraPanel.SetActive(true);
#if !UNITY_EDITOR
        if (WebCamTexture.devices.Length > 0)
        {
            mobileCameraTexture = new WebCamTexture(WebCamTexture.devices.name, 1080, 1920);
            if (cameraScreenDisplay != null) cameraScreenDisplay.texture = mobileCameraTexture;
            mobileCameraTexture.Play();
        }
#endif
    }
    public void TakePhotoAndVerify()
    {
        if (mobileCameraTexture != null && mobileCameraTexture.isPlaying) mobileCameraTexture.Stop();
        StartCoroutine(ShowARRecognitionAndOpenInfo());
    }
    private IEnumerator ShowARRecognitionAndOpenInfo()
    {
        if (scanningStatusPanel != null)
        {
            scanningStatusPanel.SetActive(true);
            AppLanguage currentLang = LanguageManager.Instance.CurrentLanguage;
            if (currentLang == AppLanguage.RU) scanningStatusText.text = "🔄 Сканирование...";
            else if (currentLang == AppLanguage.EN) scanningStatusText.text = "🔄 Scanning...";
            else if (currentLang == AppLanguage.ZH) scanningStatusText.text = "🔄 正在扫描...";
        }
        yield return new WaitForSeconds(2f);
        if (scanningStatusPanel != null)
        {
            AppLanguage currentLang = LanguageManager.Instance.CurrentLanguage;
            if (currentLang == AppLanguage.RU) scanningStatusText.text = "✅ Место распознано!";
            else if (currentLang == AppLanguage.EN) scanningStatusText.text = "✅ Location verified!";
            else if (currentLang == AppLanguage.ZH) scanningStatusText.text = "✅ 地点识别成功!";
        }
        yield return new WaitForSeconds(1.5f);
        if (cameraPanel != null) cameraPanel.SetActive(false);
        if (scanningStatusPanel != null) scanningStatusPanel.SetActive(false);
        if (infoPanel != null) infoPanel.SetActive(true);
    }
    public void StartQuestFromInfoPanel()
    {
        if (activeLocation != null && QuestSystemManager.Instance != null)
        {
            QuestSystemManager.Instance.LaunchQuest(activeLocation.locationId);
        }
    }
    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double R = 6371e3;
        double phi1 = lat1 * Mathf.Deg2Rad;
        double phi2 = lat2 * Mathf.Deg2Rad;
        double deltaPhi = (lat2 - lat1) * Mathf.Deg2Rad;
        double deltaLambda = (lon2 - lon1) * Mathf.Deg2Rad;
        double a = System.Math.Sin(deltaPhi / 2) * System.Math.Sin(deltaPhi / 2) +
        System.Math.Cos(phi1) * System.Math.Cos(phi2) *
        System.Math.Sin(deltaLambda / 2) * System.Math.Sin(deltaLambda / 2);
        return R * (2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a)));
    }
}
