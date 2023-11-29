using BH.Enemies;
using BH.Game;
using System;
using System.Collections;
using UnityEngine;

namespace BH.Patterns
{
    public class PatternCircleAround : AtkPattern
    {
        [Header("Shoot Origin")]
        [SerializeField][Range(0f, 360f)] private float m_startAngle;
        [SerializeField][Range(0f, 360f)] private float m_angleOffset;
        [SerializeField] private float m_distFromCenter;

        [Header("One Burst Params")]
        [SerializeField] private float m_rotationEachBurst;
        [SerializeField] private int m_nbBullet;
        [SerializeField] private float m_shakeAmount;

        [Header("If targets the player")]
        [SerializeField] private Transform m_playerTrs;

        [Header("Audio")]
        [SerializeField] private AudioClip m_shootClip;
        [SerializeField] private AudioSource m_shootSource;
        [SerializeField][Range(0f, 1f)] private float m_volume;

        private EnemyController m_enemy;
        private Vector3 m_previousRotation;

        /*-------------------------------------------------------------------*/

        public override void StartAtkPattern(EnemyController enemy)
        {
            m_shooterTrs = enemy.transform;
            m_enemy = enemy;

            StopAllCoroutines();
            StartCoroutine(BurstManagement());
        }

        protected override void DoOneBurst(int burstIndex)
        {
            //secu
            if (m_bulletManager == null)
            {
                Debug.Log("no bullet manager");
                return;
            }

            Shoot();

            //screen shake
            ScreenShake.instance.m_amount += m_shakeAmount;

            //sfx
            if (m_shootSource != null && m_shootClip != null)
                m_shootSource.PlayOneShot(m_shootClip, m_volume);
        }

        /*-------------------------------------------------------------------*/
        private void Shoot()
        {
            float angleBetweenBullet = m_angleOffset;

            //fix bullet distribution
            if (m_nbBullet > 1)
            {
                float angleOffset = Mathf.Clamp(m_angleOffset, 0, 360f - (360f / m_nbBullet));
                angleBetweenBullet = angleOffset / (m_nbBullet - 1);
            }

            //bullet distribution
            for (int i = 0; i < m_nbBullet; i++)
            {
                float radians = (m_shooterTrs.rotation.eulerAngles.z + m_startAngle + angleBetweenBullet * i) * Mathf.Deg2Rad;
                Vector3 dir = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));


                Vector3 origin = m_shooterTrs.position + dir * m_distFromCenter;

                m_bulletManager.Shoot(origin, dir);
            }
        }

        private IEnumerator BurstManagement()
        {
            //do all the burst
            for (int i = 0; i < m_burstNb; i++)
            {
                //init for rotation
                float currentTime = 0f;
                m_previousRotation = m_shooterTrs.rotation.eulerAngles;

                //rotate shooter before burst
                while (currentTime < m_timeBetweenBurst)
                {
                    currentTime += Time.deltaTime;
                    RotateBetweenBurst(currentTime, m_timeBetweenBurst);

                    yield return null;
                }

                DoOneBurst(i);
            }

            //next pattern
            m_enemy.FinishAnAtkPattern();
        }

        private void RotateBetweenBurst(float currentTime, float maxTime)
        {
            float ratio = currentTime / maxTime;

            //align with the player
            if (m_playerTrs != null)
            {
                LookToTarget(m_playerTrs.position, Vector2.right, ratio);
            }
            //rotate between burst
            else
            {
                Vector3 newRot = m_previousRotation + Vector3.forward * m_rotationEachBurst * ratio;
                m_shooterTrs.rotation = Quaternion.Euler(newRot);
            }
        }

        private void LookToTarget(Vector3 targetPos, Vector2 originalLookingDir, float ratio = 1)
        {
            //find the direction to look to target
            Vector2 direction = targetPos - m_shooterTrs.position;
            direction = direction.normalized;
            float angle = Vector2.SignedAngle(originalLookingDir, direction);

            //fix rotation
            m_previousRotation.z %= 360;
            m_shooterTrs.rotation = Quaternion.Euler(m_previousRotation);

            angle -= m_previousRotation.z;
            if (targetPos.y < 0)
            {
                angle += 360;
            }
            if (Mathf.Abs(angle) > 180)
            {
                float signe = (angle / Mathf.Abs(angle));
                angle = -signe * (360 - (Mathf.Abs(angle)));
            }


            //set new rotation (lerp with ratio)
            Vector3 newRot = m_previousRotation + Vector3.forward * angle * ratio;
            m_shooterTrs.rotation = Quaternion.Euler(newRot);
        }
    }
}
