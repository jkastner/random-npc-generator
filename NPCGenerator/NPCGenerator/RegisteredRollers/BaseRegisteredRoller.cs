using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCGenerator
{
    internal abstract class BaseRegisteredRoller
    {
        internal abstract String RollerName { get; }

        internal abstract void Run(NPC newNPC);
    }
}
