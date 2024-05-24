using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CX
{
    public class WorldActionManager : MonoBehaviour
    {
        public static WorldActionManager instance;

        [Header("Weapon item Action")]
        public WeaponItemAction[] weponItemActions;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject); 
            }

            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            for(int i =0;i<weponItemActions.Length;i++)
            {
                weponItemActions[i].actionID = i;
            }
        }
        public WeaponItemAction GetWeaponItemActionByID(int id)
        {
            return weponItemActions.FirstOrDefault(action => action.actionID == id);
        }
    }
}
