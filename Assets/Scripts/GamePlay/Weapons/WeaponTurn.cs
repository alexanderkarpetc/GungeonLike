using GamePlay.Weapons;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay
{
    public class WeaponTurn : NetworkBehaviour
    {
        [SerializeField] public Weapon Weapon;
        [SerializeField] protected Vector3 _leftHandPos;
        [SerializeField] protected Vector3 _rightHandPos;
        [SerializeField] protected GameObject _weaponSlot;

        // Hand positions and rendering
        public Transform _rightHand;
        public Transform _leftHand;
        protected SpriteRenderer _leftHandRenderer;
        protected SpriteRenderer _rightHandRenderer;

        // Angle for turning the gun
        protected float Angle;

        // NetworkVariable to sync the angle across clients
        private NetworkVariable<float> NetworkAngle = new NetworkVariable<float>(
            writePerm: NetworkVariableWritePermission.Owner);

        private void Start()
        {
            _leftHand.localPosition = _leftHandPos;
            _rightHand.localPosition = _rightHandPos;
            _leftHandRenderer = _leftHand.GetComponent<SpriteRenderer>();
            _rightHandRenderer = _rightHand.GetComponent<SpriteRenderer>();
            OnStart();
        }

        protected virtual void OnStart() { }

        private void Update()
        {
            if(!Weapon) return;
            if (IsOwner)
            {
                // Turn gun based on mouse position
                TurnGun();

                // Sync angle if it has changed significantly
                if (Mathf.Abs(Angle - NetworkAngle.Value) > 0.5f)
                {
                    NetworkAngle.Value = Angle;
                }
            }
            else
            {
                ApplyNetworkAngle();
            }

            ChangeSortingOrder();
            MoveHands();
        }

        protected virtual void TurnGun()
        {
            // Calculate angle based on mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - transform.position;
            Angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        private void ApplyNetworkAngle()
        {
            // Sync angle from the server or owner
            _weaponSlot.transform.rotation = Quaternion.Euler(0f, 0f, NetworkAngle.Value);
        }

        private void MoveHands()
        {
            if (Mathf.Abs(Angle) < 90)
            {
                _leftHand.localPosition = _leftHandPos;
                // Now we use _weaponSlot to handle weapon positioning
                _weaponSlot.transform.position = !Weapon.IsDoubleHanded ? _rightHand.position : _leftHand.position;
                SpriteUtil.SetXScale(_weaponSlot, 1);
                Weapon.IsInverted = false;
                if (Weapon.SecondHandPos != null)
                    _rightHand.position = Weapon.SecondHandPos.position;
            }
            else
            {
                _rightHand.localPosition = _rightHandPos;
                // Use the weapon slot for inverted positioning as well
                _weaponSlot.transform.position = !Weapon.IsDoubleHanded ? _leftHand.position : _rightHand.position;
                SpriteUtil.SetXScale(_weaponSlot, -1);
                Weapon.IsInverted = true;
                if (Weapon.SecondHandPos != null)
                    _leftHand.position = Weapon.SecondHandPos.position;
            }
        }

        protected virtual void ChangeSortingOrder() { }
    }
}