# Community Made Functions For Mod Development

This project aim to make modding [Fortress Craft Evolved](store.steampowered.com/app/254200/?snr=1_7_15__13), mainly by providing standardized methods for interacting with Items and GameObjects. Among other things, this library provides a CommunityItemInterface as well as some helper functions to help interact with other CommunityItemInterface implementors, as well as a standardized way to interact with some vanilla machines as well.

Contributors:
* [Vici37](https://github.com/Vici37)
* [Steveman0](https://github.com/steveman0)
* [Nikey646](https://github.com/Nikey646)

# How Do I Use This For My Mod?
Clone this repo (or just download it), and copy the FCECommunityTools.dll into the same place you'd put your plugin\_YourMod.dll . Then, in your development environment of choice, add FCECommunityTools.dll as a reference to your project.

# CommunityItemInterface
Provides the following functions:

    Boolean HasItems();
    Boolean HasItems();
    Boolean HasItem(ItemBase item);
    Boolean HasItems(ItemBase item, out Int32 amount);
    Boolean HasFreeSpace(UInt32 amount);
    Int32 GetFreeSpace();
    Boolean GiveItem(ItemBase item);
    ItemBase TakeItem(ItemBase item);
    ItemBase TakeAnyItem();

# Helper Methods
CommunityUtil provides some extension methods for MachineEntities. Namely:

    // Get a list of all CommunityItemInterface implementors that touch the machineEntity block (can be applied to most classes instead)
    bool encounteredNullSegment;
    List<CommunityItemInterface> itemInterfaces = machineEntity.CheckSurrounding<CommunityItemInterface>(out encounterdNullSegment);

    // Give an ItemBase to a machine touching this one that will take it
    // Will handle CommunityItemInterface, StorageHopper, and ConveyorEntity
    machineEntity.GiveToSurrounding(someItemBase);
    
    // Take an item from the surroundings that match the provided ItemBase
    ItemBase item = machineEntity.TakeFromSurrounding(someItemBase);

    // Take any ItemBase from the surroundings that's being offered
    ItemBase item = machineEntity.TakeFromSurrounding();

# Want to Help?
Make a pull request with what changes you want to see!
