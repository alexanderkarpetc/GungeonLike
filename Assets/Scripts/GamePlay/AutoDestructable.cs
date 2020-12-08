using UnityEngine;

public class AutoDestructable : MonoBehaviour
{
    [SerializeField] private float _seconds;
    void Start()
    {
        Destroy(gameObject, _seconds);
    }
}
