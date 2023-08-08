/*using System;

using HarmonyLib;

using Il2Cpp;

using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New;
using Il2CppAssets.Scripts.Unity.UI_New.Knowledge;
using Il2CppAssets.Scripts.Unity.UI_New.Main;
using Il2CppAssets.Scripts.Unity.UI_New.Main.HeroSelect;
using Il2CppAssets.Scripts.Unity.UI_New.Main.PowersSelect;
using Il2CppAssets.Scripts.Unity.UI_New.Pause;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppAssets.Scripts.Unity.UI_New.Store;
using Il2CppAssets.Scripts.Unity.UI_New.Upgrade;

using Il2CppPlayEveryWare.EpicOnlineServices;

using Il2CppSystem.Threading.Tasks;

using UnityEngine.UI;

using Object = UnityEngine.Object;

namespace BTD6EpicGamesModCompat;

[HarmonyPatch]
public static class Patches {
    private const string DisabledStoreMessage =
        "In order to get mods to run on Epic Games, I had to gut Epic Store functionality.";

    private const string DisabledEpicLoginMessage =
        "In order to get mods to run on Epic Games, I had to gut Epic Login functionality.\nGet a linking code for an account by logging into BTD Battles, BTD6, BTD Monkey City, Bloons Adventure Time TD, or BTD Battles 2 on Steam or a mobile device.";

    [HarmonyPatch(typeof(EOSHostManager), nameof(EOSHostManager.WaitTillReady))]
    [HarmonyPrefix]
    public static bool TryBypassWaitForEOS(out Task __result) {
        __result = Task.CompletedTask;
        return false;
    }

    [HarmonyPatch(typeof(EOSHostManager), nameof(EOSHostManager.Start))]
    [HarmonyPrefix]
    public static bool BypassEOSHostManagerStart() {
        return false;
    }

    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.Awake))]
    [HarmonyPrefix]
    public static bool BypassEOSManagerAwake() {
        return false;
    }

    [HarmonyPatch(typeof(EOSManager), nameof(EOSManager.Update))]
    [HarmonyPrefix]
    public static bool BypassEOSManagerUpdate() {
        return false;
    }

    [HarmonyPatch(typeof(Modding), nameof(Modding.CheckForMods))]
    [HarmonyPrefix]
    public static bool TryBypassModCheck(out bool __result) {
        __result = true;
        return false;
    }

    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
    [HarmonyPostfix]
    public static void DisableStoreButton(MainMenu __instance) {
        var storeBtn = __instance.storeBtn;
        storeBtn.gameObject.GetComponent<Image>().color = storeBtn.colors.disabledColor;
        MainMenu.__c.__9__21_10 = null;
        storeBtn.onClick.RemoveAllListeners();
        storeBtn.onClick.AddListener(new Action(() => { PopupScreen.instance.ShowOkPopup(DisabledStoreMessage); }));
    }

    [HarmonyPatch(typeof(Purchaser), nameof(Purchaser.InitializePurchasing))]
    [HarmonyPrefix]
    public static bool DisablePurchasingStartup() {
        return false;
    }

    [HarmonyPatch(typeof(MainAccountPopup), nameof(MainAccountPopup.Awake))]
    [HarmonyPostfix]
    public static void DisableEpicLogin(MainAccountPopup __instance) {
        var loginEpicBtn = __instance.loginEpicBtn;
        loginEpicBtn.gameObject.GetComponent<Image>().color = loginEpicBtn.colors.disabledColor;
        loginEpicBtn.gameObject.GetComponentInChildren<NK_TextMeshProUGUI>().color = loginEpicBtn.colors.disabledColor;
        loginEpicBtn.onClick.RemoveAllListeners();
        loginEpicBtn.onClick.AddListener(new Action(() => {
            PopupScreen.instance.ShowOkPopup(DisabledEpicLoginMessage);
        }));
    }

    [HarmonyPatch(typeof(PauseScreen), nameof(PauseScreen.Open))]
    [HarmonyPostfix]
    public static void DisablePauseStore(PauseScreen __instance) {
        __instance.storeButton.gameObject.SetActive(false);
    }

    [HarmonyPatch(typeof(UpgradeScreen), nameof(UpgradeScreen.Open))]
    [HarmonyPostfix]
    public static void DisableUpgradePurchases(UpgradeScreen __instance) {
        Object.Destroy(__instance.purchaseAllTowerUpgradesIncludingParagon.gameObject);
        Object.Destroy(__instance.purchaseAllTowerUpgrades.gameObject);
        Object.Destroy(__instance.purchaseParagonTowerUpgrades.gameObject);
        Object.Destroy(__instance.purchaseTowerXP.gameObject);
    }

    [HarmonyPatch(typeof(UpgradeScreen), nameof(UpgradeScreen.SetIapButtons))]
    [HarmonyPrefix]
    public static bool DisableUpgradeIaps() {
        return false;
    }

    [HarmonyPatch(typeof(TowerProductButton), nameof(TowerProductButton.StartPurchase))]
    [HarmonyPrefix]
    public static bool DisableTowerProductButton() {
        return false;
    }

    [HarmonyPatch(typeof(HeroUpgradeDetails), nameof(HeroUpgradeDetails.Awake))]
    [HarmonyPostfix]
    public static void RemoveHeroBundles(HeroUpgradeDetails __instance) {
        Object.Destroy(__instance.heroBundleBtn.transform.parent.gameObject);
    }

    [HarmonyPatch(typeof(CommonForegroundScreen), nameof(CommonForegroundScreen.Show))]
    [HarmonyPostfix]
    public static void HideBuyMMMKButtons(CommonForegroundScreen __instance) {
        __instance.buyMoreMonkeyMoneyButton.SetActive(false);
        __instance.buyKnowledgeButton.SetActive(false);
    }

    [HarmonyPatch(typeof(InstaTowerScreen), nameof(InstaTowerScreen.Awake))]
    [HarmonyPostfix]
    public static void DisableBuyInstasButton(InstaTowerScreen __instance) {
        Object.Destroy(__instance.buyInstasBtn.gameObject);
    }

    [HarmonyPatch(typeof(KnowledgeMain), nameof(KnowledgeMain.Open))]
    [HarmonyPostfix]
    public static void DisableUnlockMKEarlyButton(KnowledgeMain __instance) {
        Object.Destroy(__instance.storeBanner);
    }

    [HarmonyPatch(typeof(PopupScreen), nameof(PopupScreen.ShowMonkeyMonkeyPopup))]
    [HarmonyPrefix]
    public static bool SwitchToNonBuying(PopupScreen __instance, PopupScreen.ReturnCallback onCancel) {
        __instance.ShowOkPopup("Not enough monkey money.");
        return false;
    }
}*/