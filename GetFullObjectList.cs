using System;

public class GetFullObjectList
{
    public bool runalready = false;
    public List<GameObject> AllObjects;

    public void run()
    {
        // diagnostic dumps 8000+ objects to file (slow)
        List<GameObject> allobjects = (Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]).ToList();
        this.AllObjects = allobjects;
        this.runalready = true;
    }

    public GameObject GetObjectFromList(string name)
    {
        if (!this.runalready)
            this.run();
        for (int n = 0; n < this.AllObjects.Count; n++)
        {
            if (this.AllObjects[n].name == name)
            {
                return this.AllObjects[n];
            }
        }
        return (GameObject)null;
    }
}