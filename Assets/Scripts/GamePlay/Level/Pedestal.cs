using System;
using System.Linq;
using GamePlay.Weapons;
using UnityEngine;

namespace GamePlay.Level
{
    public class Pedestal : MonoBehaviour
    {
        [HideInInspector] public Weapon Item;
        [SerializeField] private SpriteRenderer _itemSprite;
        private int _index;

        private void Start()
        {
            _itemSprite.sprite = Item._uiImage;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var newWeapon = Item.GetComponent<Weapon>();
                if(newWeapon)
                {
                    AppModel.Player().Backpack.AddWeapon(newWeapon);
                }
            }
        }
    }
}
