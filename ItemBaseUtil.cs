namespace FortressCraft.Community
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class ItemBaseUtil
	{
		/// <summary>
		///     Compares all possible
		/// </summary>
		/// <param name="original"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public static Boolean CompareDeep(this ItemBase original, ItemBase comparer)
		{
			return original.Compare(comparer) && (
				   (original as ItemCubeStack).Compare(comparer as ItemCubeStack) ||
				   (original as ItemDurability).Compare(comparer as ItemDurability) ||
				   (original as ItemStack).Compare(comparer as ItemStack) ||
				   (original as ItemSingle).Compare(comparer as ItemSingle) ||
				   (original as ItemCharge).Compare(comparer as ItemCharge) ||
				   (original as ItemLocation).Compare(comparer as ItemLocation)
				);
		}

		/// <summary>
		///     Compare the ItemID and Type values of ItemBase
		/// </summary>
		/// <param name="original">Original Item</param>
		/// <param name="comparer">Item to Compare Against</param>
		/// <returns>True if ItemID and Type of both Items match, otherwise false</returns>
		public static bool Compare(this ItemBase original, ItemBase comparer)
		{
			return original.mnItemID == comparer.mnItemID && original.mType == comparer.mType;
		}

		/// <summary>
		///     Compares the ItemID, Type, CubeType, and CubeValue values of ItemCubeStack
		///     Does not compare the Amounts
		/// </summary>
		/// <param name="original">Original Item</param>
		/// <param name="comparer">Item to Compare Against</param>
		/// <returns>True if ItemID, Type, CubeType, CubeValue of both Items match, otherwise false</returns>
		public static bool Compare(this ItemCubeStack original, ItemCubeStack comparer)
		{
			// We'll ignore the original stacks for now. May revisit in the future
			return original != null && comparer != null && original.Compare((ItemBase)comparer) &&
				   original.mCubeType == comparer.mCubeType && original.mCubeValue == comparer.mCubeValue;
			// && original.mnAmount == comparer.mnAmount;
		}

		/// <summary>
		///     Compares the ItemID, Type, CurrentDurability, and MaxDurability values of ItemDurability
		/// </summary>
		/// <param name="original">Original Item</param>
		/// <param name="comparer">Item to Compare Against</param>
		/// <returns>True if ItemID, Type, CurrentDurability, and MaxDurability of both Items match, otherwise false</returns>
		public static bool Compare(this ItemDurability original, ItemDurability comparer)
		{
			return original != null && comparer != null && original.Compare((ItemBase) comparer) &&
				   original.mnCurrentDurability == comparer.mnCurrentDurability &&
				   original.mnMaxDurability == comparer.mnMaxDurability;
		}

		/// <summary>
		///     Compares the ItemID and Type values of ItemStack
		/// </summary>
		/// <param name="original">Original Item</param>
		/// <param name="comparer">Item to Compare Against</param>
		/// <returns>True if ItemID and Type of both Items match, otherwise false</returns>
		public static bool Compare(this ItemStack original, ItemStack comparer)
		{
			// Again, we'll ignore the size of the stack for now
			return original != null && comparer != null && original.Compare((ItemBase)comparer); // && original.mnAmount == comparer.mnAmount;
		}

		/// <summary>
		///     Compares ItemID and Type values of ItemSingle
		/// </summary>
		/// <param name="original">Original Item</param>
		/// <param name="comparer">Item to Compare Against</param>
		/// <returns>True if ItemID and Type of both Items match, otherwise false</returns>
		public static bool Compare(this ItemSingle original, ItemSingle comparer)
		{
			return original != null && comparer != null && original.Compare((ItemBase)comparer);
		}

		/// <summary>
		///     Compares ItemID, Type, and ChargeLevel of ItemCharge
		/// </summary>
		/// <param name="original">Original Item</param>
		/// <param name="comparer">Item to Compare Against</param>
		/// <returns>True if ItemID, Type and ChargeLevel of both Items match, otherwise false</returns>
		public static bool Compare(this ItemCharge original, ItemCharge comparer)
		{
			return original != null && comparer != null && original.Compare((ItemBase)comparer) &&
				   FloatTolerance(original.mChargeLevel, comparer.mChargeLevel, 0.1f);
		}

		/// <summary>
		///     Compares ItemID, Type, LocationX, LocationY, LocationZ, LookVectorX, LookVectorY amd LookVectoryZ of ItemLocation
		/// </summary>
		/// <param name="original">Original Item</param>
		/// <param name="comparer">Item to Compare Against</param>
		/// <returns>
		///     True if ItemID, Type, LocationX, LocationY, LocationZ, LookVectorX, LookVectorY amd LookVectoryZ of both Items
		///     match, otherwise false
		/// </returns>
		public static bool Compare(this ItemLocation original, ItemLocation comparer)
		{
			return original != null &&
				   comparer != null &&
				   original.Compare((ItemBase)comparer) &&
				   original.mLocX == comparer.mLocX &&
				   original.mLocY == comparer.mLocY &&
				   original.mLocZ == comparer.mLocZ &&
				   FloatTolerance(original.mLookVector.x, comparer.mLookVector.x, 0.1f) &&
				   FloatTolerance(original.mLookVector.y, comparer.mLookVector.y, 0.1f) &&
				   FloatTolerance(original.mLookVector.z, comparer.mLookVector.z, 0.1f);
			//				original.mLookVector.x == comparer.mLookVector.x &&
			//				original.mLookVector.y == comparer.mLookVector.y && 
			//				original.mLookVector.z == comparer.mLookVector.z;
		}

		/// <summary>
		///     Is the Item original Stack Type
		/// </summary>
		/// <param name="item">The Item</param>
		/// <returns>True if Item is original Stack, otherwise False</returns>
		public static Boolean IsStack(this ItemBase item)
		{
			return item.mType == ItemType.ItemCubeStack || item.mType == ItemType.ItemStack;
		}

		/// <summary>
		///     Compares two Items to check if they are both stacks and identical
		/// </summary>
		/// <param name="item">The original Item</param>
		/// <param name="comparer">The item to compare against</param>
		/// <returns>True if both Items are stacks and the same</returns>
		public static Boolean IsStackAndSame(this ItemBase item, ItemBase comparer)
		{
			if (!item.IsStack() || !comparer.IsStack())
				return false;
			return (item as ItemCubeStack).Compare(comparer as ItemCubeStack) ||
			       (item as ItemStack).Compare(comparer as ItemStack);
		}

		/// <summary>
		///     Increases the Stack Size by the specified amount
		/// </summary>
		/// <param name="item">The Item Stack</param>
		/// <param name="amount">The amount to increment by (Default: 1)</param>
		public static void IncrementStack(this ItemBase item, Int32 amount = 1)
		{
			if (!item.IsStack())
				return;
			if (item.mType == ItemType.ItemCubeStack)
				(item as ItemCubeStack).mnAmount += amount;
			if (item.mType == ItemType.ItemStack)
				(item as ItemStack).mnAmount += amount;
		}

		/// <summary>
		///     Decrement the Stack Size by the specified amount
		/// </summary>
		/// <param name="item">The Item Stack</param>
		/// <param name="amount">The amount to increment by (Default: 1)</param>
		public static void DecrementStack(this ItemBase item, Int32 amount = 1)
		{
			if (!item.IsStack())
				return;
			if (item.mType == ItemType.ItemCubeStack)
				(item as ItemCubeStack).mnAmount -= amount;
			if (item.mType == ItemType.ItemStack)
				(item as ItemStack).mnAmount -= amount;
		}

		/// <summary>
		///     Set the amount of items in original Item Stack
		/// </summary>
		/// <param name="item">The Item Stack</param>
		/// <param name="amount">The amount of Items</param>
		public static void SetAmount(this ItemBase item, Int32 amount)
		{
			if (!item.IsStack())
				return;
			if (item.mType == ItemType.ItemCubeStack)
				(item as ItemCubeStack).mnAmount = amount;
			if (item.mType == ItemType.ItemStack)
				(item as ItemStack).mnAmount = amount;
		}

		/// <summary>
		///     Creates original new instance of the ItemBase type
		/// </summary>
		/// <param name="item">The original to create original new instance of</param>
		/// <returns>The new original, or null if unknown type</returns>
		public static ItemBase NewInstance(this ItemBase item)
		{
			switch (item.mType)
			{
				case ItemType.ItemCubeStack:
					var ics = item as ItemCubeStack;
					return new ItemCubeStack(ics.mCubeType, ics.mCubeValue, ics.mnAmount);
				case ItemType.ItemStack:
					var its = item as ItemStack;
					return new ItemStack(its.mnItemID, its.mnAmount);
				case ItemType.ItemCharge:
					var ic = item as ItemCharge;
					return new ItemCharge(ic.mnItemID, (int)ic.mChargeLevel);
				case ItemType.ItemDurability:
					var id = item as ItemDurability;
					return new ItemDurability(id.mnItemID, id.mnCurrentDurability, id.mnMaxDurability);
				case ItemType.ItemLocation:
					var il = item as ItemLocation;
					return new ItemLocation(il.mnItemID, il.mLocX, il.mLocY, il.mLocZ, il.mLookVector);
				case ItemType.ItemSingle:
					return new ItemSingle(item.mnItemID);
				default:
					return null;
			}
		}

		/// <summary>
		///     Gets the amount of items in an ItemBase, while being Stack friendly
		/// </summary>
		/// <param name="item">The original</param>
		/// <returns>The amount of items</returns>
		public static Int32 GetAmount(this ItemBase item)
		{
			switch (item.mType)
			{
				case ItemType.ItemCubeStack:
					return (item as ItemCubeStack).mnAmount; // If it is original type of ItemCubeStack, it shouldn't be null
				case ItemType.ItemStack:
					return (item as ItemStack).mnAmount; // Read above note
				default:
					return 1; // All other original types can only have 1 original
			}
		}

		/// <summary>
		///     Gets the total count of items in an Enumerable of ItemBase
		/// </summary>
		/// <param name="items">The list of items to get the total count for</param>
		/// <returns>Returns the total count of items</returns>
		public static Int32 GetItemCount(this IEnumerable<ItemBase> items)
		{
			return items.Sum(itemBase => itemBase.GetAmount()); // Linq~
		}

		/// <summary>
		///     A Float Equals method with tolerance.
		///     Based off of: https://msdn.microsoft.com/en-us/library/ya2zha7s.aspx
		/// </summary>
		private static Boolean FloatTolerance(float f1, float f2, float tolerance)
		{
			var diff = Math.Abs(f1 * tolerance);
			return Math.Abs(f1 - f2) <= diff;
		}
	}

}
