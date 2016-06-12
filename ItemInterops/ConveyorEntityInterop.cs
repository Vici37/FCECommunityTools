using FortressCraft.Community.Utilities;

namespace FortressCraft.Community.ItemInterops {
    internal class ConveyorEntityInterop : ItemInteropInterface {
        public int GetFreeSpace(SegmentEntity entity) {
            var conveyor = entity.As<ConveyorEntity>();

            if (conveyor.mbReadyToConvey) return 1;
            return 0; 
        }

        public bool GiveItem(SegmentEntity entity, ItemBase item) {
            if (GetFreeSpace(entity) == 1) {
                var conveyor = entity.As<ConveyorEntity>();
                if (item.mType == ItemType.ItemCubeStack) {
                    ItemCubeStack cube = item as ItemCubeStack;
                    conveyor.AddCube(cube.mCubeType, cube.mCubeValue, 1);
                    return true;
                }
                conveyor.AddItem(item);
                return true;
            }
            return false;
        }

        public bool HasFreeSpace(SegmentEntity entity, uint amount) {
            if (amount > 1) return false;
            return GetFreeSpace(entity) >= amount;
        }
        
        public bool HasItem(SegmentEntity entity, ItemBase item) {
            if (GetFreeSpace(entity) == 1) return false;
            var conveyor = entity.As<ConveyorEntity>();
            if(item.mType == ItemType.ItemCubeStack) {
                ItemCubeStack cube = item as ItemCubeStack;
                if(conveyor.mCarriedCube == cube.mCubeType && conveyor.mCarriedValue == cube.mCubeValue) return true;
                return false;
            }
            return item.CompareDeep(conveyor.mCarriedItem);
        }

        public bool HasItems(SegmentEntity entity) {
            return GetFreeSpace(entity) > 0;
        }

        public bool HasItems(SegmentEntity entity, ItemBase item, out int amount) {
            amount = 0;
            if (GetFreeSpace(entity) == 1) return false;
            if(HasItem(entity, item)) {
                amount = 1;
                return true;
            }
            return false;
        }

        public ItemBase TakeAnyItem(SegmentEntity entity) {
            if (GetFreeSpace(entity) == 1) return null;
            var conveyor = entity as ConveyorEntity;
            ItemBase ret = null;
            if (conveyor.mCarriedCube != 0) {
                ret = ItemManager.SpawnCubeStack(conveyor.mCarriedCube, conveyor.mCarriedValue, 1);
            } else {
                ret = conveyor.mCarriedItem;
            }

            if (ret == null) return null;
            conveyor.RemoveCube();
            conveyor.RemoveItem();
            conveyor.FinaliseOffloadingCargo();
            return ret;
        }

        public ItemBase TakeItem(SegmentEntity entity, ItemBase item) {
            if (GetFreeSpace(entity) == 1) return null;
            if (!HasItem(entity, item)) return null;
            return TakeAnyItem(entity);
        }
    }
}
