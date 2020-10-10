using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
  [HideInInspector] public Weapon Weapon;
  [SerializeField] public Transform _weaponSlot;

  private void Start()
  {
    GetWeapon();
  }

  private void GetWeapon()
  {
    Weapon = _weaponSlot.GetChild(0).GetComponent<Weapon>();
    Weapon.IsPlayers = true;
  }

  void Update()
  {
    CheckShoot();
  }

  private void CheckShoot()
  {
    if (Input.GetMouseButton(0))
    {
      Weapon.TryShoot();
    }
  }
}