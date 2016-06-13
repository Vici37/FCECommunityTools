using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FortressCraft.Community.Utilities
{
	/// <summary>
	///		Extension Methods and Helpers for the ConveyorEntity class
	/// </summary>
	public static class ConveyorEntityUtil
	{
		
		// Inverse of SegmentEntity.IsConveyorFacingMe. Seems a bit more natural, to Nikey~
		/// <summary>
		///		Will check to see if the Conveyor is facing the supplied Machine
		/// </summary>
		/// <param name="conveyor">The conveyor to check</param>
		/// <param name="entity">The entity to check against</param>
		/// <returns>True if the Conveyor Is Facing the Entity.</returns>
		public static Boolean IsFacing(this ConveyorEntity conveyor, SegmentEntity entity)
		{
			return entity.IsConveyorFacingMe(conveyor);
		}

	}
}
