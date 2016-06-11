using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.CompilerServices;
using FortressCraft.Community.ItemInterops;

namespace FortressCraft.Community
{
	/// <summary>
	///		A class that provides Interopability between Storage and Machines for Pre-StorageInterface mods
	/// </summary>
	public class ItemInterop
	{
		// Not truly readonly, it can be accessed and manually added to via Reflection
		// But if someone is that desparate, than whatever.
		private static readonly Dictionary<Type, ItemInteropInterface> _supportedTypes;

		private readonly MachineEntity _caller;

		static ItemInterop()
		{
			_supportedTypes = new Dictionary<Type, ItemInteropInterface>
			{
				{ typeof (StorageHopper), new StorageHopperInterop() },
				{ typeof (ConveyorEntity), new ConveyorEntityInterop() }
			};
		}

		public ItemInterop(MachineEntity caller)
		{
			if (caller == null)
				throw new ArgumentNullException(nameof(caller));
			this._caller = caller;
		}
		
		private static Type SupportedType(SegmentEntity entity)
		{
			if (_supportedTypes.ContainsKey(entity.GetType()))
				return entity.GetType();

			return entity is CommunityItemInterface ? typeof (CommunityItemInterface) : null;
		}

		/// <summary>
		///		Checks to see if the <see cref="SegmentEntity">entity</see> has any items
		/// </summary>
		/// <param name="entity">A <see cref="SegmentEntity">SegmentEntity</see></param>
		/// <returns>True if the <see cref="SegmentEntity">entity</see> has any items, false otherwise</returns>
		public Boolean HasItems(SegmentEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			var type = SupportedType(entity);
			if (type == null)
				return false;

			if (type == typeof (CommunityItemInterface))
			{
				var iEntity = entity as CommunityItemInterface;
				return iEntity.HasItems();
			}

			var interop = _supportedTypes[type];
			return interop.HasItems(this._caller, entity);
		}

		public Boolean HasItem(SegmentEntity entity, ItemBase item)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			Int32 amount;
			return this.HasItems(entity, item, out amount);
		}

		public Boolean HasItems(SegmentEntity entity, ItemBase item, out Int32 amount)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			amount = 0;
			var type = SupportedType(entity);
			if (type == null)
				return false;

			if (type == typeof (CommunityItemInterface))
			{
				var iEntity = entity as CommunityItemInterface;
				return iEntity.HasItems(item, out amount);
			}

			var interop = _supportedTypes[type];
			return interop.HasItems(this._caller, entity, item, out amount);
		}

		public Boolean HasCapacity(SegmentEntity entity, UInt32 amount)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			var type = SupportedType(entity);
			if (type == null)
				return false;

			if (type == typeof (CommunityItemInterface))
			{
				var iEntity = entity as CommunityItemInterface;
				return iEntity.HasFreeSpace(amount);
			}

			var interop = _supportedTypes[type];
			return interop.HasFreeSpace(this._caller, entity, amount);
		}

		public Int32 GetCapacity(SegmentEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			var type = SupportedType(entity);
			if (type == null)
				return 0;

			if (type == typeof (CommunityItemInterface))
			{
				var iEntity = entity as CommunityItemInterface;
				return iEntity.GetFreeSpace();
			}

			var interop = _supportedTypes[type];
			return interop.GetFreeSpace(this._caller, entity);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public Boolean GiveItem(SegmentEntity entity, ItemBase item)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			var type = SupportedType(entity);
			if (type == null)
				return false;

			if (type == typeof (CommunityItemInterface))
			{
				var iEntity = entity as CommunityItemInterface;
				return iEntity.GiveItem(item);
			}
			var interop = _supportedTypes[type];
			return interop.GiveItem(this._caller, entity, item);
		}

		/// <summary>
		///		Will attempt to return the requested <see cref="ItemBase">item</see> from the <see cref="SegmentEntity">entity</see>
		/// </summary>
		/// <param name="entity">The <see cref="SegmentEntity">entity</see> to try and get the <see cref="ItemBase">item</see> from</param>
		/// <param name="item">The <see cref="ItemBase">ItemBase</see> with details of what to retrrieve</param>
		/// <returns></returns>
		public ItemBase TakeItem(SegmentEntity entity, ItemBase item)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));
			if (item == null)
				throw new ArgumentNullException(nameof(item));

			try
			{
				var type = SupportedType(entity);
				if (type == null)
					return null;

				if (type == typeof (CommunityItemInterface))
				{
					var iEntity = entity as CommunityItemInterface;
					return iEntity.TakeItem(item);
				}

				var interop = _supportedTypes[type];
				return interop.TakeItem(this._caller, entity, item);
			}
			catch(NotImplementedException)
			{
				return null;
			}
		}

		/// <summary>
		///		Will return any <see cref="ItemBase">item</see> from the <see cref="SegmentEntity">entity</see>
		/// </summary>
		/// <param name="entity">The <see cref="SegmentEntity">entity</see> to get a <see cref="ItemBase">item</see> from</param>
		/// <returns>A <see cref="ItemBase">item</see>, or <c>NULL</c></returns>
		public ItemBase TakeAnyItem(SegmentEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			try
			{
				var type = SupportedType(entity);
				if (type == null)
					return null;

				if (type == typeof (CommunityItemInterface))
				{
					var iEntity = entity as CommunityItemInterface;
					return iEntity.TakeAnyItem();
				}

				var interop = _supportedTypes[type];
				return interop.TakeAnyItem(this._caller, entity);
			}
			catch (NotImplementedException)
			{
				return null;
			}
		}

	}
}
