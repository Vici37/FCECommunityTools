using System;
using System.Collections.Generic;
using System.Linq;

namespace FortressCraft.Community.Utilities
{
	/// <summary>
	///		Extension Methods and Helpers for the ItemBase Class
	/// </summary>
	public static partial class ItemBaseUtil
	{
		/// <summary>
		///		Gets the amount of items and cubes in any type of Enumerable ItemBase
		/// </summary>
		/// <param name="items">The list of items to get the total count from</param>
		/// <returns>The amount of Items and Cubes</returns>
		public static Int32 GetItemCount(this IEnumerable<ItemBase> items)
		{
			return items.Sum(itemBase => itemBase.GetAmount());
		}

		/// <summary>
		///		Gets the amount of items in any type of Enumerable ItemBase
		/// </summary>
		/// <param name="items">The list of items to get the total count from</param>
		/// <param name="itemId">The unique id of the item to count</param>
		/// <returns>The amount of Items</returns>
		public static Int32 GetItemCount(this IEnumerable<ItemBase> items, Int32 itemId)
		{
			return items.Where(item => item.mnItemID == itemId).Sum(item => item.GetAmount());
		}

		/// <summary>
		///		Gets the amount of cubes in any type of Enumerable ItemBase
		/// </summary>
		/// <param name="items"></param>
		/// <param name="cubeId"></param>
		/// <param name="cubeValue"></param>
		/// <returns>The amount of Cubes</returns>
		public static Int32 GetItemCount(this IEnumerable<ItemBase> items, UInt16 cubeId, UInt16 cubeValue)
		{
			return items.Where(item =>
			{
				var cube = item.As<ItemCubeStack>();
				if (cube == null)
					return false;
				return cube.mCubeType == cubeId && cube.mCubeValue == cubeValue;
			}).Sum(item => item.GetAmount());
		}

		/// <summary>
		///		Gets the amount of cubes OR items from any Enumerable ItemBase, based off of an ItemBase
		/// </summary>
		/// <param name="items">The list of items to get the total count from</param>
		/// <param name="restraints">The ItemBase which to restrain the Count to</param>
		/// <returns>The amount of Cubes or Items</returns>
		public static Int32 GetItemCount(this IEnumerable<ItemBase> items, ItemBase restraints)
		{
			if (restraints.mType != ItemType.ItemCubeStack)
				return items.GetItemCount(restraints.mnItemID);

			var cube = restraints.As<ItemCubeStack>();
			return items.GetItemCount(cube.mCubeType, cube.mCubeValue);
		}
	}
}
