using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCGenerator
{
    class NPCGeneratorExceptions : Exception
    {
        private String _userMessage;

        public String UserMessage
        {
            get { return _userMessage; }
            set { _userMessage = value; }
        }

    }
}
