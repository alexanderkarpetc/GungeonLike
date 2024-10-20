using System;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace GamePlay.Level
{
    public class Environment : NetworkBehaviour
    {
        public EnvType Type;
        [SerializeField] private float maxHealth;
        [SerializeField] protected GameObject DestroyFx;
        
        private bool _isDestroying;
        
        private NetworkVariable<float> _health = new NetworkVariable<float>();

        private void Start()
        {
            // todo: init health on server only
            if (IsServer)        
                _health.Value = maxHealth;
        }

        private void OnEnable()
        {
            _health.OnValueChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _health.OnValueChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(float oldHealth, float newHealth)
        {
            if (newHealth <= 0 && !_isDestroying)
            {
                _isDestroying = true;
                DoDestroy();
            }
        }

        // This method is called by players to deal damage, and it's executed on the server
        [ServerRpc(RequireOwnership = false)]
        public void DealDamageServerRpc(float damage)
        {
            if (_isDestroying) return;

            _health.Value -= damage;  // This will automatically sync the health change to all clients
        }

        protected virtual void DoDestroy()
        {
            // Play destroy effects
            if (DestroyFx != null)
            {
                Instantiate(DestroyFx, transform.position, Quaternion.identity);
            }

            DestroyOnClientsClientRpc();
            Destroy(gameObject);
        }

        [ClientRpc]
        private void DestroyOnClientsClientRpc()
        {
            // Clients play destruction effects (if necessary)
            if (DestroyFx != null)
            {
                Instantiate(DestroyFx, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}