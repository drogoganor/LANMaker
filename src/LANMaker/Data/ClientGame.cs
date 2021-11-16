using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LANMaker.Data
{
    public class ClientGame
    {
        public string Name { get; set; }
        public string InstalledVersion { get; set; }
        public string InstallPath { get; set; }
        public string ExePath { get; set; }
        public bool Portable { get; set; }
        public bool Multiplayer { get; set; }
    }
}
