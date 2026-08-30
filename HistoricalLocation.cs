using UnityEngine;

[CreateAssetMenu(fileName = "NewLocation", menuName = "RusskyMap/LocationData")]
public class HistoricalLocation : ScriptableObject
{
    [Header("Уникальный ID локации")]
    public int locationId;

    [Header("Геолокация (GPS)")]
    public double latitude;
    public double longitude;
    public float triggerRadiusMeter = 50f;

    [Header("Названия (RU / EN / ZH)")]
    public string nameRU;
    public string nameEN;
    public string nameZH;

    [Header("Историческая справка (RU)")]
    [TextArea(5, 10)] public string historyRU;

    [Header("Историческая справка (EN)")]
    [TextArea(5, 10)] public string historyEN;

    [Header("Историческая справка (ZH)")]
    [TextArea(5, 10)] public string historyZH;

    [Header("Задание квеста (RU)")]
    public string questTaskRU;

    [Header("Задание квеста (EN)")]
    public string questTaskEN;

    [Header("Задание квеста (ZH)")]
    public string questTaskZH;

    [Header("AR Префаб (3D-модель пушки/лиса/киота)")]
    public GameObject arModelPrefab;
}