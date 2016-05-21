using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// A collection of static methods that don't really belong anywhere
public static class CommunityUtil {

    /// <summary>
    ///     Checks the N/E/S/W/Up/Down directions of the Center block for any SegmentEntitys of T;
    ///     Will report whether there was a failure because a segment wasn't loaded.
    /// </summary>
    /// <typeparam name="T">The SegmentEntity class to search for.</typeparam>
    /// <param name="center">The MachineEntity to Search Around</param>
    /// <param name="encounteredNullSegments">Whether or not a Null Segment was Encountered</param>
    /// <returns></returns>
    public static List<T> CheckSurrondings<T>(this MachineEntity center, out Boolean encounteredNullSegments) where T : SegmentEntity
    {
        return checkSurrounding<T>(center, out encounteredNullSegments);
    }

    // Used to get all items of a specific type around the provided machine entity.
    // Will report whether there was a failure because a segment wasn't loaded.
    //
    // Example Usage:
    // bool encounteredNullSegment;
    // List<CoveyorEntity> conveyorBelts = CommunityUtil.checkSurrounding<ConveyorEntity>(this, encounteredNullSegment);
    public static List<T> checkSurrounding<T>(MachineEntity center, out bool encounteredNullSegment) where T : SegmentEntity {
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

    // Outputs the SegmentEntity's coordinates as a string, handy for debugging, maybe
    public static string getPosString(SegmentEntity ent) {
        return "[" + ent.mnX + ", " + ent.mnY + ", " + ent.mnZ + "]";
    }
}
