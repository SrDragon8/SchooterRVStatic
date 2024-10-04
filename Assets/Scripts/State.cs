using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE,
        PATROL,
        CHASE,
        ATTACK,
        DEAD
    };

    public enum EVENT // This is an enum, it is a list of named integer constants
    {
        ENTER,
        UPDATE,
        EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;

    float visDist = 10.0f;
    float visAngle = 30.0f;
    float shootDist = 3.0f;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) // This is the constructor, it will be called when the state is created
    {
        npc = _npc; // Set the npc to the _npc
        anim = _anim;
        player = _player;
        stage = EVENT.ENTER;
        nextState = null;
    }

    public virtual void Enter() // This is the Enter method, it will be called when the state is started
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Update() // This is the Update method, it will be called every frame
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Exit() // This is the Exit method, it will be called when the state is done
    {
        stage = EVENT.EXIT;
    }

    public State Process() // This is the state machine, it will call the Enter, Update and Exit methods
    {
        if (stage == EVENT.ENTER) Enter(); // If the stage is ENTER, call the Enter method
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this; // Return the current state
    }
} 
