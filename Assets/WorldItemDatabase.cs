using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CX
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase instance;


        public WeaponItem unarmedWeapon;
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        private List<Item> items = new List<Item>();


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            foreach(var weapon in weapons)
            {
                items.Add(weapon);
            }
            for(int i =0;i< items.Count;i++)
            {
                items[i].itemID = i;
            }
        }
        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(wepon => wepon.itemID == ID);
        }
    }
}
