using HarmonyLib;
using MGSC;
using System;

namespace ShuttleCargoElevatorAccess
{
    [HarmonyPatch(typeof(ElevatorWindow), nameof(ElevatorWindow.CloseButtonOnClick))]
    public class OverrideCloseButton
    {
        /*
        static void Postfix(ref ElevatorWindow __result)
        {

            __result._missionExitButton.OnClick += ShuttleCargoButtonClick;
            
        }
        */
        private static void ShuttleCargoButtonClick(CommonButton button, int arg2)
        {
            throw new NotImplementedException();
        }

        static void Postfix(ElevatorWindow __instance, CommonButton arg1, int arg2)
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
