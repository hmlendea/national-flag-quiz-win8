using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace National_Flag_Quiz
{
    public class Flag
    {
        public string Name { get; set; }
        public Uri ImageUri
        {
            get { return new Uri("ms-appx:///Assets//Flags//" + Name + ".png"); }
        }
        public bool Used { get; set; }

        public Flag(string name)
        {
            Name = name;
            Used = false;
        }
    }

    public class FlagCollection
    {
        public Flag this[string name]
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    if (Flag[i].Name == name)
                        return Flag[i];

                return null;
            }
        }
        public Flag this[int index] { get { return Flag[index]; } }

        public Flag[] Flag
        {
            get { return flag; }
            set { flag = value; }
        } private Flag[] flag;
        public int Count { get { return Flag.Length; } }

        public FlagCollection()
        {
            Flag = new Flag[0];
        }

        public void Add(Flag flag)
        {
            Array.Resize(ref this.flag, this.flag.Length + 1);
            Flag[Flag.Length - 1] = flag;
        }
        public void Remove(string flagName)
        {
            int i = 0;
            while (i < Flag.Length)
            {
                if (Flag[i].Name == flagName)
                {
                    for (int j = i + 1; j < this.flag.Length; j++)
                        Flag[j - 1] = Flag[j];

                    Array.Resize(ref this.flag, Flag.Length - 1);
                }

                i += 1;
            }
        }
        public void RemoveAt(int index)
        {
            for (int j = index + 1; j < this.flag.Length; j++)
                Flag[j - 1] = Flag[j];

            Array.Resize(ref this.flag, Flag.Length - 1);
        }

        public void Clear()
        {
            Flag = new Flag[0];
        }
    }
}
