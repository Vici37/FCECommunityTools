using System;
using System.Collections.Generic;

namespace FortressCraft.Community.Utilities
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
		
		public static KeyValuePair<UInt16, UInt16> GetCubeData(this SegmentEntity entity, string dataName)
		{
			UInt16 type;
			UInt16 value;

			if (!entity.GetCubeData(dataName, out type, out value))
				return new KeyValuePair<UInt16, UInt16>(0, 0);
			return new KeyValuePair<UInt16, UInt16>(type, value);
		}

		public static Boolean GetCubeData(this SegmentEntity entity, string dataName, out UInt16 cubeType, out UInt16 cubeValue)
		{
			cubeType = 0;
			cubeValue = 0;

			TerrainDataEntry terrainDataEntry;
			TerrainDataValueEntry terrainDataValueEntry;
			TerrainData.GetCubeByKey(dataName, out terrainDataEntry, out terrainDataValueEntry);

			if (terrainDataEntry == null)
				return false;

			cubeType = terrainDataEntry.CubeType;
			cubeValue = terrainDataValueEntry.Value;

			return true;
		}

	}
}
