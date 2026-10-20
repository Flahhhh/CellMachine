using System.Drawing;
using Const;

namespace Models
{
    class Colony
    {
        public Color color;
        public int id;
        public int? ancestorID;
        private static int NextID {get; set;} = 1;
        
        public long descendantCount = 0;
        public long heirCount = 0;
        public int[] gene = new int[Settings.gameConfig.geneSize];

        public Colony(Color color, int? ancestorID=null)
        {
            id = NextID;
            NextID++;

            this.ancestorID = ancestorID;
            this.color = color;
        }

    }
}