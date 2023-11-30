using UnityEngine;

namespace BH.Game
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform trPlayer;
        [SerializeField] private float smoothTime;
        private Vector3 velocity = Vector3.zero;

        void FixedUpdate()
        {
            if (trPlayer != null)
            {
                //follow player with smooth
                Vector3 targetPosition = new Vector3(trPlayer.position.x, trPlayer.position.y, transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }
}
