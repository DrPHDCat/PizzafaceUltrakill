using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Logic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[BepInPlugin("PizzafaceUltrakillMod", "PizzafaceUltrakillMod", "1.2.2")]
public class Plugin : BaseUnityPlugin
{
    // Token: 0x06000003 RID: 3 RVA: 0x0000206C File Offset: 0x0000026C
    private void Awake()
    {
        Plugin.Instance = this;
        this.log = base.Logger;
        base.Logger.LogInfo("Plugin PizzafaceUltrakillMod is loaded!");
        this.assetBundle = AssetBundle.LoadFromMemory(PizzafaceUltrakillMod.Properties.Resources.pizzaface);
        this.fatass = this.assetBundle.LoadAsset<AudioClip>("fatass");
        this.pizzaface = this.assetBundle.LoadAsset<GameObject>("pizzaface");
        this.pizzaface.transform.localScale = new Vector3(5f, 5f, 5f);
        this.pizzaface.AddComponent<Pizzaface>();
        Harmony harmony = new Harmony("PizzafaceUltrakillMod");
        harmony.PatchAll();
        this.pRank = this.assetBundle.LoadAsset<AudioClip>("VictoriousP");
        this.sRank = this.assetBundle.LoadAsset<AudioClip>("VictoriousS");
        this.aRank = this.assetBundle.LoadAsset<AudioClip>("VictoriousA");
        this.cRank = this.assetBundle.LoadAsset<AudioClip>("VictoriousC");
        this.dRank = this.assetBundle.LoadAsset<AudioClip>("VictoriousD");
        this.preRank = this.assetBundle.LoadAsset<AudioClip>("VictoriousPre");
        this.sadPizza = this.assetBundle.LoadAsset<Sprite>("SadPizza");
    }

    // Token: 0x06000004 RID: 4 RVA: 0x000021B6 File Offset: 0x000003B6
    public void ResetScene()
    {
        SceneHelper.LoadScene(SceneHelper.CurrentScene, false);
    }

    // Token: 0x04000002 RID: 2
    public AssetBundle assetBundle;

    // Token: 0x04000003 RID: 3
    public static Plugin Instance;

    // Token: 0x04000004 RID: 4
    public AudioClip fatass;

    // Token: 0x04000005 RID: 5
    public GameObject pizzaface;

    // Token: 0x04000006 RID: 6
    public ManualLogSource log;

    // Token: 0x04000007 RID: 7
    public AudioClip pRank;

    // Token: 0x04000008 RID: 8
    public AudioClip sRank;

    // Token: 0x04000009 RID: 9
    public AudioClip aRank;

    // Token: 0x0400000A RID: 10
    public AudioClip cRank;

    // Token: 0x0400000B RID: 11
    public AudioClip dRank;

    // Token: 0x0400000C RID: 12
    public AudioClip preRank;

    // Token: 0x0400000D RID: 13
    public Sprite sadPizza;
}
public class Pizzaface : MonoBehaviour
{
    // Token: 0x06000006 RID: 6 RVA: 0x000021CE File Offset: 0x000003CE
    public void Awake()
    {
        Pizzaface.Instance = this;
    }

