using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouVoice
{
    public class Command
    {
        public List<string> Commands;
        public Action OnCommanded { get; protected set; }
        public string Name { get; protected set; }

        public Command()
        {
            this.OnCommanded = null;
            this.Commands = new List<string>();
        }
        public Command(string Name, Action OnCommanded, List<string> Commands)
        {
            this.Name = Name;
            this.OnCommanded = OnCommanded;
            this.Commands = Commands;
        }
    }
}
