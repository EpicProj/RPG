using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Sim_FrameWork
{
    public class BlockEditor : EditorWindow
    {
        private GameObject SelectedBlock;
        private GameObject SelectedPrefab;

        private GameObject[] Blocks;

        private GameObject Instance;
        private bool BlocksGotten, ReplaceBlockDialog;
        private Vector2 BlockListScroll, BlockEditorScroll;
        private ushort LastID;
        //Texture
        private Texture TextureSheet;

        private bool FindMapGenerator()
        {
            foreach(var obj in Object.FindObjectsOfType<MapGenerator>())
            {
                if (obj != null)
                {
                    MapGenerator generator = obj as MapGenerator;
                    Instance = generator.gameObject;
                    return true;
                }
            }
            return false;
        }

        [MenuItem("SimPro/Map/BlockEditor")]
        private static void Init()
        {
            BlockEditor window = (BlockEditor)GetWindow(typeof(BlockEditor));
            window.Show();
        }

        public void OnGUI()
        {
            if (Instance == null)
            {
                if (FindMapGenerator() == false)
                {
                    EditorGUILayout.LabelField("MapGenerator not found");
                    return;
                }
            }
            if (!BlocksGotten)
            {
                GetBlocks();
            }

            GUILayout.Space(10);
            MapGenerator generator = Instance.GetComponent<MapGenerator>();
            generator.m_BlockPath = EditorGUILayout.TextField("BlockPath", generator.m_BlockPath);
            if (GUI.changed)
            {
                PrefabUtility.ReplacePrefab(generator.gameObject, PrefabUtility.GetPrefabParent(generator.gameObject), ReplacePrefabOptions.ConnectToPrefab);
            }
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.Width(190));

            GUILayout.Space(10);

            //new Block
            if (GUILayout.Button("NewBlock", GUILayout.Width(145), GUILayout.Height(30)))
            {
                CreateBlock();
            }
            GUILayout.Space(10);

            BlockListScroll = EditorGUILayout.BeginScrollView(BlockListScroll);
            int i = 0;
            int lastBtn = 0;
            foreach(var block in Blocks)
            {
                if (block != null)
                {
                    if(i-1 != lastBtn)
                    {
                        GUILayout.Space(10);
                    }
                    lastBtn = i;

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(i.ToString());

                    BaseBlock baseBlock = block.GetComponent<BaseBlock>();

                    //Select btn
                    if(SelectedPrefab!=null && block.name == SelectedPrefab.name)
                    {
                        GUILayout.Box(baseBlock.BlockName, GUILayout.Width(140));
                    }else if (GUILayout.Button(baseBlock.BlockName, GUILayout.Width(140)))
                    {
                        SelectBlock(block);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                i++;
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            //BlockEditor
            EditorGUILayout.BeginVertical();
            BlockEditorScroll = EditorGUILayout.BeginScrollView(BlockListScroll);
            GUILayout.Space(20);
            if (SelectedBlock == null)
            {
                EditorGUILayout.LabelField("Block Selected");
            }

            if (SelectedBlock != null)
            {
                BaseBlock selecedBlock = SelectedBlock.GetComponent<BaseBlock>();
                selecedBlock.BlockName = EditorGUILayout.TextField("Name", selecedBlock.BlockName);

                selecedBlock.SetID((ushort)EditorGUILayout.IntField("ID", selecedBlock.GetID()));

                GUILayout.Space(10);

                // mesh
                selecedBlock.m_CustomMesh = EditorGUILayout.Toggle("Custom mesh", selecedBlock.m_CustomMesh);
                selecedBlock.mesh = (Mesh)EditorGUILayout.ObjectField("Mesh", selecedBlock.mesh, typeof(Mesh), false);

                //texture
                if (selecedBlock.m_CustomMesh == false)
                {
                    if (selecedBlock.m_Texture.Length < 0)
                    {
                        selecedBlock.m_Texture = new Vector2[6];
                    }
                    selecedBlock.m_CuntomSides = EditorGUILayout.Toggle("Define side textures", selecedBlock.m_CuntomSides);

                    if (selecedBlock.m_CuntomSides)
                    {
                        selecedBlock.m_Texture[0] = EditorGUILayout.Vector2Field("Top ", selecedBlock.m_Texture[0]);
                        selecedBlock.m_Texture[1] = EditorGUILayout.Vector2Field("Bottom ", selecedBlock.m_Texture[1]);
                        selecedBlock.m_Texture[2] = EditorGUILayout.Vector2Field("Right ", selecedBlock.m_Texture[2]);
                        selecedBlock.m_Texture[3] = EditorGUILayout.Vector2Field("Left ", selecedBlock.m_Texture[3]);
                        selecedBlock.m_Texture[4] = EditorGUILayout.Vector2Field("Forward ", selecedBlock.m_Texture[4]);
                        selecedBlock.m_Texture[5] = EditorGUILayout.Vector2Field("Back ", selecedBlock.m_Texture[5]);
                    }
                    else
                    {
                        selecedBlock.m_Texture[0] = EditorGUILayout.Vector2Field("Texture ", selecedBlock.m_Texture[0]);
                    }

                }
                else
                {
                    selecedBlock.m_Rotation = (MeshRotation)EditorGUILayout.EnumPopup("Mesh Rotation", selecedBlock.m_Rotation);
                }

                GUILayout.Space(10);

                //Material
                selecedBlock.m_submeshIndex = EditorGUILayout.IntField("Material index", selecedBlock.m_submeshIndex);
                if (selecedBlock.m_submeshIndex < 0)
                {
                    selecedBlock.m_submeshIndex = 0;
                }

                // transparency
                selecedBlock.m_Transparency = (Transparency)EditorGUILayout.EnumPopup("Transparency", selecedBlock.m_Transparency);

                // collision
                selecedBlock.m_ColliderType = (ColliderType)EditorGUILayout.EnumPopup("Collider", selecedBlock.m_ColliderType);

                GUILayout.Space(10);

                // components
                GUILayout.Label("Components");
                foreach (Object component in selecedBlock.GetComponents<Component>())
                {
                    if (component is Transform == false && component is BaseBlock == false)
                    {
                        GUILayout.Label(component.GetType().ToString());
                    }

                }
                GUILayout.Space(20);

                // apply
                if (GUILayout.Button("Apply", GUILayout.Height(20),GUILayout.Width(140)))
                {

                    if (SelectedPrefab != null && SelectedPrefab.GetComponent<BaseBlock>().GetID() != selecedBlock.GetID() && GetBlock(selecedBlock.GetID()) != null)
                    {
                        ReplaceBlockDialog = true;
                    }
                    else
                    {
                        ReplaceBlockDialog = false;
                        UpdateBlock();
                        ApplyBlocks();
                        GetBlocks();
                    }

                }
                if (ReplaceBlockDialog)
                {
                    GUILayout.Label("A block with this ID already exists!" + SelectedPrefab.GetComponent<BaseBlock>().GetID() + selecedBlock.GetID());
                }

            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

        }
        public void OnDestory()
        {
            DestroyImmediate(SelectedBlock);
        }
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.G))
            {
                CreateBlock();
                UpdateBlock();
                ApplyBlocks();
            }

        }

        #region Method
        private string GetPath()
        {
            try
            {
                return Instance.GetComponent<MapGenerator>().m_BlockPath;
            }
            catch(System.Exception)
            {
                Debug.LogError("MapGenerator not found");
                return null;
            }
        }

        private string GetBlockPath(ushort data)
        {
            return GetPath() + "Block_" + data + ".prefab";
        }
        private string GetPrefabPath(GameObject block)
        {
            return GetPath() + block.name.Split("("[0])[0] + ".prefab";
        }

        private void GetBlocks()
        {
            Blocks = new GameObject[ushort.MaxValue];
            for(ushort i = 0; i < ushort.MaxValue; i++)
            {
                GameObject block = GetBlock(i);
                if (block != null)
                {
                    Blocks[i] = block;
                    LastID = i;
                }
            }
            BlocksGotten = true;
        }
        
        private GameObject GetBlock(ushort data)
        {
            Object blockObj = AssetDatabase.LoadAssetAtPath(GetBlockPath(data), typeof(object));
            GameObject block = null;
            if (blockObj != null)
            {
                block = (GameObject)blockObj;
            }
            else
            {
                return null;
            }

            if(block!=null && block.GetComponent<BaseBlock>() != null)
            {
                return block;
            }
            else
            {
                return null;
            }
        }

        private void SelectBlock(GameObject block)
        {
            DestroyImmediate(SelectedBlock);
            try
            {
                SelectedBlock = Instantiate(block, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            }
            catch (System.Exception)
            {

            }
            SelectedPrefab = block;
            SelectedBlock.name = block.name;
            Selection.objects = new Object[] { SelectedBlock };
        }

        private void CreateBlock()
        {
            SelectBlock(GetBlock(0));
            SelectedBlock.name = "Block_" + (LastID + 1);
            SelectedPrefab = null;
            ReplaceBlockDialog = false;

            UpdateBlock();
            ApplyBlocks();
            GetBlocks();
        }

        private void UpdateBlock()
        {
            SelectedBlock.name = "Block_" + SelectedBlock.GetComponent<BaseBlock>().GetID();
            GameObject newBlock = PrefabUtility.CreatePrefab(GetPrefabPath(SelectedBlock), SelectedBlock);
            SelectedPrefab = newBlock;
        }

        private void ApplyBlocks()
        {
            List<GameObject> blocks = new List<GameObject>();
            int empty = 0;
            for(ushort i = 0; i < ushort.MaxValue; i++)
            {
                Object block = AssetDatabase.LoadAssetAtPath(GetBlockPath(i), typeof(Object));
                if (block != null)
                {
                    while (empty > 0)
                    {
                        blocks.Add(null);
                        empty--;
                    }
                    blocks.Add((GameObject)block);
                }
                else
                {
                    empty++;
                }
            }

            MapGenerator generator = Instance.GetComponent<MapGenerator>();
            generator.m_Blocks = blocks.ToArray();
            PrefabUtility.ReplacePrefab(generator.gameObject, PrefabUtility.GetPrefabParent(generator.gameObject), ReplacePrefabOptions.ConnectToPrefab);
        }

        private void SaveBlocks()
        {
            UpdateBlock();
            ApplyBlocks();
        }


        #endregion
    }
}