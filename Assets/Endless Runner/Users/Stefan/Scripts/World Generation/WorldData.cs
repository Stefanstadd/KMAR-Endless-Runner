[System.Serializable]
public struct WorldData
{
    public WorldNode head;

    public WorldNode tail;

    public int seed;
    public int nodeCount;

    public WorldData ( WorldNode head, int seed )
    {
        this.head = head;
        tail = WorldNode.Tail (head);

        this.seed = seed;
        nodeCount = WorldNode.Count (head);
    }

    public WorldData ( WorldNode head, int seed, int nodeCount )
    {
        this.head = head;
        tail = WorldNode.Tail (head);

        this.seed = seed;
        this.nodeCount = nodeCount;
    }
}