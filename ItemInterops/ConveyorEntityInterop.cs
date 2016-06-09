using System;

namespace FortressCraft.Community.ItemInterops
{
	internal class ConveyorEntityInterop : ItemInteropInterface
	{
		public int GetFreeSpace(SegmentEntity entity)
		{
			var conveyor = entity.As<ConveyorEntity>();

			return conveyor.mbReadyToConvey ? 1 : 0;
		}

		public bool GiveItem(SegmentEntity entity, ItemBase item)
		{
			var conveyor = entity.As<ConveyorEntity>();
			if (!this.IsReady(conveyor))
				return false;

			if (item.mType == ItemType.ItemCubeStack)
			{
				var cube = item.As<ItemCubeStack>();
				conveyor.AddCube(cube.mCubeType, cube.mCubeValue, 1);
				return true;
			}

			conveyor.AddItem(item);
			return true;
		}

		public bool HasFreeSpace(SegmentEntity entity, uint amount)
		{
			if (amount > 1)
				return false;
			return GetFreeSpace(entity) >= amount;
		}

		public bool HasItem(SegmentEntity entity, ItemBase item)
		{
			var conveyor = entity.As<ConveyorEntity>();
			return this.HasItem(conveyor, item);
		}

		public bool HasItems(SegmentEntity entity)
		{
			return GetFreeSpace(entity) > 0;
		}

		public bool HasItems(SegmentEntity entity, ItemBase item, out int amount)
		{
			amount = 0;
			var conveyor = entity.As<ConveyorEntity>();
			if (!this.HasItem(conveyor, item))
				return false;

			amount = 1;
			return true;
		}

		public ItemBase TakeAnyItem(SegmentEntity entity)
		{
			var conveyor = entity.As<ConveyorEntity>();
			return this.TakeAnyItem(conveyor);
		}

		public ItemBase TakeItem(SegmentEntity entity, ItemBase item)
		{
			var conveyor = entity.As<ConveyorEntity>();
			if (!this.HasItem(conveyor, item))
				return null;

			return this.TakeAnyItem(conveyor);
		}

		// Private methods to avoid double casts

		private Boolean IsReady(ConveyorEntity conveyor)
		{
			return conveyor.mbReadyToConvey;
		}

		private Boolean HasItem(ConveyorEntity conveyor, ItemBase item)
		{
			if (this.IsReady(conveyor))
				return false;

			if (item.mType != ItemType.ItemCubeStack)
				return item.CompareDeep(conveyor.mCarriedItem);

			ItemCubeStack cube = item.As<ItemCubeStack>();
			return conveyor.mCarriedCube == cube.mCubeType && conveyor.mCarriedValue == cube.mCubeValue;
		}

		private ItemBase TakeAnyItem(ConveyorEntity conveyor)
		{
			if (this.IsReady(conveyor))
				return null;

			ItemBase returnItem = conveyor.mCarriedCube != 0
				? ItemManager.SpawnCubeStack(conveyor.mCarriedCube, conveyor.mCarriedValue, 1)
				: conveyor.mCarriedItem;

			if (returnItem == null)
				return null;

			conveyor.RemoveCube();
			conveyor.RemoveItem();
			conveyor.FinaliseOffloadingCargo();
			return returnItem;
		}
	}
}
