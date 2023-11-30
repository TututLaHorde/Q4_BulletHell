using UnityEngine;

namespace BH.Game
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform trsTarget;
        private Transform trsInitial;
        [SerializeField] private float smoothTime;
        private Vector3 velocity = Vector3.zero;

        private void Start()
        {
            trsInitial = trsTarget;
        }

        void FixedUpdate()
        {
            if (trsTarget != null)
            {
                //follow player with smooth
                Vector3 targetPosition = new Vector3(trsTarget.position.x, trsTarget.position.y, transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
        }

        public void ResetTarget()
        {
            trsTarget = trsInitial;
        }
    }
}
