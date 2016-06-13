using System;
using System.Collections.Generic;

namespace FortressCraft.Community.Utilities
{
	///	<summary>
	///		Extension Methods and Helpers for the MachineEntity class
	///	</summary>
	public static class MachineEntityUtil
	{

		/// <summary>
		///     Checks the N/E/S/W/Up/Down directions of the Center block for any SegmentEntitys of T;
		///     Will report whether there was a failure because a segment wasn't loaded.
		/// </summary>
		/// <typeparam name="T">The SegmentEntity class to search for.</typeparam>
		/// <param name="center">The MachineEntity to Search Around</param>
		/// <param name="encounteredNullSegments">Whether or not a Null Segment was Encountered</param>
		/// <returns></returns>
		public static List<T> CheckSurrounding<T>(this MachineEntity center, out bool encounteredNullSegment) where T : SegmentEntity
		{
			List<T> ret = new List<T>();
			long[] coords = new long[3];
			encounteredNullSegment = false;
			for (int i = 0; i < 3; ++i) {
				for (int j = -1; j <= 1; j += 2) {
					Array.Clear(coords, 0, 3);
					coords[i] = j;

					long x = center.mnX + coords[0];
					long y = center.mnY + coords[1];
					long z = center.mnZ + coords[2];

					Segment segment = center.AttemptGetSegment(x, y, z);
					// Check if segment was generated (skip this point if it doesn't
					if (segment == null) {
						encounteredNullSegment = true;
						continue;
					}
					T tmcm = segment.SearchEntity(x, y, z) as T;
					if (tmcm != null)
						ret.Add(tmcm);
				}
			}

			return ret;
		}

		public static bool GiveToSurrounding(this MachineEntity center, ItemBase item)
		{
			bool ignore;
			List<SegmentEntity> list = center.CheckSurrounding<SegmentEntity>(out ignore);

			// TODO: Find a better solution to this
			var itemInterop = new ItemInterop(center);

			for (int i = 0; i < list.Count; ++i) {
				// Special case - Conveyor belts should only be given items if they're facing away from the machine
				if (list[i] is ConveyorEntity && center.IsConveyorFacingMe(list[i] as ConveyorEntity)) continue;
				if (itemInterop.GiveItem(list[i], item)) return true;
			}
			return false;
		}

		public static ItemBase TakeFromSurrounding(this MachineEntity center)
		{
			bool ignore;
			List<SegmentEntity> list = center.CheckSurrounding<SegmentEntity>(out ignore);

			// TODO: Find a better solution to this
			var itemInterop = new ItemInterop(center);

			ItemBase ret = null;
			for (int i = 0; i < list.Count; ++i) {
				// Special case - Conveyor belts should will only provide items if they're facing the machine
				if (list[i] is ConveyorEntity && !center.IsConveyorFacingMe(list[i] as ConveyorEntity)) continue;
				ret = itemInterop.TakeAnyItem(list[i]);
				if (ret != null) break;
			}
			return ret;
		}

		public static ItemBase TakeFromSurrounding(this MachineEntity center, ItemBase item)
		{
			bool ignore;
			List<SegmentEntity> list = center.CheckSurrounding<SegmentEntity>(out ignore);

			// TODO: Find a better solution to this
			var itemInterop = new ItemInterop(center);

			ItemBase ret = null;
			for (int i = 0; i < list.Count; ++i) {
				if (list[i] is ConveyorEntity && !center.IsConveyorFacingMe(list[i] as ConveyorEntity)) continue;
				ret = itemInterop.TakeItem(list[i], item);
				if (ret != null) break;
			}
			return ret;
		}
	}
}