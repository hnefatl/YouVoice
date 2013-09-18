using System;
using System.Collections.Generic;
using System.Threading;

using System.Speech.Recognition;
using SpeechLib;

namespace YouVoice
{
    public class Listener
        : ListenerBase
    {
        public Listener(bool UseBabbleGuard = true)
            : base()
        {
            if (UseBabbleGuard)
            {
                // Implement a babble guard - protection from background noise
                AddCommand(new Command("Babble", () => { }, new List<string>() { "..." }));
            }
        }

        protected override void Listen()
        {
            while (Running)
            {
                // Recognise a string and get the grammar it was found from (effectively getting the command,
                // regardless of which variant of it was used)
                string GrammarName = string.Empty;
                try
                {
                    // Recognise a command, store the name of the command used
                    GrammarName = Recog.Recognize().Grammar.Name;

                    // Locate the specific command
                    for (int x = 0; x < Commands.Count; x++)
                    {
                        if (Commands[x].Name == GrammarName)
                        {
                            // Fire the command event
                            Commands[x].OnCommanded();
                            break;
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    // Timeout, no matter. Re-loop
                    continue;
                }
            }
        }
    }
}
