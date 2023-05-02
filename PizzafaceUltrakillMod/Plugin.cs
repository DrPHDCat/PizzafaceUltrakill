using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Logic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace PizzafaceUltrakillMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public AssetBundle assetBundle;
        public static Plugin Instance;
        public AudioClip fatass;
        public GameObject pizzaface;
        public ManualLogSource log;
        public AudioClip pRank;
        public AudioClip sRank;
        public AudioClip aRank;
        public AudioClip cRank;
        public AudioClip dRank;
        public AudioClip preRank;
        public Sprite sadPizza;
        private void Awake()
        {
            Instance = this;
            log = Logger;
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            assetBundle = AssetBundle.LoadFromMemory(Properties.Resources.pizzaface);
            fatass = assetBundle.LoadAsset<AudioClip>("fatass");
            pizzaface = assetBundle.LoadAsset<GameObject>("pizzaface");
            pizzaface.transform.localScale = new Vector3(5f, 5f, 5f);
            pizzaface.AddComponent<Pizzaface>();
            Harmony har = new Harmony(PluginInfo.PLUGIN_GUID);
            har.PatchAll();
            pRank = assetBundle.LoadAsset<AudioClip>("VictoriousP");
            sRank = assetBundle.LoadAsset<AudioClip>("VictoriousS");
            aRank = assetBundle.LoadAsset<AudioClip>("VictoriousA");
            cRank = assetBundle.LoadAsset<AudioClip>("VictoriousC");
            dRank = assetBundle.LoadAsset<AudioClip>("VictoriousD");
            preRank = assetBundle.LoadAsset<AudioClip>("VictoriousPre");
            sadPizza = assetBundle.LoadAsset<Sprite>("SadPizza");
        }
        public void ResetScene()
        {
            SceneHelper.LoadScene(SceneHelper.CurrentScene);
        }

    }
    public class Pizzaface : MonoBehaviour
    {
        public static Pizzaface Instance;
        public float timeUntilMovement;
        public float transparency;
        public float timeUntilTrail;
        public Vector3 yOffset;
        public void Awake()
        {
            Instance = this;

        }
        public void Start()
        {
            timeUntilMovement = 1.5f;
            GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0);
            transparency = 0;
            yOffset = new Vector3(0, 1.5f, 0);
            timeUntilTrail = 0.5f;

        }
        public void Update()
        {
            timeUntilMovement = Mathf.MoveTowards(timeUntilMovement, 0, Time.deltaTime);
            timeUntilTrail = Mathf.MoveTowards(timeUntilTrail, 0, Time.deltaTime);
            transparency = Mathf.MoveTowards(transparency, 1, Time.deltaTime / 1.5f);
            GetComponent<SpriteRenderer>().color = new Vector4(1, transparency, transparency, transparency);
            if (timeUntilMovement <= 0)
            {
                if (Vector3.Distance(transform.position, MonoSingleton<NewMovement>.Instance.transform.position) > 50f)
                {
                    float speedMod = (Vector3.Distance(transform.position, MonoSingleton<NewMovement>.Instance.transform.position) - 50) * 3;
                    transform.position = Vector3.MoveTowards(transform.position, MonoSingleton<NewMovement>.Instance.transform.position + yOffset, Time.deltaTime * (17f + speedMod));
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, MonoSingleton<NewMovement>.Instance.transform.position + yOffset, Time.deltaTime * 17f);
                }

            }
            transform.rotation = Quaternion.LookRotation(Camera.current.transform.position - transform.position);
            if (timeUntilTrail <= 0)
            {
                GameObject pizzaTrail = new GameObject();
                pizzaTrail.AddComponent<SpriteRenderer>();
                pizzaTrail.AddComponent<PizzafaceTrail>();
                pizzaTrail.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                pizzaTrail.GetComponent<SpriteRenderer>().color = new Vector4(1, 0.5f, 0.5f, 1);
                pizzaTrail.transform.localScale = transform.localScale;
                pizzaTrail.name = "Pizza trail";
                Instantiate(pizzaTrail, transform.position, transform.rotation);
                timeUntilTrail = 0.5f;
            }

        }
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && timeUntilMovement <= 0)
            {
                DestroyPizza(true);
            }
        }
        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && timeUntilMovement <= 0)
            {
                DestroyPizza(true);
            }

        }
        public void DestroyPizza(bool kill)
        {
            if (kill)
            {
                GameObject fatass = new GameObject();
                fatass.AddComponent<AudioSource>();
                fatass.AddComponent<Fatass>();
                fatass.GetComponent<AudioSource>().playOnAwake = false;
                fatass.name = "fatass";
                Instantiate(fatass, MonoSingleton<NewMovement>.Instance.gameObject.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.gameObject.transform);
                MonoSingleton<NewMovement>.Instance.GetHurt(999, false, 1, false, false);

            }

            GameObject pizzaTrail = new GameObject();
            pizzaTrail.AddComponent<SpriteRenderer>();
            pizzaTrail.AddComponent<PizzafaceTrail>();
            if (kill)
            {
                pizzaTrail.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                pizzaTrail.GetComponent<SpriteRenderer>().color = new Vector4(1, 0.5f, 0.5f, 1);
            }
            if (!kill)
            {
                pizzaTrail.GetComponent<SpriteRenderer>().sprite = Plugin.Instance.sadPizza;
                pizzaTrail.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1);
            }
            pizzaTrail.transform.localScale = transform.localScale;
            pizzaTrail.name = "Pizza trail";
            Instantiate(pizzaTrail, transform.position, transform.rotation);
            timeUntilTrail = 0.5f;
            Destroy(gameObject);
            if (kill && SceneHelper.CurrentScene != "Endless")
            {
                SceneHelper.LoadScene(SceneHelper.CurrentScene);
            }
        }
    }
    public class PizzafaceTrail : MonoBehaviour
    {
        public void Update()
        {
            GetComponent<SpriteRenderer>().color = Vector4.MoveTowards(GetComponent<SpriteRenderer>().color, new Vector4(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0), Time.deltaTime / 3);
            if (GetComponent<SpriteRenderer>().color.a == 0)
            {
                Destroy(gameObject);
            }
            transform.rotation = Quaternion.LookRotation(Camera.current.transform.position - transform.position);
        }
    }
    public class Fatass : MonoBehaviour
    {
        public float timeUntilDestroy;
        void Awake()
        {
            GetComponent<AudioSource>().clip = Plugin.Instance.fatass;
            GetComponent<AudioSource>().Play();
            timeUntilDestroy = 7f;
        }
        void Update()
        {
            timeUntilDestroy = Mathf.MoveTowards(timeUntilDestroy, 0, Time.deltaTime);
            if (timeUntilDestroy <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    [HarmonyPatch(typeof(NewMovement), "Update")]
    public static class NMupdate
    {
        [HarmonyPostfix]
        public static void Postfix(NewMovement __instance)
        {
            if (SceneHelper.CurrentScene == "Endless")
            {
                if (Pizzaface.Instance == null && MonoSingleton<StatsManager>.Instance.timer && !__instance.dead)
                {
                    Object.Instantiate(PizzafaceUltrakillMod.Plugin.Instance.pizzaface, __instance.transform.position, Quaternion.identity);

                }
            }
            else if (Pizzaface.Instance == null && MonoSingleton<StatsManager>.Instance.timer && !MonoSingleton<FinalRank>.Instance.gameObject.activeSelf && !__instance.dead && SceneHelper.CurrentScene != "uk_construct")
            {
                Object.Instantiate(PizzafaceUltrakillMod.Plugin.Instance.pizzaface, __instance.transform.position, Quaternion.identity);

            }
            else if (Pizzaface.Instance != null)
            {
                if (!MonoSingleton<StatsManager>.Instance.timer || MonoSingleton<FinalRank>.Instance.gameObject.activeSelf || __instance.dead || SceneHelper.CurrentScene == "uk_construct")
                {
                    Pizzaface.Instance.DestroyPizza(false);
                }

            }
        }
    }
    public class Played : MonoBehaviour
    {
        public bool played;
    }
    [HarmonyPatch(typeof(FinalRank), "Update")]
    static class Pranked
    {
        [HarmonyPostfix]
        static void Postfix(FinalRank __instance)
        {
            if (__instance.totalRank.text == "<color=#FFFFFF>P</color>" && __instance.totalRank.gameObject.activeSelf == true && !__instance.GetComponent<Played>().played)
            {
                __instance.GetComponent<Played>().played = true;
                GameObject go = new GameObject();
                GameObject sound = Object.Instantiate(go, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
                sound.AddComponent<AudioSource>();
                sound.GetComponent<AudioSource>().playOnAwake = false;
                sound.GetComponent<AudioSource>().clip = PizzafaceUltrakillMod.Plugin.Instance.pRank;
                sound.GetComponent<AudioSource>().Play();
            }
            if (__instance.totalRank.text == "<color=#FF6A00>A</color>" && __instance.totalRank.gameObject.activeSelf == true && !__instance.GetComponent<Played>().played)
            {
                __instance.GetComponent<Played>().played = true;
                GameObject go = new GameObject();
                GameObject sound = Object.Instantiate(go, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
                sound.AddComponent<AudioSource>();
                sound.GetComponent<AudioSource>().playOnAwake = false;
                sound.GetComponent<AudioSource>().clip = PizzafaceUltrakillMod.Plugin.Instance.aRank;
                sound.GetComponent<AudioSource>().Play();
            }
            if (__instance.totalRank.text == "<color=#FF0000>S</color>" && __instance.totalRank.gameObject.activeSelf == true && !__instance.GetComponent<Played>().played)
            {
                __instance.GetComponent<Played>().played = true;
                GameObject go = new GameObject();
                GameObject sound = Object.Instantiate(go, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
                sound.AddComponent<AudioSource>();
                sound.GetComponent<AudioSource>().playOnAwake = false;
                sound.GetComponent<AudioSource>().clip = PizzafaceUltrakillMod.Plugin.Instance.sRank;
                sound.GetComponent<AudioSource>().Play();
            }
            if (__instance.totalRank.text == "<color=#FFD800>B</color>" && __instance.totalRank.gameObject.activeSelf == true && !__instance.GetComponent<Played>().played)
            {
                __instance.GetComponent<Played>().played = true;
                GameObject go = new GameObject();
                GameObject sound = Object.Instantiate(go, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
                sound.AddComponent<AudioSource>();
                sound.GetComponent<AudioSource>().playOnAwake = false;
                sound.GetComponent<AudioSource>().clip = PizzafaceUltrakillMod.Plugin.Instance.cRank;
                sound.GetComponent<AudioSource>().Play();
            }
            if (__instance.totalRank.text == "<color=#4CFF00>C</color>" && __instance.totalRank.gameObject.activeSelf == true && !__instance.GetComponent<Played>().played)
            {
                __instance.GetComponent<Played>().played = true;
                GameObject go = new GameObject();
                GameObject sound = Object.Instantiate(go, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
                sound.AddComponent<AudioSource>();
                sound.GetComponent<AudioSource>().playOnAwake = false;
                sound.GetComponent<AudioSource>().clip = PizzafaceUltrakillMod.Plugin.Instance.cRank;
                sound.GetComponent<AudioSource>().Play();
            }
            if (__instance.totalRank.text == "<color=#0094ff>D</color>" && __instance.totalRank.gameObject.activeSelf == true && !__instance.GetComponent<Played>().played)
            {
                __instance.GetComponent<Played>().played = true;
                GameObject go = new GameObject();
                GameObject sound = Object.Instantiate(go, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
                sound.AddComponent<AudioSource>();
                sound.GetComponent<AudioSource>().playOnAwake = false;
                sound.GetComponent<AudioSource>().clip = PizzafaceUltrakillMod.Plugin.Instance.dRank;
                sound.GetComponent<AudioSource>().Play();
            }
            if (__instance.GetComponent<Played>().played)
            {
                for (int i = 0; i < MonoSingleton<NewMovement>.Instance.transform.childCount; i++)
                {
                    if (MonoSingleton<NewMovement>.Instance.transform.GetChild(i).name == "preRankPlayer")
                    {
                        Object.Destroy(MonoSingleton<NewMovement>.Instance.transform.GetChild(i).gameObject);
                        break;
                    }
                }
            }
        }
    }
    [HarmonyPatch(typeof(FinalRank), "Start")]
    static class PrankedStart
    {
        [HarmonyPostfix]
        static void Postfix(FinalRank __instance)
        {
            __instance.gameObject.AddComponent<Played>();
            __instance.GetComponent<Played>().played = false;
            GameObject go = new GameObject();

        }
    }
}
