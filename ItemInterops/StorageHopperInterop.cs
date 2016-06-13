using System;
using FortressCraft.Community.Utilities;

namespace FortressCraft.Community.ItemInterops
{
	// Internal class, to ensure that only we, the community library, use it.
	internal class StorageHopperInterop : ItemInteropInterface
	{
		public Boolean HasItems(SegmentEntity caller, SegmentEntity entity)
		{
			var hopper = entity.As<StorageHopper>();

			return hopper?.mnStorageUsed > 0;
		}

		public Boolean HasItem(SegmentEntity caller, SegmentEntity entity, ItemBase item)
		{
			Int32 amount;
			return this.HasItems(caller, entity, item, out amount);
		}

		public Boolean HasItems(SegmentEntity caller, SegmentEntity entity, ItemBase item, out Int32 amount)
		{
			var hopper = entity.As<StorageHopper>();

			var isCube = item.mType == ItemType.ItemCubeStack;
			ItemCubeStack cube = null;
			if (isCube)
				cube = item.As<ItemCubeStack>();

			amount = !isCube ? hopper.CountHowManyOfItem(item.mnItemID) 
							 : hopper.CountHowManyOfType(cube.mCubeType, cube.mCubeValue);

			return amount > 0;
		}

		public Boolean HasFreeSpace(SegmentEntity caller, SegmentEntity entity, UInt32 amount)
		{
			var hopper = entity.As<StorageHopper>();

			return hopper.mnStorageFree >= amount;
		}

		public Int32 GetFreeSpace(SegmentEntity caller, SegmentEntity entity)
		{
			var hopper = entity.As<StorageHopper>();

			return hopper.mnStorageFree;
		}

		public Boolean GiveItem(SegmentEntity caller, SegmentEntity entity, ItemBase item)
		{
			var hopper = entity.As<StorageHopper>();

			var isCube = item.mType == ItemType.ItemCubeStack;
			ItemCubeStack cube = null;
			if (isCube)
				cube = item.As<ItemCubeStack>();

			if (!isCube)
				return hopper.AddItem(item);
			
			// Might be possible to replace with ^ as universal for Item/Cubes
			var currentStorage = hopper.mnStorageFree;
			hopper.AddCube(cube.mCubeType, cube.mCubeValue);
			return currentStorage != hopper.mnStorageFree;
		}

		public ItemBase TakeItem(SegmentEntity caller, SegmentEntity entity, ItemBase item)
		{
			var hopper = entity.As<StorageHopper>();

			var isCube = item.mType == ItemType.ItemCubeStack;
			ItemCubeStack cube = null;
			if (isCube)
				cube = item.As<ItemCubeStack>();

			if (!isCube)
				return hopper.RemoveSingleSpecificItemByID(item.mnItemID);
			return hopper.RemoveSingleSpecificCubeStack(cube);
		}

		public ItemBase TakeAnyItem(SegmentEntity caller, SegmentEntity entity)
		{
			var hopper = entity.As<StorageHopper>();
			if (hopper.mPermissions == StorageHopper.ePermissions.Locked || !hopper.mbAllowLogistics) {
				return null;
			}
			ItemBase ret = null;
			if (hopper.mnStorageUsed >= 1) {
				ushort cubeType;
				ushort cubeValue;
				hopper.GetSpecificCubeRoundRobin(StorageHopper.eRequestType.eAny, out cubeType, out cubeValue);
				if (cubeType != 0) {
					ret = ItemManager.SpawnCubeStack(cubeType, cubeValue, 1);
				} else {
					ret = hopper.RemoveSingleSpecificItemOrCubeRoundRobin(StorageHopper.eRequestType.eAny);
				}
			}
			
			if(ret != null) {
				hopper.LogisticsOperation();
				hopper.RequestImmediateNetworkUpdate();
			}
			return ret;
		}
	}
}
