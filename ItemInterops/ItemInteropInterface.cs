using System;

namespace FortressCraft.Community.ItemInterops
{
	/// <summary>
	///		This interface needs to mimic <see cref="CommunityItemInterface">CommunityItemInterface</see>'s methods,
	///		but take a <see cref="SegmentEntity">SegmentEntity</see> as the first param.
	/// </summary>
	internal interface ItemInteropInterface
	{
		Boolean HasItems(SegmentEntity entity);
		Boolean HasItem(SegmentEntity entity, ItemBase item);
		Boolean HasItems(SegmentEntity entity, ItemBase item, out Int32 amount);
		Boolean HasFreeSpace(SegmentEntity entity, UInt32 amount);
		Int32 GetFreeSpace(SegmentEntity entity);
		Boolean GiveItem(SegmentEntity entity, ItemBase item);
		ItemBase TakeItem(SegmentEntity entity, ItemBase item);
		ItemBase TakeAnyItem(SegmentEntity entity);
	}
}