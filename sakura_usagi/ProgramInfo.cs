using System;
using System.Collections.Generic;
using System.Text;

namespace sakura_usagi
{
    public static class ProgramInfo
    {
        public static readonly int MAJOR_VERSION = 0;
        public static readonly int MINOR_VERSION = 0;
        public static readonly int FEATURE_VERSION = 3;
        public static readonly int BUGFIX_VERSION = 0;
        public static readonly string VERSION_TYPE = "-Alpha";

        public static readonly string CODENAME = "Netherland Dwarf";
        public static readonly string VERSION_STRING = "v" + MAJOR_VERSION.ToString() + "." +
                                                             MINOR_VERSION.ToString() + "." +
                                                             FEATURE_VERSION.ToString() + "." +
                                                             BUGFIX_VERSION.ToString() + VERSION_TYPE + " (" + CODENAME + ")";
    }
}
