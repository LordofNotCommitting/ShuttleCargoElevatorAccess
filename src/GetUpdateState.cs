using HarmonyLib;
using MGSC;
using System;
using UnityEngine;

namespace ShuttleCargoElevatorAccess
{
    [HarmonyPatch(typeof(MagnumProgression), nameof(MagnumProgression.OnAfterLoad))]
    public class GetUpdateState
    {
        
        static void Postfix(MagnumProgression __instance)
        {
            //Plugin.Logger.Log("dept setup.");
            Plugin.curr_department = __instance.GetDepartment<ShuttleCargoDepartment>();
            //Plugin.Logger.Log("dept setup result." + Plugin.curr_department);
            //Plugin.Logger.Log("dept setup result." + Plugin.curr_department.IsActiveDepartment());
        }
    }
}
