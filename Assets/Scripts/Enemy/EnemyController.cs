using Pathfinding;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private AIDestinationSetter _destinationSetter;
        [SerializeField] private AIPath _aiPath;
        [SerializeField] private float speed;

        private void Start()
        {
            _destinationSetter.target = GameObject.Find("Player").transform;
            _aiPath.maxSpeed = speed;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
