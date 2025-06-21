using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestClasses
{

    [Serializable]
    public class TestNode : TestNodeBase
    {
        [SerializeField] private int id;
        [SerializeField] private int cost;
        [SerializeField] private int level;
        [SerializeField] private int type;
        public override int Id { get { return id; } set { id = value; } }
        public override int Cost { get { return cost; } set { cost = value; } }
        public override int Level { get { return level; } set { level = value; } }
        public override int Type { get { return type; } set { type = value; } }
    }

    [Serializable]
    public abstract class TestNodeBase
    {
        public abstract int Id { get; set; }
        public abstract int Cost { get; set; }
        public abstract int Level { get; set; }
        public abstract int Type { get; set; }
    }

    [Serializable]
    public class TestMap
    {
        [SerializeReference] private TestNodeBase[] nodes;
        public TestNodeBase[] Nodes { get { return nodes; } set { nodes = value; } }
    }
}

// can use [field: SerializeField] (on property) which is functionally same as [SerializeField] private int Id (on field); - [field: SerializeField] allows you to serialize properties directly without needing a backing field
//prior to this I was using [SerializeField] on properties, which requires backing fields but I didn't know that at the time.

//[SerializeField] does not support polymorphism - instead use [SerializeReference]
//[SerializeReference] allows to serialize fields with an interface type, or an abstract class that is not a Unity.Object, both being impossible to do with SerializeField.
//https://www.reddit.com/r/Unity3D/comments/14y0c1q/serializereference_is_very_powerfull_why_is_no/