using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    public ParticleSystemManager instance;

    public GameObject heartProofPrefab;


    private Queue<GameObject> queue;

    private void Awake()
    {
        if (instance == null)
        { 
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public GameObject GenerateParticleSystem(string name)
    {
        if (name == "heartProof")
        {
            return Instantiate(heartProofPrefab);
        }
        return Instantiate(heartProofPrefab);
    }
}
