namespace FortressCraft.Community.Window
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

	public static class ModWindowManager
	{
		private static readonly GenericMachineManager _manager;
		private static Dictionary<UInt16, ModMachineWindow> _modWindows;

		static ModWindowManager()
		{
			// Credits to @BinaryAlgorithm for this line
			_manager = typeof (GenericMachineManager).GetField("manager", BindingFlags.NonPublic | BindingFlags.Instance)?
				.GetValue(GenericMachinePanelScript.instance) as GenericMachineManager;

			if (_manager == null)
				throw new Exception("ModWindowManager was unable to obtain an instance of the GenericMachineManager");

			_manager.windows.Add(eSegmentEntity.Mod, new ModWindow());
			_modWindows = new Dictionary<UInt16, ModMachineWindow>();
		}

		public static void RegisterWindow(UInt16 cubeType, ModMachineWindow window)
		{
			window.SetManager(_manager);
			_modWindows.Add(cubeType, window);
		}

		internal static void DisplayWindow(UInt16 cubeType, SegmentEntity caller)
		{
			if (_modWindows.ContainsKey(cubeType))
				_modWindows[cubeType].SpawnWindow(caller);
		}

		internal class ModWindow : BaseMachineWindow
		{
			public override void SpawnWindow(SegmentEntity targetEntity)
			{
				var machine = targetEntity as MachineEntity;
				if (machine == null)
					throw new ArgumentException(nameof(targetEntity), "Unable to convert the provided entity into a MachineEntity, to find the CubeType ;(");

				DisplayWindow(machine.mCube, targetEntity);
			}
		}
	}
}
