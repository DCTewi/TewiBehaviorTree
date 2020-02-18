using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BahaviorTree.Editor
{
    public class BehaviorWindow : EditorWindow
    {
        private string behaviorJSON;
        private string behaviorCode;
        private string typeName = "Player";

        private bool hasException = false;
        private string exceptionMessage;
        private Vector2 jsonScroll;
        private Vector2 codeScroll;


        [MenuItem("Window/AI/Behavior CodeGen")]
        public static void GetWindow()
        {
            var window = GetWindow<BehaviorWindow>();
            window.titleContent = new GUIContent("Behavior CodeGen");
            window.minSize = new Vector2(400, 300);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Behavior3 Tree JSON", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("You can get behavior3editor desktop at \"github.com/magicsea/behavior3go\"");

            jsonScroll = EditorGUILayout.BeginScrollView(jsonScroll, GUILayout.MaxHeight(position.height * 0.3f));
            behaviorJSON = EditorGUILayout.TextArea(behaviorJSON,GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Generated Code", EditorStyles.boldLabel);

            codeScroll = EditorGUILayout.BeginScrollView(codeScroll, GUILayout.MaxHeight(position.height * 0.45f));
            _ = EditorGUILayout.TextArea(behaviorCode,GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            if (hasException)
            {
                EditorGUILayout.HelpBox(exceptionMessage, MessageType.Warning);
            }

            typeName = EditorGUILayout.TextField("Type Name:", typeName);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();

            if (GUILayout.Button("Gen!"))
            {
                hasException = false;
                behaviorCode = "Generating...";
                Repaint();
                GenerateCode();
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }

        private void GenerateCode()
        {
            try
            {
                //var data = JsonUtility.FromJson<Serialization<string, object>>(behaviorJSON).ToDictionary();

                var data = JsonConvert.DeserializeObject<JsonTree>(behaviorJSON);

                if (data.scope != "tree")
                {
                    throw new System.ArgumentException("The JSON inputed isn't from behavior3 tree json scope!");
                }

                var nodes = data.nodes;

                GetDepthOf(nodes[data.root], ref data);
                behaviorCode = $"new BehaviorTreeBuilder<{typeName}>(this)\n";

                GenCode(data.nodes[data.root], ref data);
            }
            catch (System.Exception e)
            {
                hasException = true;
                exceptionMessage = e.Message;
            }
        }

        private void GetDepthOf(JsonNode u, ref JsonTree t)
        {
            if (u.child != null) // Leaf child
            {
                t.nodes[u.child].depth = u.depth + 1;
            }
            else if (u.children != null && u.children.Count > 0)
            {
                foreach (var i in u.children)
                {
                    t.nodes[i].depth = u.depth + 1;
                    GetDepthOf(t.nodes[i], ref t);
                }
            }
        }

        private void GenCode(JsonNode now, ref JsonTree t)
        {
            int flagofchild = 0;
            Node u = new Node(now, typeName);
            behaviorCode += new string(' ', (u.Depth + 1) * 4) + '.' + u.Name + '(';
            if (u.Args.Count > 0)
            {
                for (int i = 0; i < u.Args.Count; i++)
                {
                    behaviorCode += u.Args[i] + (i == u.Args.Count - 1 ? ")" : ", ");
                }
            }
            else behaviorCode += ")";

            if (!string.IsNullOrEmpty(now.child))
            {
                behaviorCode += "\n"; flagofchild = 1;
                Node v = new Node(t.nodes[now.child], typeName);
                behaviorCode += new string(' ', (v.Depth + 1) * 4) + "." + v.Name + "().Back()\n";
            }
            else if (now.children != null && now.children.Count > 0)
            {
                behaviorCode += "\n"; flagofchild = 1;
                for (int i = 0; i < now.children.Count; i++)
                {
                    GenCode(t.nodes[now.children[i]], ref t);
                }
            }

            if (t.root == now.id)
            {
                behaviorCode += "    .End();\n";
            }
            else
            {
                if (flagofchild == 1)
                {
                    behaviorCode += new string(' ', (u.Depth + 1) * 4);
                }
                behaviorCode += ".Back()\n";
            }
        }

        private enum NodeType : short
        {
            Root = 0,
            Selector,
            Sequence,
            Parallel,
            Decorator,
            Condition,
            Action,
        }

        private struct Node
        {
            public string Name;
            public NodeType Type;
            public List<string> Args;
            public int Depth;

            public Node(string name, NodeType type, int depth)
            {
                Name = name; Type = type; Depth = depth; Args = new List<string>();
            }

            public Node(JsonNode node, string typeName)
            {
                Depth = node.depth;
                Args = new List<string>();
                switch (node.category)
                {
                    case "composite":
                        {
                            var name = node.name as string;
                            if (name == "Priority")
                            {
                                Name = "Selector"; Type = NodeType.Selector;
                            }
                            else if (name == "Sequence")
                            {
                                Name = "Sequence"; Type = NodeType.Sequence;
                            }
                            else if (name == "Parallel")
                            {
                                Name = "Parallel"; Type = NodeType.Parallel;
                            }
                            else
                            {
                                throw new System.ArgumentException($"Unexpected note name \"{name}\" in node {node.id}");
                            }
                        }
                        break;
                    case "decorator":
                        {
                            Type = NodeType.Decorator;
                            var name = node.name;
                            if (name == "Inverter")
                            {
                                Name = $"Decorator<InverterNode<{typeName}>>";
                            }
                            else if (name == "Repeater")
                            {
                                Name = $"Decorator<RepeatNode<{typeName}>> ";
                                string limit = node.properties["maxLoop"]?.ToString();
                                if (string.IsNullOrEmpty(limit) || limit == "-1")
                                {
                                    throw new System.ArgumentException($"Repeater {node.id} is unlimited.");
                                }
                                Args.Add(limit);
                            }
                            else if (name == "RepeatUntilSuccess")
                            {
                                Name = $"Decorator<UntilNode<{typeName}>>";
                                string limit = node.properties["maxLoop"]?.ToString();
                                if (!string.IsNullOrEmpty(limit) && limit != "-1")
                                {
                                    Args.Add(limit);
                                }
                            }
                            else
                            {
                                Name = $"Decorator<{name}<{typeName}>>";
                                var args = node.properties;
                                foreach (var pair in args)
                                {
                                    if (pair.Value == null)
                                    {
                                        throw new System.ArgumentException($"Null argument at node {node.id}");
                                    }
                                    Args.Add(pair.Value.ToString());
                                }
                            }
                        }
                        break;
                    case "condition":
                        {
                            Name = $"Condition<{node.name}>"; Type = NodeType.Condition;
                        }
                        break;
                    case "action":
                        {
                            Name = $"Action<{node.name}>"; Type = NodeType.Action;
                        }
                        break;
                    default:
                        throw new System.ArgumentException($"Unexpected note category \"{node.category}\"");
                }
            }
        }
    }
}
