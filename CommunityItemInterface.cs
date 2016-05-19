namespace FortressCraft.Community
{
	using System;

	/// <summary>
	///		A community interface to allow cross-mod communication for item storage
	/// </summary>
	public interface CommunityItemInterface
	{
		///  <summary>
		/// 		Checks to see if a Storage Medium has the specified item.
		///  
		/// 		Checks for Ore/Item/Cube
		///  </summary>
		///  <param name="item">The Item to Check For</param>
		///  <returns>True if the storage medium has the specified item, false otherwise</returns>
		Boolean HasItem(ItemBase item);

		///  <summary>
		/// 		Checks to see if a Storage Medium has the specified item.
		///  
		/// 		Checks for Ore/Item/Cube
		///  </summary>
		///  <param name="item">The Item to Check For</param>
		///  <param name="amount">Stores the amount of the items available</param>
		///  <returns>True if the storage medium has the specified item, false otherwise</returns>
		Boolean HasItems(ItemBase item, out Int32 amount);

		/// <summary>
		///		Checks to see if a Storage Medium has <c>amount</c> capcity free
		/// </summary>
		/// <param name="amount">The amount of capcity to check for</param>
		/// <returns>True if the storage medium has <c>amount</c> capacity free, otherwise false</returns>
		Boolean HasCapcity(UInt32 amount);

		/// <summary>
		///		Attempts to give the Item to a Storage Medium
		/// </summary>
		/// <param name="item">The Item to give to a Storage Medium</param>
		/// <returns>True if the Storage Medium accepted the item, otherwise false</returns>
		Boolean GiveItem(ItemBase item);

		/// <summary>
		///		Attempts to take an Item from a Storage Medium
		/// </summary>
		/// <param name="item">The Item to Retrieve</param>
		/// <returns>An ItemBase if the requested Item was found, otherwise null</returns>
		ItemBase TakeItem(ItemBase item);

		/// <summary>
		///		Attempts to take an Item from a Storage Medium based off of Item Id / Cube Id + Value
		/// </summary>
		/// <param name="id">Item or Cube Id</param>
		/// <param name="value">Cube Value</param>
		/// <returns>An ItemBase if the requested Item was found, otherwise null</returns>
		ItemBase TakeItem(UInt16 id, UInt16? value = null);
	}
}