using _Project.Scripts.Helper;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


namespace _Project.Scripts.GameLogic
{
    public class CarMover : MonoBehaviour,ICommandMove
    { 
        private int _carLayer;
        private int _mapLayer;
        private void Start()
        { 
            _carLayer = 1 << LayerMask.NameToLayer(Constants.CAR_LAYER);
            _mapLayer = 1 << LayerMask.NameToLayer(Constants.MAP_LAYER);
        } 
        public void Move()
        {
            
        } 
        public void CancelMove()
        {

        }
        IEnumerator CorMove(Vector3 startPosition)
        {
            yield return null;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject != null && 1 << collision.gameObject.layer == _carLayer)
            {
                CancelMove();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject != null && 1 << other.gameObject.layer == _mapLayer)
            {
                
            }
        }
    } 
    public interface ICommandMove
    {
        void Move();  
        void CancelMove();   
    }
}