using BH.Enemies;
using BH.Game;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
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

            //targets the player
            if (m_playerTrs != null) 
            {
                LookToTarget(m_enemy.transform, m_playerTrs.position, Vector2.right);
            }

            Shoot();

            //screen shake
            ScreenShake.instance.m_amount += m_shakeAmount;

            //sfx
            if (m_shootSource != null && m_shootClip != null)
                m_shootSource.PlayOneShot(m_shootClip, m_volume);

            //changements after shoot
            Vector3 newRot = m_shooterTrs.rotation.eulerAngles + Vector3.forward * m_rotationEachBurst;
            m_shooterTrs.rotation = Quaternion.Euler(newRot);
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
            for (int i = 0; i < m_burstNb; i++)
            {
                DoOneBurst(i);

                yield return new WaitForSeconds(m_timeBetweenBurst);
            }

            m_enemy.FinishAnAtkPattern();
        }

        private void LookToTarget(Transform ownTrs, Vector3 tagetPos, Vector2 originalLookingDir)
        {
            Vector2 direction = tagetPos - ownTrs.position;

            direction = direction.normalized;
            float angle = Vector2.SignedAngle(originalLookingDir, direction);
            ownTrs.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }
}
