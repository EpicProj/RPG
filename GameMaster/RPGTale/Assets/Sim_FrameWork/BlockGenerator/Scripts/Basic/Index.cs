using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class Index
    {

        public int x, y, z;

        public Index(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Index(Vector3 setIndex)
        {
            this.x = (int)setIndex.x;
            this.y = (int)setIndex.y;
            this.z = (int)setIndex.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public override string ToString()
        {
            return (x.ToString() + "," + y.ToString() + "," + z.ToString());
        }

        //Get nearby Index
        public Index GetNearbyChunkIndex(Direction direction)
        {
            if (direction == Direction.left)
                return new Index(x - 1, y, z);
            else if (direction == Direction.right)
                return new Index(x + 1, y, z);
            else if (direction == Direction.back)
                return new Index(x, y, z - 1);
            else if (direction == Direction.forward)
                return new Index(x, y, z + 1);
            else return null;
   
        }
        public static Index FromString(string index)
        {
            string[] splitString = index.Split(',');
            try
            {
                return new Index(int.Parse(splitString[0]), int.Parse(splitString[1]), int.Parse(splitString[2]));
            }
            catch (System.Exception)
            {
                Debug.LogError("Blocks:Formate Fail, index="+index);
                return null;
            }
        }

        public bool IsEqual(Index index)
        {
            if (index == null)
                return false;
            if (this.x == index.x && this.y == index.y && this.z == index.z)
                return true;
            else return false;
        }
    }
}