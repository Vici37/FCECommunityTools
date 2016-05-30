using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FortressCraft.Community.ItemInterops
{
	// Internal class, to ensure that only we, the community library, use it.
	internal class StorageHopperInterop : ItemInteropInterface
	{
		public Boolean HasItems(SegmentEntity entity)
		{
			var hopper = entity.As<StorageHopper>();

			return hopper?.mnStorageUsed > 0;
		}

		public Boolean HasItem(SegmentEntity entity, ItemBase item)
		{
			Int32 amount;
			return this.HasItems(entity, item, out amount);
		}

		public Boolean HasItems(SegmentEntity entity, ItemBase item, out Int32 amount)
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

		public Boolean HasCapcity(SegmentEntity entity, UInt32 amount)
		{
			var hopper = entity.As<StorageHopper>();

			return hopper.mnStorageFree >= amount;
		}

		public Int32 GetCapcity(SegmentEntity entity)
		{
			var hopper = entity.As<StorageHopper>();

			return hopper.mnStorageFree;
		}

		public Boolean GiveItem(SegmentEntity entity, ItemBase item)
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

		public ItemBase TakeItem(SegmentEntity entity, ItemBase item)
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

		public ItemBase TakeAnyItem(SegmentEntity entity)
		{
			throw new NotImplementedException(); // Undecided.
		}
	}
}
