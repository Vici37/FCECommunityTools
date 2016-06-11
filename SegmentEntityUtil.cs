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

		// Outputs the SegmentEntity's coordinates as a string, handy for debugging, maybe

		public static string GetPosString(this SegmentEntity ent)
		{
			return "[" + ent.mnX + ", " + ent.mnY + ", " + ent.mnZ + "]";
		}

		public static bool IsConveyorFacingMe(this SegmentEntity center, ConveyorEntity conv)
		{
			long x = conv.mnX + (long)conv.mForwards.x;
			long y = conv.mnY + (long)conv.mForwards.y;
			long z = conv.mnZ + (long)conv.mForwards.z;
			return (x == center.mnX) &&
			       (y == center.mnY) &&
			       (z == center.mnZ);
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
