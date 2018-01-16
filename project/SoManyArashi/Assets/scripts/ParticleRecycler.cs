using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ParticleRecycler : MonoBehaviour
{
    ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ps.isStopped)
        {
            if (GetComponent<ObjectPoolRestorer>() != null)
            {
                gameObject.SetActive(false);
            }else
            {
                Destroy(gameObject);
            }
        }
    }
}
