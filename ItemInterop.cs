using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace FortressCraft.Community
{
	/// <summary>
	///		A class that provides Interopability between Storage and Machines for Pre-StorageInterface mods
	/// </summary>
	public static class ItemInterop
	{

		private static readonly ReadOnlyCollection<Type> _supportedTypes;

		static ItemInterop()
		{
			_supportedTypes = new List<Type>
			{
				typeof(StorageHopper),
				typeof(CommunityItemInterface),
                typeof(ConveyorEntity)
			}.AsReadOnly();
		}
		
		private static Type SupportedType(SegmentEntity entity)
		{
			if (_supportedTypes.Contains(entity.GetType()))
				return entity.GetType();

			return entity is CommunityItemInterface ? typeof (CommunityItemInterface) : null;
		}

        public static Boolean HasItem(SegmentEntity entity) 
        {
            return HasItem(entity, null);
        }

		///  <summary>
		/// 		Checks to see if a Storage Medium has the specified item.
		///  
		/// 		Checks for Ore/Item/Cube
		///  </summary>
		///  <param name="entity">The entity to search within</param>
		///  <param name="item">The Item to Check For</param>
		///  <returns>True if the storage medium has the specified item, false otherwise</returns>
		public static Boolean HasItem(SegmentEntity entity, ItemBase item)
		{
			Int32 amount;
			return HasItems(entity, item, out amount) && amount > 0;
		}

		///  <summary>
		/// 		Checks to see if a Storage Medium has the specified item.
		///  
		/// 		Checks for Ore/Item/Cube
		///  </summary>
		///  <param name="entity">The entity to search within</param>
		///  <param name="item">The Item to Check For</param>
		///  <param name="amount">Stores the amount of the items available</param>
		///  <returns>True if the storage medium has the specified item, false otherwise</returns>
		public static Boolean HasItems(SegmentEntity entity, ItemBase item, out Int32 amount)
		{
			amount = 0;
			var type = SupportedType(entity);
			if (type == null)
				return false;

			if (type == typeof (CommunityItemInterface))
			{
				var newEntity = entity as CommunityItemInterface;

                if (item == null) 
                {
                    // make amount = 1 so that we pass the bool check in HasItem(entity, item)
                    amount = 1;
                    return newEntity.HasItem();
                }

				return newEntity.HasItems(item, out amount);
			}

			var isCube = item.mType == ItemType.ItemCubeStack;
			ItemCubeStack cube = null;
			if (isCube)
				cube = item.As<ItemCubeStack>();


			if (type == typeof (StorageHopper))
			{
				var newEntity = entity.As<StorageHopper>();

                if (item == null) 
                {
                    amount = newEntity.mnStorageUsed;
                    return amount > 0;
                }

				if (!isCube)
				{
					amount = newEntity.CountHowManyOfItem(item.mnItemID);
					return amount > 0;
				}

				if (CubeHelper.IsOre(cube.mCubeType))
				{
					amount = newEntity.CountHowManyOfOreType(cube.mCubeType);
					return amount > 0;
				}

				amount = newEntity.CountHowManyOfType(cube.mCubeType, cube.mCubeValue);
				return amount > 0;
			}

            if (type == typeof (ConveyorEntity)) 
            {
                var newEntity = entity.As<ConveyorEntity>();

                if (item == null && (newEntity.mCarriedCube != 0 || newEntity.mCarriedItem != null)) 
                {
                    // make amount = 1 so we pass the amount > 1 check in HasItem(entity, item)
                    amount = 1;
                    return true;
                }

                if (!isCube) 
                {
                    if(newEntity.mCarriedCube == cube.mCubeType && newEntity.mCarriedValue == cube.mCubeValue) {
                        amount = 1;
                        return true;
                    }
                }

                if (item.Compare(newEntity.mCarriedItem)) 
                {
                    amount = 1;
                    return true;
                }
            }
			
			return false;
		}

		// TODO: XML Doc
		public static Boolean HasCapacity(SegmentEntity entity, UInt32 amount)
		{
			var type = SupportedType(entity);
			if (type == null)
				return false;

			if (type == typeof(CommunityItemInterface))
			{
				var newEntity = entity as CommunityItemInterface;
				return newEntity.HasCapcity(amount);
			}

			// Experimental
			if (type == typeof (StorageHopper))
			{
				var newEntity = entity.As<StorageHopper>();
				return newEntity.mnStorageFree >= amount;
			}

            if (type == typeof (ConveyorEntity) && amount == 1) 
            {
                var newEntity = entity.As<ConveyorEntity>();
                return newEntity.mbReadyToConvey && newEntity.mrLockTimer == 0f;
            }

			return false;
		}

		// TODO: XML Doc
		public static Boolean AddItem(SegmentEntity entity, ItemBase item)
		{
			var type = SupportedType(entity);
			if (type == null)
				return false;

			if (type == typeof(CommunityItemInterface))
			{
				var newEntity = entity as CommunityItemInterface;
				return newEntity.HasItem(item);
			}

			var isCube = item.mType == ItemType.ItemCubeStack;
			ItemCubeStack cube = null;
			if (isCube)
				cube = item.As<ItemCubeStack>();

			// Experimental
			if (type == typeof (StorageHopper))
			{
				var newEntity = entity.As<StorageHopper>();

				if (!isCube)
					return newEntity.AddItem(item);

				var currentStorage = newEntity.mnStorageFree;
				newEntity.AddCube(cube.mCubeType, cube.mCubeValue);
				return currentStorage != newEntity.mnStorageFree;
			}

            if(type == typeof (ConveyorEntity)) 
            {
                var newEntity = entity.As<ConveyorEntity>();
                if (newEntity.mbReadyToConvey) 
                {
                    if (isCube) 
                    {
                        newEntity.AddCube(cube.mCubeType, cube.mCubeValue, 1);
                        return true;
                    }

                    newEntity.AddItem(item);
                    return true;
                }
            }

			return false;
		}

		// TODO: XML Doc
		public static ItemBase TakeItem(SegmentEntity entity, ItemBase item)
		{
			var type = SupportedType(entity);
			if (type == null)
				return null;

			if (type == typeof (CommunityItemInterface))
			{
				var newEntity = entity as CommunityItemInterface;
				return newEntity.TakeItem(item);
			}

			var isCube = item.mType == ItemType.ItemCubeStack;
			ItemCubeStack cube = null;
			if (isCube)
				cube = item.As<ItemCubeStack>();

			if (type == typeof (StorageHopper))
			{
				var newEntity = entity.As<StorageHopper>();

				if (!isCube)
					return newEntity.RemoveSingleSpecificItemByID(item.mnItemID);
				return newEntity.RemoveSingleSpecificCubeStack(cube);
			}

            // Conveyor Entities will return whatever item they're carrying, regardless of the item passed in
            if (type == typeof (ConveyorEntity)) 
            {
                var newEntity = entity.As<ConveyorEntity>();
                if (!newEntity.mbReadyToConvey && newEntity.mrLockTimer == 0f) 
                {
                    ItemBase ret = null;
                    if (newEntity.mCarriedItem == null) 
                    {
                        ret = new ItemCubeStack(newEntity.mCarriedCube, newEntity.mCarriedValue, 1);
                    }
                    else 
                    {
                        ret = newEntity.mCarriedItem;
                    }
                    newEntity.RemoveCube();
                    newEntity.RemoveItem();
                    newEntity.FinaliseOffloadingCargo();
                    return ret;
                }
            }

			return null;
		}

		// TODO: XML Doc
		public static ItemBase TakeItem(SegmentEntity entity, UInt16 type, UInt16 value)
		{
			return TakeItem(entity, new ItemCubeStack(type, value, 1));
		}

	}
}
