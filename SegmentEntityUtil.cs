using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FortressCraft.Community
{
	/// <summary>
	///		Extension Methods and Helpers for the SegmentEntity class
	/// </summary>
	public static class SegmentEntityUtil
	{

		/// <summary>
		///		Convert a SegmentEntity into a class that inherit SegmentEntity
		/// </summary>
		/// <typeparam name="TSegmentEntity">Class that Inherits SegmentEntity</typeparam>
		/// <param name="entity">The entity to convert</param>
		/// <returns>The entity as TSegmentEntity</returns>
		public static TSegmentEntity As<TSegmentEntity>(this SegmentEntity entity) where TSegmentEntity : SegmentEntity
		{
			return entity as TSegmentEntity;
		} 

	}
}
