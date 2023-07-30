using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Database {
    public class ModCollection : IEnumerable<ModBase>
    {
        private readonly ModBase[] mods;

        private int length = 0;

        public ModCollection(byte availableMods)
        {
            mods = new ModBase[10];

            length = availableMods;
        }

        public int Length => length;

        public ModBase this[int index]
        {
            get
            {
                if (index < length && index >= 0)
                    return mods[index];
                else
                    throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < length && index >= 0)
                    mods[index] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public void NewCount(int newCount)
        {
            length = newCount;
        }

        public ModBase[] GetAllMods()
        {
            return mods.Select(x => x).ToArray();
        }

        public void SetInAllMods(ModBase mod, int index)
        {
            mods[index] = mod;
        }

        public IEnumerator<ModBase> GetEnumerator()
        {
            for (int i = 0; i < mods.Length; i++)
            {
                if (i < length)
                {
                    yield return mods[i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}


//namespace Database
//{
//    public class ModCollection : IEnumerable<ModBase>
//    {
//        readonly ModBase[] mods;
//        readonly bool[] modsAvailability;

//        private int length = 0;

//        public ModCollection(byte availableMods)
//        {
//            mods = new ModBase[10];
//            modsAvailability = new bool[10];

//            for (int i = 0; i < availableMods; i++)
//            {
//                modsAvailability[i] = true;
//            }

//            length = availableMods;
//        }

//        public int Length => length;

//        public ModBase this[int index]
//        {
//            get
//            {
//                if (index < length && index >= 0)
//                    return mods[index];
//                else
//                    throw new IndexOutOfRangeException();
//            }
//            set
//            {
//                if (index < length && index >= 0)
//                    mods[index] = value;
//                else
//                    throw new IndexOutOfRangeException();
//            }
//        }

//        public void NewCount(int newCount)
//        {
//            for (int i = 0; i < modsAvailability.Length; i++)
//            {
//                if (i < newCount)
//                    modsAvailability[i] = true;
//                else
//                    modsAvailability[i] = false;
//            }

//            length = newCount;
//        }

//        public ModBase[] GetAllMods()
//        {
//            return mods.Select(x => x).ToArray();
//        }

//        public void SetInAllMods(ModBase mod, int index)
//        {
//            mods[index] = mod;
//        }

//        public IEnumerator<ModBase> GetEnumerator()
//        {
//            for (int i = 0; i < mods.Length; i++)
//            {
//                if (modsAvailability[i])
//                {
//                    yield return mods[i];
//                }
//            }
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
//    }
//}