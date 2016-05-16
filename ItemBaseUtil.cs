using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Collection of functions to help compare and manipulate items.
public static class ItemBaseUtil {

    // Deep comparison of the two provided ItemBases (check types as well)
    public static bool compareBaseDeep(ItemBase a, ItemBase b) {
        return a.mnItemID == b.mnItemID && a.mType == b.mType &&
            (compareCubeStack(a as ItemCubeStack, b as ItemCubeStack) ||
             compareDurability(a as ItemDurability, b as ItemDurability) ||
             compareStack(a as ItemStack, b as ItemStack) ||
             compareSingle(a as ItemSingle, b as ItemSingle) ||
             compareCharge(a as ItemCharge, b as ItemCharge) ||
             compareLocation(a as ItemLocation, b as ItemLocation));
    }

    // Compare just the basics of the items
    public static bool compareBase(ItemBase a, ItemBase b) {
        return a.mnItemID == b.mnItemID && a.mType == b.mType;
    }

    // Compare the ItemCubeStacks. Does not compare ammounts
    public static bool compareCubeStack(ItemCubeStack a, ItemCubeStack b) {
        // We'll ignore the item stacks for now. May revisit in the future
        return a != null && b != null && compareBase(a, b) && a.mCubeType == b.mCubeType && a.mCubeValue == b.mCubeValue;// && a.mnAmount == b.mnAmount;
    }

    // Compare ItemDurability types, might be better to ignore currentDurability
    public static bool compareDurability(ItemDurability a, ItemDurability b) {
        return a != null && b != null && compareBase(a, b) && a.mnCurrentDurability == b.mnCurrentDurability && a.mnMaxDurability == b.mnMaxDurability;
    }

    // Compare ItemStacks, ignore amounts
    public static bool compareStack(ItemStack a, ItemStack b) {
        // Again, we'll ignore the size of the stack for now
        return a != null && b != null && compareBase(a, b);// && a.mnAmount == b.mnAmount;
    }

    // Compare ItemSingle, really a wrapper for compareBase, but also checks the class types
    public static bool compareSingle(ItemSingle a, ItemSingle b) {
        return a != null && b != null && compareBase(a, b);
    }

    // Compare ItemCharge, might be better to ignore charge level
    public static bool compareCharge(ItemCharge a, ItemCharge b) {
        return a != null && b != null && compareBase(a, b) && a.mChargeLevel == b.mChargeLevel;
    }

    // Compare ItemLocation
    public static bool compareLocation(ItemLocation a, ItemLocation b) {
        return a != null && b != null && compareBase(a, b) && a.mLocX == b.mLocX && a.mLocY == b.mLocY && a.mLocZ == b.mLocZ &&
            a.mLookVector.x == b.mLookVector.x && a.mLookVector.y == b.mLookVector.y && a.mLookVector.z == b.mLookVector.z;
    }

    public static bool isStackAndSame(ItemBase a, ItemBase b) {
        return a != null && b != null && (compareCubeStack(a as ItemCubeStack, b as ItemCubeStack) || compareStack(a as ItemStack, b as ItemStack));
    }

    public static bool isStack(ItemBase a) {
        return a != null && (a.mType == ItemType.ItemCubeStack || a.mType == ItemType.ItemStack);
    }

    // Helpful if you don't want to convert an ItemBase to either ItemCubeStack or ItemStack
    public static void incrementStack(ItemBase a, int amount) {
        if(a.mType == ItemType.ItemCubeStack) {
            (a as ItemCubeStack).mnAmount += amount;
        } else if(a.mType == ItemType.ItemStack) {
            (a as ItemStack).mnAmount += amount;
        }
    }

    // Helpful if you don't want to convert an ItemBase to either ItemCubeStack or ItemStack
    public static void decrementStack(ItemBase a, int amount) {
        if (a.mType == ItemType.ItemCubeStack) {
            (a as ItemCubeStack).mnAmount -= amount;
        } else if (a.mType == ItemType.ItemStack) {
            (a as ItemStack).mnAmount -= amount;
        }
    }

    // Helpful if you don't want to convert an ItemBase to either ItemCubeStack or ItemStack
    public static void setAmount(ItemBase a, int amount) {
        if (a.mType == ItemType.ItemCubeStack) {
            (a as ItemCubeStack).mnAmount = amount;
        } else if (a.mType == ItemType.ItemStack) {
            (a as ItemStack).mnAmount = amount;
        }
    }

    // Clones the provided ItemBase
    public static ItemBase newInstance(ItemBase a) {
        switch(a.mType) {
            case ItemType.ItemCubeStack:
                ItemCubeStack ics = a as ItemCubeStack;
                return new ItemCubeStack(ics.mCubeType, ics.mCubeValue, ics.mnAmount);
            case ItemType.ItemStack:
                ItemStack its = a as ItemStack;
                return new ItemStack(its.mnItemID, its.mnAmount);
            case ItemType.ItemCharge:
                ItemCharge ic = a as ItemCharge;
                return new ItemCharge(ic.mnItemID, (int)ic.mChargeLevel);
            case ItemType.ItemDurability:
                ItemDurability id = a as ItemDurability;
                return new ItemDurability(id.mnItemID, id.mnCurrentDurability, id.mnMaxDurability);
            case ItemType.ItemLocation:
                ItemLocation il = a as ItemLocation;
                return new ItemLocation(il.mnItemID, il.mLocX, il.mLocY, il.mLocZ, il.mLookVector);
            case ItemType.ItemSingle:
                return new ItemSingle(a.mnItemID);
        }
        return null;
    }

    // Helpful if you don't want to convert an ItemBase to either ItemCubeStack or ItemStack
    public static int getAmount(ItemBase item) {
        if (item.mType == ItemType.ItemCubeStack) {
            ItemCubeStack a = item as ItemCubeStack;
            if (a != null) return a.mnAmount;
        } else if (item.mType == ItemType.ItemStack) {
            ItemStack a = item as ItemStack;
            if (a != null) return a.mnAmount;
        }
        return 1;
    }

    // Gets the amount of all items in a list, also handling item stacks
    public static int getItemCount(List<ItemBase> items) {
        int ret = 0;
        foreach(ItemBase it in items) {
            ret += getAmount(it);
        }
        return ret;
    }
}
