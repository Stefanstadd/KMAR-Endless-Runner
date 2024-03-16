using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class NodeSampler : MonoBehaviour
{
    const float LANE_SIZE = 1.8f;

    [SerializeField]
    protected Vector3 offset;

    public Vector3 CurrentLanePosition
    {
        get; private set;
    }
    

    int m_currentLane = 0;

    public int CurrentLane
    {
        get
        {
            return m_currentLane;
        }
        private set
        {
            if ( value == m_currentLane )
                return;
            m_currentLane = value;

            OnCurrentLaneSwitched ( );
        }
    }

    //Private Variables

    #region Nodes Variables
    private WorldNode m_currentNode;

    private WorldNode m_nextNode;

    public WorldNode CurrentNode
    {
        get
        {
            return m_currentNode;
        }
        private set
        {
            if ( value == m_currentNode )
                return;
            m_currentNode = value;

            OnCurrentNodeChanged ( );
        }
    }

    public WorldNode NextNode
    {
        get
        {
            return m_nextNode;
        }
        private set
        {
            m_nextNode = value;
        }
    }

    protected Vector3 SamplePosition
    {
        get
        {
            return transform.position + offset;
        }
    }

    #endregion


    //Logic

    #region Node Movement

    public void SetCurrentNode(WorldNode node )
    {
        if(node == null)
        {
            Debug.Log ("Node is null");
            return;
        }

        CurrentNode = node;
        NextNode = CurrentNode.next;
    }

    protected void MoveToNextNode ( )
    {
        CurrentNode = CurrentNode.next;

        NextNode = CurrentNode.next;
    }

    protected virtual void OnCurrentNodeChanged ( )
    {

    }
    #endregion

    #region Lane Movement
    public void MoveToLaneRight ( )
    {
        if(CurrentLane < 1 )
        {
            CurrentLane++;
        }
    }

    public void MoveToLaneLeft ( )
    {
        if(CurrentLane > -1 )
        {
            CurrentLane -- ;
        }
    }
    protected virtual void OnCurrentLaneSwitched ( )
    {
        CurrentLanePosition = new (LANE_SIZE * CurrentLane, 0, 0);
    }

    #endregion
}
