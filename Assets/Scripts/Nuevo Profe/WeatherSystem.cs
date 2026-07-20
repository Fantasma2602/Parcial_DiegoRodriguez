using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    [SerializeField] private Weather currentWeather;
    [SerializeField] private Light ligthReference;

    public Weather GetWeather()
    {
        return currentWeather;
    }
    private void Awake()
    {
        StartCoroutine(RandomWeather());
    }

    public void SetWeather(Weather newWeather)
    {
        currentWeather = newWeather;

        switch (newWeather)
        {
            case Weather.Sunny:
                {
                    ligthReference.intensity = 6;
                    ligthReference.color = new Color(1, 0.9492415f, 0);
                    break;
                }
            case Weather.Rainny:
                {
                    ligthReference.intensity = 50;
                    ligthReference.color = new Color(0, 0.08501413f, 0.2641509f);
                    break;
                }
            case Weather.Cloudy:
                {
                    ligthReference.intensity = 0.6f;
                    ligthReference.color = new Color(0.4575472f, 0.765339f, 1);
                    break;
                }
        }



        Debug.Log("Se cambio el clima a: " + currentWeather.ToString());
    }
    private IEnumerator RandomWeather()
    {
        yield return new WaitForSeconds(5f);
        int numero = Random.Range(0, 10);

        switch (numero)
        {
            case 0:
                {
                    SetWeather(Weather.Sunny);
                    break;
                }
            case 5:
                {
                    SetWeather(Weather.Rainny);
                    break;
                }
            case 7:
                {
                    SetWeather(Weather.Cloudy);
                    break;
                }
        }

        StartCoroutine(RandomWeather());

    }
}
public enum Weather
{
    Sunny,
    Rainny,
    Cloudy,
}
