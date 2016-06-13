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


        /// <summary>
        ///     Adds an ItemBase to a list, consolidating stacks - obeys list storage capacity by returning excess items
        /// </summary>
        /// <param name="item">The item to add to the list</param>
        /// <param name="targetlist">The list to transfer it to</param>
        /// <param name="returnpartialstack">If true returns partial stack when insufficient stack size found</param>
        /// <param name="storagecapacity">The list's storage capacity</param>
        /// <returns>The remaining items that do not fit in the list</returns>
        public static ItemBase AddListItem(this ItemBase item, IEnumerable<ItemBase> targetlist, bool returnpartialstack, int storagecapacity = int.MaxValue)
        {
            List<ItemBase> workingtarget = targetlist.ToList();
            int remainder = 0;
            int listcount = workingtarget.GetItemCount();
            int itemcount = item.GetAmount();
            int newtotal = listcount + itemcount;
            ItemBase returnitem = null;
            if (newtotal > storagecapacity)
            {
                remainder = newtotal - storagecapacity;
                itemcount -= remainder;
                item.SetAmount(itemcount);
                returnitem = NewInstance(item);
                returnitem.SetAmount(remainder);
            }
            for (int index = 0; index < workingtarget.Count; index++)
            {
                if (workingtarget[index].IsStackAndSame(item))
                {
                    workingtarget[index].IncrementStack(itemcount);
                    break;
                }
                else
                {
                    workingtarget.Add(item);
                    break;
                }
            }
            targetlist = workingtarget;
            return returnitem;
        }

        /// <summary>
        ///     Deduct an item from an item list by example
        /// </summary>
        /// <param name="item">The example item to attempt to remove from the list</param>
        /// <param name="sourcelist">The list to take the item from</param>
        /// <param name="returnpartialstack">If true returns partial stack when insufficient stack size found</param>
        /// <returns>The ItemBase object or null if unavailable</returns>
        public static ItemBase RemoveListItem(this ItemBase item, IEnumerable<ItemBase> sourcelist, bool returnpartialstack)
        {
            List<ItemBase> workingsource = sourcelist.ToList();
            if (item.IsStack())
            {
                int itemcount = item.GetAmount();
                int listitemcount;

                for (int index = 0; index < workingsource.Count; index++)
                {
                    ItemBase i = workingsource[index];
                    listitemcount = i.GetAmount();
                    if (i.Compare(item) && listitemcount > itemcount)
                    {
                        i.DecrementStack(itemcount);
                        sourcelist = workingsource;
                        return item;
                    }
                    else if (i.Compare(item) && listitemcount == itemcount)
                    {
                        workingsource.Remove(i);
                        sourcelist = workingsource;
                        return item;
                    }
                    else if (returnpartialstack)
                    {
                        item.SetAmount(listitemcount);
                        sourcelist = workingsource;
                        return item;
                    }
                }
            }
            else
                for (int index = 0; index < workingsource.Count; index++)
                {
                    ItemBase i = workingsource[index];
                    if (i.Compare(item))
                    {
                        workingsource.Remove(i);
                        sourcelist = workingsource;
                        return i;
                    }
                }
            sourcelist = workingsource;
            return null;
        }

        /// <summary>
        ///     Moves specified amount of any items from one list to another obeying target storage capacity.
        /// </summary>
        /// <param name="sourcelist">Source list of items</param>
        /// <param name="targetlist">Target list of items</param>
        /// <param name="amount">Quantity of items to move</param>
        /// <param name="StorageCapacity">Storage capacity of target item list</param>
        /// <param name="takefirstitem">Moves only the first item found</param>
        /// <param name="whiteblacklist">A list of items to server as a whitelist or blacklist for transferring</param>
        /// <param name="iswhitelist">True if whitelist otherwise treat as a blacklist</param>
        public static void MoveItems(IEnumerable<ItemBase> sourcelist, IEnumerable<ItemBase> targetlist, int amount = 1, int StorageCapacity = int.MaxValue, bool takefirstitem = false, IEnumerable<ItemBase> whiteblacklist = null, bool iswhitelist = true)
        {
            int listcount;
            int freespace;
            List<ItemBase> workingsource = sourcelist.ToList();
            List<ItemBase> workingtarget = targetlist.ToList();
            List<ItemBase> filter = new List<ItemBase>();
            if (whiteblacklist != null)
                filter = whiteblacklist.ToList();
            int filtercount = filter.Count;
            bool matchfound = false;

            if (amount <= 0)
            {
                return;
            }
            for (int index = 0; index < workingsource.Count; index++)
            {
                listcount = workingsource[index].GetAmount();
                freespace = StorageCapacity - workingtarget.GetItemCount();
                ItemBase i = workingsource[index];

                if (filtercount != 0)
                {
                    for (int index2 = 0; index2 < filtercount; index2++) 
                    {
                        ItemBase j = filter[index2];
                        matchfound = (i.Compare(j));
                        if (matchfound)
                            break;
                    }
                    //XOR to skip the continue otherwise white/black list violation so go to next item
                    if (matchfound ^ iswhitelist)
                        continue;
                }

                if (i.IsStack())
                {
                    if (listcount > amount)
                    {
                        if (amount > freespace)
                        {
                            AddListItem(NewInstance(i).SetAmount(freespace), workingtarget, false);
                            i.DecrementStack(freespace);
                            sourcelist = workingsource;
                            targetlist = workingtarget;
                            return;
                        }
                        else
                        {
                            AddListItem(NewInstance(i).SetAmount(amount), workingtarget, false);
                            i.DecrementStack(amount);
                            sourcelist = workingsource;
                            targetlist = workingtarget;
                            return;
                        }
                    }
                    else if (listcount == amount)
                    {
                        if (amount > freespace)
                        {
                            AddListItem(NewInstance(i).SetAmount(freespace), workingtarget, false);
                            workingsource.Remove(i);
                            sourcelist = workingsource;
                            targetlist = workingtarget;
                            return;
                        }
                        else
                        {
                            AddListItem(NewInstance(i).SetAmount(amount), workingtarget, false);
                            workingsource.Remove(i);
                            sourcelist = workingsource;
                            targetlist = workingtarget;
                            return;
                        }
                    }
                    else
                    {
                        if (listcount > freespace)
                        {
                            AddListItem(NewInstance(i).SetAmount(freespace), workingtarget, false);
                            workingsource.Remove(i);
                            amount -= listcount;
                        }
                        else
                        {
                            AddListItem(NewInstance(i).SetAmount(listcount), workingtarget, false);
                            workingsource.Remove(i);
                            amount -= listcount;
                        }
                    }
                }
                else
                {
                    if (freespace > 0)
                    {
                        AddListItem(NewInstance(i), workingtarget, false);
                        workingsource.Remove(i);
                        amount--;
                    }
                    else
                    {
                        sourcelist = workingsource;
                        targetlist = workingtarget;
                        return;
                    }
                }
                if (takefirstitem)
                    break;
            }
            sourcelist = workingsource;
            targetlist = workingtarget;
        }
    }
}
