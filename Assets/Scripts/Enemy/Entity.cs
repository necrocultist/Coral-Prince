using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
   public FiniteStateMachine stateMachine;

   public D_Entity entityData;
   
   public int facingDirection { get; private set; }
   public Rigidbody2D rb { get; private set; }
   //public Animator anim { get; private set; }
   public GameObject aliveGO { get; private set; }

   [SerializeField]
   private Transform wallCheck;

   [SerializeField] private Transform ledgeCheck;

   private Vector2 velocityWorkspace;

   public virtual void Start()
   {
      aliveGO = transform.Find("Alive").gameObject;
      rb = aliveGO.GetComponent<Rigidbody2D>();
      //anim = aliveGO.GetComponent<Animator>();

      stateMachine = new FiniteStateMachine();
   }

   public virtual void Update()
   {
      stateMachine.currentState.LogicUpdate();
   }

   public virtual void FixedUpdate()
   {
      stateMachine.currentState.PhysicsUpdate();
   }

   public virtual void SetVelocity(float velocity)
   {
      velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
      rb.velocity = velocityWorkspace;
   }

   public virtual bool CheckWall()
   {
      return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance,
         entityData.groundMask);
   }

   public virtual bool CheckLedge()
   {
      return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance,
         entityData.groundMask);
   }
}
