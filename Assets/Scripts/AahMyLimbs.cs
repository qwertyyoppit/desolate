using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AahMyLimbs : MonoBehaviour
{
    [SerializeField] private GameObject torso;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject larm;
    [SerializeField] private GameObject rarm;
    [SerializeField] private GameObject lleg;
    [SerializeField] private GameObject rleg;
    
    [NonSerialized] public BodyPart TorsoPart;
    [NonSerialized] public BodyPart HeadPart;
    [NonSerialized] public BodyPart LeftArmPart;
    [NonSerialized] public BodyPart RightArmPart;
    [NonSerialized] public BodyPart LeftLegPart;
    [NonSerialized] public BodyPart RightLegPart;

    public BodyPart[] Parts => new BodyPart[] { TorsoPart, HeadPart, LeftArmPart, RightArmPart, LeftLegPart, RightLegPart };

    private PlayerProjectileManager projectileManager;

    private void Awake()
    {
        TorsoPart = new BodyPart(torso, torso.GetComponent<BoxCollider>(), torso.GetComponent<Rigidbody>());
        HeadPart = new BodyPart(torso, head.GetComponent<BoxCollider>(), head.GetComponent<Rigidbody>());
        LeftArmPart = new BodyPart(torso, larm.GetComponent<BoxCollider>(), larm.GetComponent<Rigidbody>());
        RightArmPart = new BodyPart(torso, rarm.GetComponent<BoxCollider>(), rarm.GetComponent<Rigidbody>());
        LeftLegPart = new BodyPart(torso, lleg.GetComponent<BoxCollider>(), lleg.GetComponent<Rigidbody>());
        RightLegPart = new BodyPart(torso, rleg.GetComponent<BoxCollider>(), rleg.GetComponent<Rigidbody>());
    }

    private void Start()
    {
        

        projectileManager = Refs.PlayerProjectileManager;

        projectileManager.projectileHit += OnHit;
    }

    private void OnHit(Projectile proj)
    {
        if (proj.HitInfo.collider == TorsoPart.Box)
        {
            foreach (var part in Parts)
            {
                if (part.Box.gameObject == torso) continue;
                part.Rb.constraints = RigidbodyConstraints.None;
                part.Rb.linearVelocity = Vector3.zero;
                part.Rb.AddForce(proj.transform.forward * proj.Speed, ForceMode.Impulse);
            }
        }
    }
}

public class BodyPart
{
    public GameObject Obj;
    public Rigidbody Rb;
    public BoxCollider Box;

    public BodyPart(GameObject obj, BoxCollider box, Rigidbody rb)
    {
        Obj = obj;
        Rb = rb;
        Box = box;
    }
}