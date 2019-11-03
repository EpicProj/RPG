using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sim_FrameWork
{
    public class Index
    {

        public int x, y;

        public Index(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Index(Vector2 setIndex)
        {
            this.x = (int)setIndex.x;
            this.y = (int)setIndex.y;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y);
        }

        public override string ToString()
        {
            return (x.ToString() + "," + y.ToString());
        }

        //Get nearby Index
        public Index GetNearbyChunkIndex(Direction direction)
        {
            if (direction == Direction.down)
                return new Index(x, y - 1);
            else if (direction == Direction.up)
                return new Index(x, y + 1);
            else if (direction == Direction.left)
                return new Index(x - 1, y);
            else if (direction == Direction.right)
                return new Index(x + 1, y);
            else return null;
   
        }
        public static Index FormatString(string index)
        {
            string[] splitString = index.Split(',');
            try
            {
                return new Index(int.Parse(splitString[0]), int.Parse(splitString[1]));
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
            if (this.x == index.x && this.y == index.y )
                return true;
            else return false;
        }
    }
}