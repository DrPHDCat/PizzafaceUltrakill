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
        private void Awake()
        {
            Instance = this;
            log = Logger;
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            assetBundle = AssetBundle.LoadFromFile(Path.Combine(Paths.PluginPath, "Pizzaface\\Assets\\pizzaface"));
            fatass = assetBundle.LoadAsset<AudioClip>("fatass");
            pizzaface = assetBundle.LoadAsset<GameObject>("pizzaface");
            pizzaface.AddComponent<Pizzaface>();
            Harmony har = new Harmony(PluginInfo.PLUGIN_GUID);
            har.PatchAll();
            
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
                transform.position = Vector3.MoveTowards(transform.position, MonoSingleton<NewMovement>.Instance.transform.position + yOffset, Time.deltaTime * 20f);
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
                GameObject fatass = new GameObject();
                fatass.AddComponent<AudioSource>();
                fatass.AddComponent<Fatass>();
                fatass.GetComponent<AudioSource>().playOnAwake = false;
                fatass.name = "fatass";
                Instantiate(fatass, MonoSingleton<NewMovement>.Instance.gameObject.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.gameObject.transform);
                MonoSingleton<NewMovement>.Instance.GetHurt(999, false, 1, false, false);
                Destroy(gameObject);
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
            pizzaTrail.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            pizzaTrail.GetComponent<SpriteRenderer>().color = new Vector4(1, 0.5f, 0.5f, 1);
            pizzaTrail.transform.localScale = transform.localScale;
            pizzaTrail.name = "Pizza trail";
            Instantiate(pizzaTrail, transform.position, transform.rotation);
            timeUntilTrail = 0.5f;
            Destroy(gameObject);
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
}
