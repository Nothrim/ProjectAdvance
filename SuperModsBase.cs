using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TAPI;
namespace ProjectAdvance
{
    public sealed class SuperModsBase : TAPI.ModBase
    {
        public static SuperModsBase SMB
        {
            get;
            private set;
        }
        public SuperModsBase()
            : base()
        {
            SMB = this;
        }
    }
}