    // Token: 0x06000007 RID: 7 RVA: 0x000021D8 File Offset: 0x000003D8
    public void Start()
    {
        this.timeUntilMovement = 1.5f;
        base.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f);
        this.transparency = 0f;
        this.yOffset = new Vector3(0f, 1.5f, 0f);
        this.timeUntilTrail = 0.5f;
    }

    // Token: 0x06000008 RID: 8 RVA: 0x0000224C File Offset: 0x0000044C
    public void Update()
    {
        this.timeUntilMovement = Mathf.MoveTowards(this.timeUntilMovement, 0f, Time.deltaTime);
        this.timeUntilTrail = Mathf.MoveTowards(this.timeUntilTrail, 0f, Time.deltaTime);
        this.transparency = Mathf.MoveTowards(this.transparency, 1f, Time.deltaTime / 1.5f);
        base.GetComponent<SpriteRenderer>().color = new Vector4(1f, this.transparency, this.transparency, this.transparency);
        bool flag = this.timeUntilMovement <= 0f;
        if (flag)
        {
            bool flag2 = Vector3.Distance(base.transform.position, MonoSingleton<NewMovement>.Instance.transform.position) > 50f;
            if (flag2)
            {
                float num = (Vector3.Distance(base.transform.position, MonoSingleton<NewMovement>.Instance.transform.position) - 50f) * 3f;
                base.transform.position = Vector3.MoveTowards(base.transform.position, MonoSingleton<NewMovement>.Instance.transform.position + this.yOffset, Time.deltaTime * (17f + num));
            }
            else
            {
                base.transform.position = Vector3.MoveTowards(base.transform.position, MonoSingleton<NewMovement>.Instance.transform.position + this.yOffset, Time.deltaTime * 17f);
            }
        }
        base.transform.rotation = Quaternion.LookRotation(Camera.current.transform.position - base.transform.position);
        bool flag3 = Vector3.Distance(base.transform.position, MonoSingleton<NewMovement>.Instance.transform.position) < 25f;
        if (flag3)
        {
            base.transform.GetChild(0).GetComponent<AudioSource>().mute = false;
            bool flag4 = Vector3.Distance(base.transform.position, MonoSingleton<NewMovement>.Instance.transform.position) < 20f;
            if (flag4)
            {
                bool flag5 = Vector3.Distance(base.transform.position, MonoSingleton<NewMovement>.Instance.transform.position) < 15f;
                if (flag5)
                {
                    bool flag6 = Vector3.Distance(base.transform.position, MonoSingleton<NewMovement>.Instance.transform.position) < 10f;
                    if (flag6)
                    {
                        bool flag7 = Vector3.Distance(base.transform.position, MonoSingleton<NewMovement>.Instance.transform.position) < 5f;
                        if (flag7)
                        {
                            base.transform.GetChild(0).GetComponent<AudioSource>().volume = 1f;
                        }
                        else
                        {
                            base.transform.GetChild(0).GetComponent<AudioSource>().volume = 0.8f;
                        }
                    }
                    else
                    {
                        base.transform.GetChild(0).GetComponent<AudioSource>().volume = 0.6f;
                    }
                }
                else
                {
                    base.transform.GetChild(0).GetComponent<AudioSource>().volume = 0.4f;
                }
            }
            else
            {
                base.transform.GetChild(0).GetComponent<AudioSource>().volume = 0.2f;
            }
        }
        else
        {
            base.transform.GetChild(0).GetComponent<AudioSource>().mute = true;
        }
        bool flag8 = this.timeUntilTrail <= 0f;
        if (flag8)
        {
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.AddComponent<PizzafaceTrail>();
            gameObject.GetComponent<SpriteRenderer>().sprite = base.GetComponent<SpriteRenderer>().sprite;
            gameObject.GetComponent<SpriteRenderer>().color = new Vector4(1f, 0.5f, 0.5f, 1f);
            gameObject.transform.localScale = base.transform.localScale;
            gameObject.name = "Pizza trail";
            Object.Instantiate<GameObject>(gameObject, base.transform.position, base.transform.rotation);
            this.timeUntilTrail = 0.5f;
        }
    }

    // Token: 0x06000009 RID: 9 RVA: 0x00002688 File Offset: 0x00000888
    private void OnTriggerEnter(Collider other)
    {
        bool flag = other.CompareTag("Player") && this.timeUntilMovement <= 0f;
        if (flag)
        {
            this.DestroyPizza(true);
        }
    }

    // Token: 0x0600000A RID: 10 RVA: 0x000026C4 File Offset: 0x000008C4
    private void OnTriggerStay(Collider other)
    {
        bool flag = other.CompareTag("Player") && this.timeUntilMovement <= 0f;
        if (flag)
        {
            this.DestroyPizza(true);
        }
    }

    // Token: 0x0600000B RID: 11 RVA: 0x00002700 File Offset: 0x00000900
    public void DestroyPizza(bool kill)
    {
        if (kill)
        {
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<AudioSource>();
            gameObject.AddComponent<Fatass>();
            gameObject.GetComponent<AudioSource>().playOnAwake = false;
            gameObject.name = "fatass";
            Object.Instantiate<GameObject>(gameObject, MonoSingleton<NewMovement>.Instance.gameObject.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.gameObject.transform);
            MonoSingleton<NewMovement>.Instance.GetHurt(999, false, 1f, false, false);
        }
        GameObject gameObject2 = new GameObject();
        gameObject2.AddComponent<SpriteRenderer>();
        gameObject2.AddComponent<PizzafaceTrail>();
        if (kill)
        {
            gameObject2.GetComponent<SpriteRenderer>().sprite = base.GetComponent<SpriteRenderer>().sprite;
            gameObject2.GetComponent<SpriteRenderer>().color = new Vector4(1f, 0.5f, 0.5f, 1f);
        }
        bool flag = !kill;
        if (flag)
        {
            gameObject2.GetComponent<SpriteRenderer>().sprite = Plugin.Instance.sadPizza;
            gameObject2.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
        }
        gameObject2.transform.localScale = base.transform.localScale;
        gameObject2.name = "Pizza trail";
        Object.Instantiate<GameObject>(gameObject2, base.transform.position, base.transform.rotation);
        this.timeUntilTrail = 0.5f;
        Object.Destroy(base.gameObject);
        bool flag2 = kill && SceneHelper.CurrentScene != "Endless";
        if (flag2)
        {
            SceneHelper.LoadScene(SceneHelper.CurrentScene, false);
        }
    }

    // Token: 0x0400000E RID: 14
    public static Pizzaface Instance;

    // Token: 0x0400000F RID: 15
    public float timeUntilMovement;

    // Token: 0x04000010 RID: 16
    public float transparency;

    // Token: 0x04000011 RID: 17
    public float timeUntilTrail;

    // Token: 0x04000012 RID: 18
    public Vector3 yOffset;
}
public class PizzafaceTrail : MonoBehaviour
{
    // Token: 0x0600000D RID: 13 RVA: 0x000028BC File Offset: 0x00000ABC
    public void Update()
    {
        base.GetComponent<SpriteRenderer>().color = Vector4.MoveTowards(base.GetComponent<SpriteRenderer>().color, new Vector4(base.GetComponent<SpriteRenderer>().color.r, base.GetComponent<SpriteRenderer>().color.g, base.GetComponent<SpriteRenderer>().color.b, 0f), Time.deltaTime / 3f);
        bool flag = base.GetComponent<SpriteRenderer>().color.a == 0f;
        if (flag)
        {
            Object.Destroy(base.gameObject);
        }
        base.transform.rotation = Quaternion.LookRotation(Camera.current.transform.position - base.transform.position);
    }
}
public class Fatass : MonoBehaviour
{
    // Token: 0x0600000F RID: 15 RVA: 0x00002997 File Offset: 0x00000B97
    private void Awake()
    {
        base.GetComponent<AudioSource>().clip = Plugin.Instance.fatass;
        base.GetComponent<AudioSource>().Play();
        this.timeUntilDestroy = 7f;
    }

