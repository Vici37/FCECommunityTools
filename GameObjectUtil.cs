namespace FortressCraft.Community
{
	using UnityEngine;

	/// <summary>
	///		Extension Methods and Helpers for the GameObject class
	/// </summary>
	public static class GameObjectUtil
	{
		private static readonly GameObject[] _allObjects;

		/// <summary>
		///		Stores all of the GameObjects from the Games Resources into a cached object
		/// </summary>
		static GameObjectUtil()
		{
			_allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
		}

		/// <summary>
		///		Gets a GameObject from the Resources
		///		Originally created by binaryalgorithm &amp; steveman0
		/// </summary>
		/// <param name="name">The Name of the GameObject to retrieve</param>
		/// <returns>The named GameObject, or null</returns>
		public static GameObject GetObjectFromList(string name)
		{
			for (var i = 0; i < _allObjects.Length; i++)
			{
				if (_allObjects[i].name == name)
					return _allObjects[i];
			}
			return null;
		}
	}
}
