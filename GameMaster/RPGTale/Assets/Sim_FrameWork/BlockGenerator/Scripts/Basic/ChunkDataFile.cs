using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Sim_FrameWork
{
    public class ChunkDataFile : MonoBehaviour
    {
        public static bool SavingChunks;
        public static Dictionary<string, string> TempChunkData;
        public static Dictionary<string, string[]> LoadedRegions;


        public void SaveData()
        {
            Chunk chunk = UIUtility.SafeGetComponent<Chunk>(transform);
            string Data = CompressData(chunk);
            //TODO
        }

        public static IEnumerator SaveAllChunks()
        {
            if (!MapGenerator.SaveBlockData)
            {
                Debug.LogWarning("Saveing is disabled");
                yield break;
            }
            while (SavingChunks)
            {
                yield return new WaitForEndOfFrame();
            }
            SavingChunks = true;

            var count = 0;
            List<Chunk> chunksToSave = new List<Chunk>(ChunkManager.Chunks.Values);

            foreach(var chunk in chunksToSave)
            {
                UIUtility.SafeGetComponent<ChunkDataFile>(chunk.gameObject.transform).SaveData();
                count++;
                if (count > MapGenerator.MaxChunkSaves)
                {
                    yield return new WaitForEndOfFrame();
                    count = 0;
                }
            }

            //WriteData  TODO

        }

        //压缩信息
        public static string CompressData(Chunk chunk)
        {
            StringWriter writer = new StringWriter();
            var i = 0;
            var length = chunk.GetDataLength();

            ushort currentCount = 0;
            ushort currentData = 0;

            for(i = 0; i < length; i++)
            {
                var Data = chunk.GetBlockSimpleData(i);
                if (Data != currentData)
                {
                    if (i != 0)
                    {
                        writer.Write((char)currentCount);
                        writer.Write((char)currentData);
                    }
                    currentCount = 1;
                    currentData = Data;
                }
                else
                {
                    currentCount++;
                }
                if (i == length - 1)
                {
                    writer.Write((char)currentCount);
                    writer.Write((char)currentData);
                }
            }
            string compressedData = writer.ToString();
            writer.Flush();
            writer.Close();
            return compressedData;
        }

    }
}

