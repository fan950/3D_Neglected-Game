using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class Model : MonoBehaviour
{
    protected struct Model_State_Common
    {
        public int nHp;
        public int nHp_Max;

        public int nAttack;
        public float fAttack_Speed;
        public eAttack_Type eAttack_Type;
        public float fAttack_Distance;
    }
    [HideInInspector] public int nIndex;
    protected GameObject targetObj;
    protected NavMeshAgent navMeshAgent;
    public bool bMove_Stop
    {
        get
        {
            if (navMeshAgent.isOnNavMesh == false)
            {
                return false;
            }
            return navMeshAgent.isStopped;
        }
    }

    public GameObject Get_TargetObj { get { return targetObj; } }
    //FSM
    [HideInInspector] public eFsm_State current_State;
    protected Dictionary<eFsm_State, FSM> dicFsm = new Dictionary<eFsm_State, FSM>();
    [HideInInspector] public Animator animator;
    private string sPlay_AniName;
    //State
    protected Model_State_Common model_State_Common;

    public virtual int nHp_Max { get { return model_State_Common.nHp_Max; } set { model_State_Common.nHp_Max = value; } }
    public float fAttack_Speed { get { return model_State_Common.fAttack_Speed; } set { model_State_Common.fAttack_Speed = value; } }
    public virtual int nAttack { get { return model_State_Common.nAttack; } }
    public virtual int nHp { get { return model_State_Common.nHp; } set { model_State_Common.nHp = value; } }
    public eAttack_Type eAttack_Type { get { return model_State_Common.eAttack_Type; } }
    protected const float fRotate_Speed = 600;
    [HideInInspector] public float fAttack_Time = 0;
    [HideInInspector] public bool bAttack = false;

    protected const string sAttack_Type = "Attack_Type";
    public virtual void Init()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        Active_Nav(true);
    }
    public void Active_Nav(bool bActive)
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = bActive;
    }
    public void Set_FSM(eFsm_State eState)
    {
        if (current_State == eState)
            return;

        dicFsm[current_State].End_FSM(this);
        dicFsm[eState].Start_FSM(this);
        current_State = eState;
    }

    public virtual void Update_Model()
    {
        if (dicFsm.ContainsKey(current_State))
            dicFsm[current_State].Update_FSM(this);
    }

    public virtual void Idle()
    {
        if (targetObj != null)
        {
            Distance_In_Target(delegate { Set_FSM(eFsm_State.Move); }, delegate { Set_FSM(eFsm_State.Attack); });
        }
    }
    public virtual void Die() { }
    public virtual void Move()
    {
        if (targetObj == null)
        {
            Set_FSM(eFsm_State.Idle);
            return;
        }
        Distance_In_Target(delegate
        {
            if (navMeshAgent.isOnNavMesh == false)
            {
                nHp = 0;
                return;
            }
            navMeshAgent.SetDestination(targetObj.transform.position);
        },
        delegate
        {
            Set_FSM(eFsm_State.Attack);
            Nav_Stop(true);
        }
        );
    }
    public virtual void Nav_Stop(bool bStop)
    {
        if (navMeshAgent.isOnNavMesh == false)
        {
            nHp = 0;
            return;
        }

        if (navMeshAgent.isStopped == bStop)
            return;

        if (bStop)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
        }
        else
        {
            navMeshAgent.ResetPath();
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = true;
        }
        navMeshAgent.isStopped = bStop;
    }
    #region Target
    public virtual void Set_Target(GameObject obj)
    {
        targetObj = obj;
    }

    public void Distance_In_Target(Action outAction, Action inAction)
    {
        if (targetObj == null || nHp <= 0)
        {
            Set_FSM(eFsm_State.Idle);
            fAttack_Time = 0;
            bAttack = false;
            return;
        }

        LookAt_Target(targetObj);

        float _fDistance = (targetObj.transform.position - transform.position).sqrMagnitude;
        if (_fDistance > Math.Pow(model_State_Common.fAttack_Distance, 2))
        {
            if (outAction != null)
                outAction();
        }
        else
        {
            if (inAction != null)
                inAction();
        }
    }
    public void LookAt_Target(GameObject target)
    {
        Vector3 _direction = target.transform.position - transform.position;
        Quaternion _targetRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, fRotate_Speed * Time.deltaTime);
    }
    #endregion
    #region Attack
    public virtual void Attack()
    {
        Distance_In_Target(delegate { Set_FSM(eFsm_State.Idle); }, delegate { Attack_Action(); });
    }
    public virtual void Attack_Action(string sName = "Attack") { }
    #endregion
    public void Play_AniTrigger(string sName)
    {
        if (sPlay_AniName == sName)
            return;
        animator.ResetTrigger(sPlay_AniName);
        animator.SetTrigger(sName);
        sPlay_AniName = sName;
    }
}
