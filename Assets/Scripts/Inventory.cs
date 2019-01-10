using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Singleton
    //Instancira sam sebe i održava se na životu kroz sve scene
    public static Inventory instance;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { 
            Debug.LogWarning("More then one instance of Inventory found!");
            Destroy(gameObject);
        }
    }

    #endregion

    // popalio odavde https://www.youtube.com/watch?v=YLhj7SfaxSE

    public List<Item> items = new List<Item>();

    public void Add(Item item)
    {
        items.Add(item);
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public bool Contains(Item item)
    {
        if (items.Contains(item))
        {
            return true;
        }
        else {
            return false;
        }
    }
    
}
