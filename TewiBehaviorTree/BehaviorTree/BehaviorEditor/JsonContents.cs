using System;
using System.Collections.Generic;

namespace BahaviorTree.Editor
{
    [Serializable]
    public class JsonTree
    {
        public string version;
        public string scope;
        public string id;
        public string title;
        public string description;
        public string root;
        public Dictionary<string, int?> properties;
        public Dictionary<string, JsonNode> nodes;
    }

    [Serializable]
    public class JsonNode
    {
        public int depth = 0;
        public string id;
        public string name;
        public string category;
        public string title;
        public string description;
        public Dictionary<string, int?> properties;
        public Dictionary<string, int?> display;
        public List<string> children;
        public string child;
    }
}
