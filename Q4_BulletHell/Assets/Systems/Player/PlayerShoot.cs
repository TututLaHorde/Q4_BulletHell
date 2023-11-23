using BH.Bullets;
using BH.Player;
using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float m_bulletIntervale;
    [SerializeField][Range(0f, 10f)] private float m_bulletAccuracy;

    [SerializeField] private BulletManager m_bulletManager;
    [SerializeField] private Transform m_shootOrigin;

    private PlayerController m_playerController;

    private void Start()
    {
        m_playerController = GetComponent<PlayerController>();

        StartCoroutine(AutoShoot());
    }

    private IEnumerator AutoShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_bulletIntervale);

            Vector3 targetPos = RandomVector2() * (10f - m_bulletAccuracy);
            targetPos += m_playerController.m_enemyTrs.position;
            Vector3 dir = targetPos - m_shootOrigin.position;
            //Debug.Log(dir.normalized);
            m_bulletManager.Shoot(m_shootOrigin, dir.normalized);
        }
    }

    private Vector2 RandomVector2()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        return new Vector2(x, y);
    }
}
