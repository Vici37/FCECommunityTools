using System;

namespace FortressCraft.Community.ItemInterops
{
	/// <summary>
	///		This interface needs to mimic <see cref="CommunityItemInterface">CommunityItemInterface</see>'s methods,
	///		but take a <see cref="SegmentEntity">SegmentEntity</see> as the first param.
	/// </summary>
	internal interface ItemInteropInterface
	{
		Boolean HasItems(SegmentEntity caller, SegmentEntity entity);
		Boolean HasItem(SegmentEntity caller, SegmentEntity entity, ItemBase item);
		Boolean HasItems(SegmentEntity caller, SegmentEntity entity, ItemBase item, out Int32 amount);
		Boolean HasFreeSpace(SegmentEntity caller, SegmentEntity entity, UInt32 amount);
		Int32 GetFreeSpace(SegmentEntity caller, SegmentEntity entity);
		Boolean GiveItem(SegmentEntity caller, SegmentEntity entity, ItemBase item);
		ItemBase TakeItem(SegmentEntity caller, SegmentEntity entity, ItemBase item);
		ItemBase TakeAnyItem(SegmentEntity caller, SegmentEntity entity);
	}
}