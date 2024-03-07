using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class NodeSampler : MonoBehaviour
{
    [SerializeField]
    protected Vector3 offset;

    private Queue<WorldNode> nodeQueue = new();

    private Queue<Vector3> path = new ( );

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
}
