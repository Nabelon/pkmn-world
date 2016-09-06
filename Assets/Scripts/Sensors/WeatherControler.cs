using UnityEngine;
using System.Collections;
using SimpleJSON;
public class WeatherControler{
    static WeatherControler controler;
    System.DateTime lastCall;
    float lastLat,lastLng = 360; 
    private string weather = "sun";

    public static WeatherControler getWeatherControler()
    {
        if (controler == null) controler = new WeatherControler();
        return controler;
    }
    public string getTime()
    {
        System.DateTime now = System.DateTime.Now;
        if (now.Hour < 4) return "night";
        if (now.Hour < 10) return "morning";
        if (now.Hour < 16) return "day";
        if (now.Hour < 22) return "evening";
        return "night";
    }
    public string getWeather(float lat, float lng)
    {
        try
        {
            //check if we can use last call's weather
            if (lastLat - lat < 0.005 && lastLng - lng < 0.005 && System.DateTime.Now.Subtract(lastCall).Minutes < 5) return weather;
            //download weather
            string url = "http://api.openweathermap.org//data//2.5//weather?lat=" + lat.ToString() + "&lon=" + lng.ToString() + "&APPID=0d3715954c8607d10b2cb7eeb3befefe";
            WWW request = new WWW(url);
            while (!request.isDone)
            {
                System.Threading.Thread.Sleep(100);
            }
            // Parse response into the SimpleJSON format
            JSONNode response = JSON.Parse(request.text);
            string tmpW = response["weather"][0]["main"].ToString();
            if (tmpW == "\"Clear\"" )
            {
                weather = "sun";
            }
            else if (tmpW.Equals("\"Clouds\""))
            {
                weather = "cloud";
            }
            else
            {
                weather = "rain";
            }
            lastLat = lat;
            lastLng = lng;
            lastCall = System.DateTime.Now;
        } catch  { Debug.Log("Error in weatherControler"); }
        return weather;
    }
    private WeatherControler()
    {
        
    }
}
