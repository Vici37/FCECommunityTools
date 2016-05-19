namespace FortressCraft.Community
{
	using UnityEngine;

	public static class GameObjectUtil
	{
		private static GameObject[] _allObjects;
		 
		static GameObjectUtil()
		{
			_allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
		}

        // Originally By binaryalgorithm & steveman0
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
