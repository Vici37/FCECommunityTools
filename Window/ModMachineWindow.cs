namespace FortressCraft.Community.Window
{
	public abstract class ModMachineWindow : BaseMachineWindow
	{
		internal void SetManager(GenericMachineManager manager)
		{
			this.manager = manager;
		}
	}
}
