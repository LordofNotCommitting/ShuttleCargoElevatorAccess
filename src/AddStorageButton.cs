using HarmonyLib;
using MGSC;
using System;
using UnityEngine;

namespace ShuttleCargoElevatorAccess
{
    [HarmonyPatch(typeof(ElevatorWindow), nameof(ElevatorWindow.Configure))]
    public class AddStorageButtonPatch
    {
        public static CommonButton storageButton;

        static void Prefix(ElevatorWindow __instance)
        {
            if (storageButton == null)
            {
                storageButton = UnityEngine.Object.Instantiate(__instance._missionExitButton, __instance._missionExitButton.transform.parent.transform);
                storageButton.OnClick += ShuttleCargoButtonClick;
                storageButton.name = "ShipCargoButton";
                storageButton.gameObject.SetActive(true);

                Transform captionTransform = storageButton.transform.Find("Caption");
                if (captionTransform != null)
                {
                    LocalizableLabel locLabel = captionTransform.GetComponent<LocalizableLabel>();
                    if (locLabel != null)
                    {
                        locLabel._label = "ui.magnum.shuttleCargo";
                    }
                }
                storageButton.SetInteractable(true);
            }
        }
        private static void ShuttleCargoButtonClick(CommonButton button, int arg2)
        {
            UI.Hide<ElevatorWindow>();
            //Shuttle shuttle_instance = UnityEngine.Object.FindObjectOfType<Shuttle>();

            //shuttle_instance.InteractionInventoryType = EShuttleInteractionInventoryType.ShuttleCargo;
            UI.Chain<InventoryScreen>().Show(true).Invoke(delegate (InventoryScreen v)
            {
                //v.ConfigureTabs(shuttle_instance._mapObstacle);
                ItemOnFloor itemOnFloor = v._itemsOnFloor.Get(v._creatures.Player.CreatureData.Position);
                v._lastInteractObstacle = null;
                ShuttleCargoDepartment department = v._magnumSpaceship.GetDepartment<ShuttleCargoDepartment>();
                if (department != null)
                {
                    v._tabsView.AddTab(v._shuttleCargoView, department.ShuttleCargo, TabType.Nymeric);
                    v._shuttleCargoView.AddCaptionByTab(department.ShuttleCargo, Localization.Get("ui.magnum.shuttleCargo"));
                }

                if (itemOnFloor != null && !itemOnFloor.Storage.Empty)
                {
                    v._tabsView.AddTab(v._itemsOnFloorView, itemOnFloor.Storage, TabType.Nymeric);
                }
                v._hideAfterItemsOnFloorLooted = (v._tabsView.TabsCount > 0);
                v._tabsView.SelectAndShowFirstTab();

            }).SetBackOnBackgroundClick(true);
        }

        /*
        static void Postfix(ref ElevatorWindow __result)
        {

            __result._missionExitButton.OnClick += ShuttleCargoButtonClick;
            
        }
        */


        /*
        static Action<CommonButton, int> Postfix(ref ElevatorWindow __result, CommonButton arg1, int arg2)
        {
            return (button, number) => {
                Shuttle shuttle_instance = UnityEngine.Object.FindObjectOfType<Shuttle>();
                if (shuttle_instance != null)
                {
                    shuttle_instance.InteractionInventoryType = EShuttleInteractionInventoryType.ShuttleCargo;
                    UI.Chain<InventoryScreen>().Show(true).Invoke(delegate (InventoryScreen v)
                    {
                        v.ConfigureTabs(shuttle_instance._mapObstacle);
                    }).SetBackOnBackgroundClick(true);
                }
            };
        }
        */
    }
}
