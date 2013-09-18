using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;

using System.Speech.Recognition;
using SpeechLib;

namespace YouVoice
{
    public class DictationListener
        : ListenerBase
    {
        protected string Output;

        public DictationListener(string Output)
            : base()
        {
            #region Base Commands
            // Clear
            Commands.Add(new Command("Clear", () =>
            {
                lock (Output)
                {
                    Output = string.Empty;
                }
            }, new List<string>() { "Clear", "Reset" }));

            // Stop
            Commands.Add(new Command("Stop", () =>
            {
                Stop();
            }, new List<string>() { }));
            #endregion

            this.Output = Output;
        }

        protected override void Listen()
        {
            while (Running)
            {
                string Recognised = Recog.Recognize().Text;
                // Check all commands
                for (int x = 0; x < Commands.Count; x++)
                {
                    // Check all command words for that command
                    for (int y = 0; y < Commands[x].Commands.Count; y++)
                    {
                        // If there's a match
                        if (Commands[x].Commands[y] == Recognised)
                        {
                            // Run the specified command
                            Commands[x].OnCommanded();
                            // Continue to the next iteration
                            continue;
                        }
                    }
                }

                // Mustn't have been a match
                Output = Recognised;
            }
        }
    }
}
