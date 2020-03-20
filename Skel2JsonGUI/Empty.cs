using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spine;

namespace Skel2Json
{
    class Empty: TextureLoader
    {
        public void Load(AtlasPage page, string path)
        {
            return;
        }

        public void Unload(object texture)
        {
            return;
        }
    }
}
