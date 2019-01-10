using UnityEngine;

//Omogućava kreiranje novog itema iz asset izbornika
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
//Generator za iteme, može se dodati recimo sprite, description, stats i kategorije tog tipa
public class Item : ScriptableObject {

    public string itemName = "Key";

}