    // Token: 0x06000010 RID: 16 RVA: 0x000029C8 File Offset: 0x00000BC8
    private void Update()
    {
        this.timeUntilDestroy = Mathf.MoveTowards(this.timeUntilDestroy, 0f, Time.deltaTime);
        bool flag = this.timeUntilDestroy <= 0f;
        if (flag)
        {
            Object.Destroy(base.gameObject);
        }
    }

    // Token: 0x04000013 RID: 19
    public float timeUntilDestroy;
}
[HarmonyPatch(typeof(NewMovement), "Update")]
public static class NMupdate
{
    // Token: 0x06000012 RID: 18 RVA: 0x00002A1C File Offset: 0x00000C1C
    [HarmonyPostfix]
    public static void Postfix(NewMovement __instance)
    {
        bool flag = SceneHelper.CurrentScene == "Endless";
        if (flag)
        {
            bool flag2 = Pizzaface.Instance == null && MonoSingleton<StatsManager>.Instance.timer && !__instance.dead;
            if (flag2)
            {
                Object.Instantiate<GameObject>(Plugin.Instance.pizzaface, __instance.transform.position, Quaternion.identity);
            }
        }
        else
        {
            bool flag3 = Pizzaface.Instance == null && MonoSingleton<StatsManager>.Instance.timer && !MonoSingleton<FinalRank>.Instance.gameObject.activeSelf && !__instance.dead && SceneHelper.CurrentScene != "uk_construct";
            if (flag3)
            {
                Object.Instantiate<GameObject>(Plugin.Instance.pizzaface, __instance.transform.position, Quaternion.identity);
            }
            else
            {
                bool flag4 = Pizzaface.Instance != null;
                if (flag4)
                {
                    bool flag5 = !MonoSingleton<StatsManager>.Instance.timer || MonoSingleton<FinalRank>.Instance.gameObject.activeSelf || __instance.dead || SceneHelper.CurrentScene == "uk_construct";
                    if (flag5)
                    {
                        Pizzaface.Instance.DestroyPizza(false);
                    }
                }
            }
        }
    }
}
public class Played : MonoBehaviour
{
    // Token: 0x04000014 RID: 20
    public bool played;
}
[HarmonyPatch(typeof(FinalRank), "Update")]
internal static class Pranked
{
    // Token: 0x06000014 RID: 20 RVA: 0x00002B60 File Offset: 0x00000D60
    [HarmonyPostfix]
    private static void Postfix(FinalRank __instance)
    {
        bool flag = __instance.totalRank.text == "<color=#FFFFFF>P</color>" && __instance.totalRank.gameObject.activeSelf && !__instance.GetComponent<Played>().played;
        if (flag)
        {
            __instance.GetComponent<Played>().played = true;
            GameObject gameObject = new GameObject();
            GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
            gameObject2.AddComponent<AudioSource>();
            gameObject2.GetComponent<AudioSource>().playOnAwake = false;
            gameObject2.GetComponent<AudioSource>().clip = Plugin.Instance.pRank;
            gameObject2.GetComponent<AudioSource>().Play();
        }
        bool flag2 = __instance.totalRank.text == "<color=#FF6A00>A</color>" && __instance.totalRank.gameObject.activeSelf && !__instance.GetComponent<Played>().played;
        if (flag2)
        {
            __instance.GetComponent<Played>().played = true;
            GameObject gameObject3 = new GameObject();
            GameObject gameObject4 = Object.Instantiate<GameObject>(gameObject3, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
            gameObject4.AddComponent<AudioSource>();
            gameObject4.GetComponent<AudioSource>().playOnAwake = false;
            gameObject4.GetComponent<AudioSource>().clip = Plugin.Instance.aRank;
            gameObject4.GetComponent<AudioSource>().Play();
        }
        bool flag3 = __instance.totalRank.text == "<color=#FF0000>S</color>" && __instance.totalRank.gameObject.activeSelf && !__instance.GetComponent<Played>().played;
        if (flag3)
        {
            __instance.GetComponent<Played>().played = true;
            GameObject gameObject5 = new GameObject();
            GameObject gameObject6 = Object.Instantiate<GameObject>(gameObject5, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
            gameObject6.AddComponent<AudioSource>();
            gameObject6.GetComponent<AudioSource>().playOnAwake = false;
            gameObject6.GetComponent<AudioSource>().clip = Plugin.Instance.sRank;
            gameObject6.GetComponent<AudioSource>().Play();
        }
        bool flag4 = __instance.totalRank.text == "<color=#FFD800>B</color>" && __instance.totalRank.gameObject.activeSelf && !__instance.GetComponent<Played>().played;
        if (flag4)
        {
            __instance.GetComponent<Played>().played = true;
            GameObject gameObject7 = new GameObject();
            GameObject gameObject8 = Object.Instantiate<GameObject>(gameObject7, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
            gameObject8.AddComponent<AudioSource>();
            gameObject8.GetComponent<AudioSource>().playOnAwake = false;
            gameObject8.GetComponent<AudioSource>().clip = Plugin.Instance.cRank;
            gameObject8.GetComponent<AudioSource>().Play();
        }
        bool flag5 = __instance.totalRank.text == "<color=#4CFF00>C</color>" && __instance.totalRank.gameObject.activeSelf && !__instance.GetComponent<Played>().played;
        if (flag5)
        {
            __instance.GetComponent<Played>().played = true;
            GameObject gameObject9 = new GameObject();
            GameObject gameObject10 = Object.Instantiate<GameObject>(gameObject9, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
            gameObject10.AddComponent<AudioSource>();
            gameObject10.GetComponent<AudioSource>().playOnAwake = false;
            gameObject10.GetComponent<AudioSource>().clip = Plugin.Instance.cRank;
            gameObject10.GetComponent<AudioSource>().Play();
        }
        bool flag6 = __instance.totalRank.text == "<color=#0094ff>D</color>" && __instance.totalRank.gameObject.activeSelf && !__instance.GetComponent<Played>().played;
        if (flag6)
        {
            __instance.GetComponent<Played>().played = true;
            GameObject gameObject11 = new GameObject();
            GameObject gameObject12 = Object.Instantiate<GameObject>(gameObject11, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity, MonoSingleton<NewMovement>.Instance.transform);
            gameObject12.AddComponent<AudioSource>();
            gameObject12.GetComponent<AudioSource>().playOnAwake = false;
            gameObject12.GetComponent<AudioSource>().clip = Plugin.Instance.dRank;
            gameObject12.GetComponent<AudioSource>().Play();
        }
        bool played = __instance.GetComponent<Played>().played;
        if (played)
        {
            for (int i = 0; i < MonoSingleton<NewMovement>.Instance.transform.childCount; i++)
            {
                bool flag7 = MonoSingleton<NewMovement>.Instance.transform.GetChild(i).name == "preRankPlayer";
                if (flag7)
                {
                    Object.Destroy(MonoSingleton<NewMovement>.Instance.transform.GetChild(i).gameObject);
                    break;
                }
            }
        }
    }
}
[HarmonyPatch(typeof(FinalRank), "Start")]
internal static class PrankedStart
{
    // Token: 0x06000015 RID: 21 RVA: 0x00003028 File Offset: 0x00001228
    [HarmonyPostfix]
    private static void Postfix(FinalRank __instance)
    {
        __instance.gameObject.AddComponent<Played>();
        __instance.GetComponent<Played>().played = false;
        GameObject gameObject = new GameObject();
    }
}
