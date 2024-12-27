using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using ModSettings;
using UnityEngine;

namespace StorageProblem;

public class Settings : JsonModSettings
{

	[Name("Container Slot Limit Ratio")]
	[Description("How many slot can be used in a container per kg limit.")]
	[Slider(0.2f, 10f, 99)]
	public float containerSlotLimitRatio = 0.2f;

	[Name("Min Slots")]
	[Slider(3, 12)]
	public int minSlots = 3;

	[Name("Max Slots")]
	[Slider(6, 30)]
	public int maxSlots = 12;

	[Name("No Problem Mode")]
	[Description("Make it simply converts from kg to slots and applys it.")]
	public bool noProblemMode = false;

	[Name("Base Weight Capacity")]
	[Description("This amount is not used to calculate slot limit. So 7 means only 3kg is used to calculate slot limit for a 10kg container.")]
	[Slider(5, 30)]
	public float baseWeightCapacity = 10f;
}

internal class StorageProblem : MelonMod
{
	internal Settings settings { get; set; }
	internal static StorageProblem Instance { get; private set; }

	public override void OnInitializeMelon()
	{
		Instance = this;
		settings = new Settings();
		settings.AddToModSettings("Storage Problem");
	}
}


[HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.OnInventoryToContainer))]
internal class ContainerSlotLimitPatch
{
	private static bool Prefix(Panel_Container __instance)
	{
		if (__instance.m_Container != null)
		{
			Container container = __instance.m_Container;
			var slotLimit = CalculateContainerSlots(container.m_Capacity.ToQuantity(1f));
			if (container.m_Items.Count >= slotLimit)
			{
				GameAudioManager.PlayGUIError();
				return false;
			}
		}
		return true;
	}

	internal static int CalculateContainerSlots(float capacity)
	{
		capacity -= StorageProblem.Instance.settings.baseWeightCapacity;
		var slotLimit = Mathf.FloorToInt(capacity * StorageProblem.Instance.settings.containerSlotLimitRatio);
		if (!StorageProblem.Instance.settings.noProblemMode)
		{
			slotLimit = StorageProblem.Instance.settings.maxSlots - slotLimit;
			slotLimit = Mathf.Clamp(slotLimit, StorageProblem.Instance.settings.minSlots, StorageProblem.Instance.settings.maxSlots);
		}
		return slotLimit;
	}
}
[HarmonyPatch(typeof(Panel_Container), nameof(Panel_Container.RefreshContainerTable))]
internal class ContainerLabelPatch
{
	private static void Postfix(Panel_Container __instance)
	{
		if (StorageProblem.Instance.settings.containerSlotLimitRatio < 0.1f
		 || __instance.m_Container == null) return;

		Container container = __instance.m_Container;
		var slotLimit = ContainerSlotLimitPatch.CalculateContainerSlots(container.m_Capacity.ToQuantity(1f));
		__instance.m_Label_ContainerCapacity.text += $" ({__instance.m_Container.m_Items.Count}/{slotLimit})";
	}
}