using System.Collections.Generic;
using UnityEngine;

public class NodeSampler : MonoBehaviour
{
    [SerializeField]
    protected Vector3 offset;

    private Queue<WorldNode> nodeQueue = new();

    private Queue<Vector3> path = new ( );

    private WorldNode m_currentNode;

    private WorldNode m_nextNode;

    protected WorldNode CurrentNode
    {
        get
        {
            return m_currentNode;
        }
        set
        {
            if ( value == m_currentNode )
                return;
            m_currentNode = value;

            OnCurrentNodeChanged ( );
        }
    }

    protected WorldNode NextNode
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

    protected virtual void OnEnable ( )
    {
        Coms.WorldGenerator.onNodeGenerated += AddNodeToSample;
    }

    protected virtual void OnDisable ( )
    {
        Coms.WorldGenerator.onNodeGenerated -= AddNodeToSample;
    }

    protected virtual void Update ( )
    {
        if ( CurrentNode == null )
        {
            if( nodeQueue.Count == 0 )
            {
                return;
            }
            MoveToNextNode ( );
        }
    }


    protected void MoveToNextNode ( )
    {
        CurrentNode = nodeQueue.Dequeue ( );

        if ( nodeQueue.TryPeek (out WorldNode next) )
            NextNode = next;
        else
            NextNode = null;
    }

    protected virtual void OnCurrentNodeChanged ( )
    {

    }

    void AddNodeToSample(WorldNode node )
    {
        nodeQueue.Enqueue( node );
    }
}
