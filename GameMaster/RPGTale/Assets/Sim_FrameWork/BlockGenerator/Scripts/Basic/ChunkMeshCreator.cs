using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public enum MeshRotation
    {
        none,
        back,
        left,
        right
    }
    public class ChunkMeshCreator : MonoBehaviour
    {
        private Chunk chunk;
        private int SideLength;
        private GameObject noCollideCollider;

        //mesh data
        private List<Vector3> Vertices = new List<Vector3>();
        private List<List<int>> Faces = new List<List<int>>();
        private List<Vector2> UVs = new List<Vector2>();
        private int FaceCount;

        //collider data
        private List<Vector3> SolidColliderVertices = new List<Vector3>();
        private List<int> SolidColiiderFaces = new List<int>();
        private int SolidFaceCount;

        private List<Vector3> NoColliderVertices = new List<Vector3>();
        private List<int> NocolliderFaces = new List<int>();
        private int NoCollideFaceCount;

        public Mesh cube;

        private bool inited;

        public void Init()
        {
            chunk = GetComponent<Chunk>();
            SideLength = chunk.SideLength;
            for(var i = 0; i < GetComponent<Renderer>().materials.Length; i++)
            {
                Faces.Add(new List<int>());
            }
            inited = true;
        }

        #region Generate Mesh

        public void RebuildMesh()
        {
            if (!inited)
            {
                Init();
            }
            foreach(Transform trans in transform)
            {
                Destroy(trans.gameObject);
            }
            int x = 0, y = 0, z = 0;
            chunk.GetNearbyChunks();
            while (x < SideLength)
            {
                while (y < SideLength)
                {
                    while (z < SideLength)
                    {
                        var block = chunk.GetBlock(x, y, z);
                        if (block != 0)
                        {
                            var blockType = MapGenerator.GetBlockType(block);
                            if (blockType.m_CustomMesh == false)
                            {
                                Transparency transparency = blockType.m_Transparency;
                                ColliderType colliderType = blockType.m_ColliderType;

                                if (CheckNearbyChunks(x, y, z, Direction.forward, transparency) == true)
                                    CreateFace(block, Facing.forward, colliderType, x, y, z);

                                if (CheckNearbyChunks(x, y, z, Direction.back, transparency) == true)
                                    CreateFace(block, Facing.back, colliderType, x, y, z);

                                if (CheckNearbyChunks(x, y, z, Direction.up, transparency) == true)
                                    CreateFace(block, Facing.up, colliderType, x, y, z);

                                if (CheckNearbyChunks(x, y, z, Direction.down, transparency) == true)
                                    CreateFace(block, Facing.down, colliderType, x, y, z);

                                if (CheckNearbyChunks(x, y, z, Direction.left, transparency) == true)
                                    CreateFace(block, Facing.left, colliderType, x, y, z);

                                if (CheckNearbyChunks(x, y, z, Direction.right, transparency) == true)
                                    CreateFace(block, Facing.right, colliderType, x, y, z);

                                if(colliderType==ColliderType.none&& MapGenerator.GenerateColliders)
                                {
                                    AddCubeMesh(x, y, z, false);
                                }
                                else
                                {
                                    if (CheckAllNearbyChunks(x, y, z) == false)
                                    {
                                        //如果没有任何一个不是不透明的，渲染
                                        CreateCustomMesh(block, x, y, z, blockType.mesh);
                                    }
                                }

                            }
                        }
                        z += 1;
                      
                    }
                    z = 0;
                    y += 1;

                }
                y = 0;
                x += 1;
            }
            UpdateMesh(GetComponent<MeshFilter>().mesh);
        }

        private bool CheckNearbyChunks(int x, int y, int z, Direction direction, Transparency transparency)
        {
            Index index = chunk.GetNearbyChunkIndex(x, y, z, direction);
            ushort nearbyBlock = chunk.GetBlock(index.x, index.y, index.z);
            if (nearbyBlock == ushort.MaxValue)
            {
                //Chunk Missing
                if(MapGenerator.ShowBorderFaces || direction == Direction.up)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            Transparency trans = MapGenerator.GetBlockType(nearbyBlock).m_Transparency;
            if (transparency == Transparency.transparent)
            {
                if (trans == Transparency.transparent)
                {
                    return false;
                }else
                {
                    return true;
                }
            }
            else
            {
                if (trans == Transparency.solid)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool CheckAllNearbyChunks(int x,int y,int z)
        {
            for(int i = 0; i < 6; i++)
            {
                if (MapGenerator.GetBlockType(chunk.GetBlock(chunk.GetNearbyChunkIndex(x, y, z, (Direction)i))).m_Transparency != Transparency.solid)
                {
                    return false;
                }
            }
            return true;
        }

        private void CreateFace(ushort block,Facing facing,ColliderType colliderType,int x,int y,int z)
        {
            BaseBlock baseBlock = MapGenerator.GetBlockType(block);
            List<int> FaceList = Faces[baseBlock.m_submeshIndex];

            if(facing == Facing.forward)
            {
                Vertices.Add(new Vector3(x + 0.5001f, y + 0.5001f, z + 0.5f));
                Vertices.Add(new Vector3(x - 0.5001f, y + 0.5001f, z + 0.5f));
                Vertices.Add(new Vector3(x - 0.5001f, y - 0.5001f, z + 0.5f));
                Vertices.Add(new Vector3(x + 0.5001f, y - 0.5001f, z + 0.5f));
                if (colliderType == ColliderType.cube && MapGenerator.GenerateColliders)
                {
                    SolidColliderVertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                    SolidColliderVertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                    SolidColliderVertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                    SolidColliderVertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                }
                else if (facing == Facing.up)
                {
                    Vertices.Add(new Vector3(x - 0.5001f, y + 0.5f, z + 0.5001f));
                    Vertices.Add(new Vector3(x + 0.5001f, y + 0.5f, z + 0.5001f));
                    Vertices.Add(new Vector3(x + 0.5001f, y + 0.5f, z - 0.5001f));
                    Vertices.Add(new Vector3(x - 0.5001f, y + 0.5f, z - 0.5001f));
                    if (colliderType == ColliderType.cube && MapGenerator.GenerateColliders)
                    {
                        SolidColliderVertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                        SolidColliderVertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                        SolidColliderVertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                        SolidColliderVertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                    }
                }
                else if (facing == Facing.right)
                {
                    Vertices.Add(new Vector3(x + 0.5f, y + 0.5001f, z - 0.5001f));
                    Vertices.Add(new Vector3(x + 0.5f, y + 0.5001f, z + 0.5001f));
                    Vertices.Add(new Vector3(x + 0.5f, y - 0.5001f, z + 0.5001f));
                    Vertices.Add(new Vector3(x + 0.5f, y - 0.5001f, z - 0.5001f));
                    if (colliderType == ColliderType.cube && MapGenerator.GenerateColliders)
                    {
                        SolidColliderVertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                        SolidColliderVertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
                        SolidColliderVertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                        SolidColliderVertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                    }
                }
                else if (facing == Facing.back)
                {
                    Vertices.Add(new Vector3(x - 0.5001f, y + 0.5001f, z - 0.5f));
                    Vertices.Add(new Vector3(x + 0.5001f, y + 0.5001f, z - 0.5f));
                    Vertices.Add(new Vector3(x + 0.5001f, y - 0.5001f, z - 0.5f));
                    Vertices.Add(new Vector3(x - 0.5001f, y - 0.5001f, z - 0.5f));
                    if (colliderType == ColliderType.cube && MapGenerator.GenerateColliders)
                    {
                        SolidColliderVertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                        SolidColliderVertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
                        SolidColliderVertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                        SolidColliderVertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                    }
                }
                else if (facing == Facing.down)
                {
                    Vertices.Add(new Vector3(x - 0.5001f, y - 0.5f, z - 0.5001f));
                    Vertices.Add(new Vector3(x + 0.5001f, y - 0.5f, z - 0.5001f));
                    Vertices.Add(new Vector3(x + 0.5001f, y - 0.5f, z + 0.5001f));
                    Vertices.Add(new Vector3(x - 0.5001f, y - 0.5f, z + 0.5001f));
                    if (colliderType == ColliderType.cube && MapGenerator.GenerateColliders)
                    {
                        SolidColliderVertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                        SolidColliderVertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
                        SolidColliderVertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
                        SolidColliderVertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                    }
                }
                else if (facing == Facing.left)
                {
                    Vertices.Add(new Vector3(x - 0.5f, y + 0.5001f, z + 0.5001f));
                    Vertices.Add(new Vector3(x - 0.5f, y + 0.5001f, z - 0.5001f));
                    Vertices.Add(new Vector3(x - 0.5f, y - 0.5001f, z - 0.5001f));
                    Vertices.Add(new Vector3(x - 0.5f, y - 0.5001f, z + 0.5001f));
                    if (colliderType == ColliderType.cube && MapGenerator.GenerateColliders)
                    {
                        SolidColliderVertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
                        SolidColliderVertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
                        SolidColliderVertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
                        SolidColliderVertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
                    }
                }

                //UV
                float Unit = MapGenerator.TextureUnit;
                Vector2 offset = MapGenerator.GetTextureOffset(block, facing);
                float padding = Unit * MapGenerator.TexturePadding;
                //top left
                UVs.Add(new Vector2(Unit * offset.x + padding, Unit * offset.y + Unit - padding));
                //top right
                UVs.Add(new Vector2(Unit * offset.x + Unit - padding, Unit * offset.y + Unit - padding));
                //buttom left
                UVs.Add(new Vector2(Unit * offset.x + padding, Unit * offset.y + Unit + padding));
                //buttom right
                UVs.Add(new Vector2(Unit * offset.x + Unit - padding, Unit * offset.y + Unit + padding));

                //Add Face
                FaceList.Add(FaceCount + 0);
                FaceList.Add(FaceCount + 1);
                FaceList.Add(FaceCount + 3);
                FaceList.Add(FaceCount + 1);
                FaceList.Add(FaceCount + 2);
                FaceList.Add(FaceCount + 3);
                if(colliderType==ColliderType.cube&& MapGenerator.GenerateColliders)
                {
                    SolidColiiderFaces.Add(SolidFaceCount + 0);
                    SolidColiiderFaces.Add(SolidFaceCount + 1);
                    SolidColiiderFaces.Add(SolidFaceCount + 3);
                    SolidColiiderFaces.Add(SolidFaceCount + 1);
                    SolidColiiderFaces.Add(SolidFaceCount + 2);
                    SolidColiiderFaces.Add(SolidFaceCount + 3);
                }

                //Add Face Count
                FaceCount += 4;
                if (colliderType == ColliderType.cube && MapGenerator.GenerateColliders)
                {
                    SolidFaceCount += 4;
                }
                if (Vertices.Count > ushort.MaxValue - 5)
                {
                    CreateNewMesh();
                }
            }
        }

        private void CreateCustomMesh(ushort block,int x,int y,int z,Mesh mesh)
        {
            BaseBlock baseBlock = MapGenerator.GetBlockType(block);
            List<int> FacesList = Faces[baseBlock.m_submeshIndex];

            if (mesh == null)
            {
                //Debug.LogError();
                return;
            }

            if (Vertices.Count + mesh.vertices.Length > ushort.MaxValue - 1)
            {
                CreateNewMesh();
            }
            List<Vector3> rotatedVertices = new List<Vector3>();
            MeshRotation rotation = baseBlock.m_Rotation;

            //180
            if (rotation == MeshRotation.back)
            {
                foreach(var vertex in mesh.vertices)
                {
                    rotatedVertices.Add(new Vector3(-vertex.x, vertex.y, -vertex.z));
                }
            }
            //90 right
            else if (rotation == MeshRotation.right)
            {
                foreach (var vertex in mesh.vertices)
                {
                    rotatedVertices.Add(new Vector3(vertex.z, vertex.y, -vertex.x));
                }
            }
            //90 left
            else if (rotation == MeshRotation.left)
            {
                foreach (var vertex in mesh.vertices)
                {
                    rotatedVertices.Add(new Vector3(-vertex.z, vertex.y, vertex.x));
                }
            }
            //no 
            else 
            {
                foreach (var vertex in mesh.vertices)
                {
                    rotatedVertices.Add(vertex);
                }
            }

            //vertices
            foreach(var vertex in rotatedVertices)
            {
                Vertices.Add(vertex + new Vector3(x, y, z));
            }

            //UV
            foreach(var uv in mesh.uv)
            {
                UVs.Add(uv);
            }

            //faces
            foreach(int face in mesh.triangles)
            {
                FacesList.Add(FaceCount + face);
            }
            FaceCount += mesh.vertexCount;

            //collider
            if (MapGenerator.GenerateColliders)
            {
                ColliderType colliderType = MapGenerator.GetBlockType(block).m_ColliderType;

                if (colliderType == ColliderType.mesh)
                {
                    foreach(var vertex in rotatedVertices)
                    {
                        SolidColliderVertices.Add(vertex + new Vector3(x, y, z));
                    }
                    foreach(var face in mesh.triangles)
                    {
                        SolidColiiderFaces.Add(SolidFaceCount + face);
                    }
                    SolidFaceCount += mesh.vertexCount;
                }

                //cube collider
                if (colliderType == ColliderType.cube)
                {
                    AddCubeMesh(x, y, z, true);
                }else if (block != 0)
                {
                    AddCubeMesh(x, y, z, false);
                }
            }




        }
       

        private void CreateNewMesh()
        {
            GameObject meshContainer = Instantiate(chunk.MeshContainer, transform.position, transform.rotation) as GameObject;
            meshContainer.transform.parent = this.transform;
            UpdateMesh(meshContainer.GetComponent<MeshFilter>().mesh);
        }

        private void AddCubeMesh(int x ,int y,int z,bool solid)
        {
            if (solid)
            {
                foreach(var vertex in cube.vertices)
                {
                    SolidColliderVertices.Add(vertex + new Vector3(x, y, z));
                }
                foreach(int face in cube.triangles)
                {
                    SolidColiiderFaces.Add(SolidFaceCount + face);
                }
                SolidFaceCount += cube.vertexCount;
            }
            else
            {
                foreach (var vertex in cube.vertices)
                {
                    NoColliderVertices.Add(vertex + new Vector3(x, y, z));
                }
                foreach (int face in cube.triangles)
                {
                    NocolliderFaces.Add(SolidFaceCount + face);
                }
                NoCollideFaceCount += cube.vertexCount;
            }
        }

        /// <summary>
        /// 更新Mesh
        /// </summary>
        /// <param name="mesh"></param>
        private void UpdateMesh(Mesh mesh)
        {
            mesh.Clear();
            mesh.vertices = Vertices.ToArray();
            mesh.subMeshCount = GetComponent<Renderer>().materials.Length;

            for(int i = 0; i < Faces.Count; ++i)
            {
                mesh.SetTriangles(Faces[i].ToArray(), i);
            }

            mesh.uv = UVs.ToArray();
            mesh.RecalculateNormals();
            if (MapGenerator.GenerateColliders)
            {
                Mesh colMesh = new Mesh();
                colMesh.vertices = SolidColliderVertices.ToArray();
                colMesh.triangles = SolidColiiderFaces.ToArray();
                colMesh.RecalculateNormals();

                GetComponent<MeshCollider>().sharedMesh = null;
                GetComponent<MeshCollider>().sharedMesh = colMesh;

                if (NocolliderFaces.Count > 0)
                {
                    Mesh nocolMesh = new Mesh();
                    nocolMesh.vertices = NoColliderVertices.ToArray();
                    nocolMesh.triangles = NocolliderFaces.ToArray();
                    nocolMesh.RecalculateNormals();

                    noCollideCollider = Instantiate(chunk.ChunkCollider, transform.position, transform.rotation) as GameObject;
                    noCollideCollider.transform.parent = this.transform;
                    noCollideCollider.GetComponent<MeshCollider>().sharedMesh = nocolMesh;
                }else if (noCollideCollider != null)
                {
                    Destroy(noCollideCollider);
                }
            }

            Vertices.Clear();
            UVs.Clear();
            foreach(var faceList in Faces)
            {
                faceList.Clear();
            }

            SolidColiiderFaces.Clear();
            SolidColliderVertices.Clear();
            NoColliderVertices.Clear();
            NocolliderFaces.Clear();

            FaceCount = 0;
            SolidFaceCount = 0;
            NoCollideFaceCount = 0;

        }


        #endregion
    }
}
