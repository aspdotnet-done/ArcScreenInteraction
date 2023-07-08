using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using LitJson;

/// <summary>
/// 城市天气
/// </summary>
public class WeatherManager : MonoBehaviour
{
    public string privateKey;   //心知天气私钥
    public Image imgWeather;    //天气图片
    public Text textWeather;    //天气
    public Text textTemperature;//温度
    public Text textCity;       //城市

    void Start()
    {
        StartCoroutine(GetRuntimeWeather());
    }

    IEnumerator GetRuntimeWeather()
    {
        while (true)
        {
            //1.获取本地公网IP
            UnityWebRequest wwwWebIp = UnityWebRequest.Get(@"http://icanhazip.com/");
            yield return wwwWebIp.SendWebRequest();
            if (wwwWebIp.isNetworkError || wwwWebIp.isHttpError)
            {
                yield break;
            }
            //Debug.Log("IP：" + wwwWebIp.downloadHandler.text);
            //2.根据IP查询城市（心知天气提供接口，需要申请key）***这里别忘记修改
            string urlQueryCity = $"https://api.seniverse.com/v3/location/search.json?key={privateKey}&q={wwwWebIp.downloadHandler.text}";
            //Debug.Log("cityQuery:" + urlQueryCity);
            UnityWebRequest wwwQueryCity = UnityWebRequest.Get(urlQueryCity);
            yield return wwwQueryCity.SendWebRequest();
            if (wwwQueryCity.isNetworkError || wwwQueryCity.isHttpError)
            {
                yield break;
            }
            //Debug.Log("城市数据：" + wwwQueryCity.downloadHandler.text);
            JObject cityData = JsonConvert.DeserializeObject<JObject>(wwwQueryCity.downloadHandler.text);

            string cityId = cityData["results"][0]["id"].ToString();
            textCity.text = cityData["results"][0]["name"].ToString(); //城市

            //3.根据城市查询天气（心知天气提供接口，需要申请key）***这里别忘记修改
            string urlWeather = $"https://api.seniverse.com/v3/weather/now.json?key={privateKey}&location={cityId}&language=zh-Hans&unit=c";
            Debug.Log("weatherQuery:" + urlWeather);
            UnityWebRequest wwwWeather = UnityWebRequest.Get(urlWeather);
            yield return wwwWeather.SendWebRequest();

            if (wwwWeather.isNetworkError || wwwWeather.isHttpError)
            {
                Debug.Log(wwwWeather.error);
            }

            //4.解析天气
            try
            {
                JObject weatherData = JsonConvert.DeserializeObject<JObject>(wwwWeather.downloadHandler.text);
                string spriteName = string.Format("Weather/{0}@2x", weatherData["results"][0]["now"]["code"].ToString());

                //天气文字
                textWeather.text = weatherData["results"][0]["now"]["text"].ToString();


                //图片，可以在心知天气上下载
                imgWeather.sprite = Resources.Load<Sprite>(spriteName);
                //Debug.Log(spriteName);

                //温度
                textTemperature.text = string.Format("{0} °C", weatherData["results"][0]["now"]["temperature"].ToString());
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
            yield return new WaitForSeconds(60 * 20);
        }
    }
}


/*
 * 返回的Json天气格式
 * {
	"results": [{
		"location": {
			"id": "WX4FBXXFKE4F",
			"name": "北京",
			"country": "CN",
			"path": "北京,北京,中国",
			"timezone": "Asia/Shanghai",
			"timezone_offset": "+08:00"
		},
		"now": {
			"text": "晴",
			"code": "0",
			"temperature": "-10"
		},
		"last_update": "2021-01-08T09:20:00+08:00"
	}]
}
 */