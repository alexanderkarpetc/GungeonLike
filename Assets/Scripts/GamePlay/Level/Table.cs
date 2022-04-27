using System;
using UnityEditor.Animations;
using UnityEngine;

namespace GamePlay.Level
{
    public class Table : Environment
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private TableInteractor _interactor;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private GameObject _leftOut;
        [SerializeField] private GameObject _rightOut;
        [SerializeField] private GameObject _topOut;
        [SerializeField] private GameObject _botOut;
        
        private TableSide? _currentSide;
        
        private static readonly int EastFlip = Animator.StringToHash("EastFlip");
        private static readonly int WestFlip = Animator.StringToHash("WestFlip");
        private static readonly int NorthFlip = Animator.StringToHash("NorthFlip");
        private static readonly int SouthFlip = Animator.StringToHash("SouthFlip");

        public void ChooseSide(TableSide? side)
        {
            _currentSide = side;
            _leftOut.SetActive(_currentSide == TableSide.Left);
            _rightOut.SetActive(_currentSide == TableSide.Right);
            _topOut.SetActive(_currentSide == TableSide.Top);
            _botOut.SetActive(_currentSide == TableSide.Bot);
        }

        public void Flip()
        {
            switch (_currentSide)
            {
                case TableSide.Left:
                    _animator.SetTrigger(EastFlip);
                    break;
                case TableSide.Right:
                    _animator.SetTrigger(WestFlip);
                    break;
                case TableSide.Top:
                    _animator.SetTrigger(SouthFlip);
                    break;
                case TableSide.Bot:
                    _animator.SetTrigger(NorthFlip);
                    break;
            }
            var colliderSize = _collider.size;

            _collider.size = _currentSide is TableSide.Bot or TableSide.Top
                ? new Vector2(colliderSize.x, colliderSize.y / 2)
                : new Vector2(colliderSize.x / 2, colliderSize.y);

            gameObject.tag = "Environment";
            _leftOut.SetActive(false);
            _rightOut.SetActive(false);
            _topOut.SetActive(false);
            _botOut.SetActive(false);
            Destroy(_interactor.gameObject);
        }
    }

    public enum TableSide
    {
        Left, Right, Top, Bot
    }
}