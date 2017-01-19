// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Debug.cs" company="">
//   
// </copyright>
// <summary>
//   captures all Messages sent though debug.write line so that they can send/output to custom location
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FeedScraper.WebApp
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    ///     captures all Messages sent though debug.write line so that they can send/output to custom location
    /// </summary>
    public class MyDebug
    {
        /// <summary>
        ///     Lists all Messages received during the run of the program
        /// </summary>
        public static List<string> Messages = new List<string>();

        /// <summary>
        ///     The write line to console as well as stores it to the "Messages" list
        /// </summary>
        /// <param name="msg"> 
        ///     txt of the message to be logged/displayed in console
        /// </param>
        public static void WriteLine(string msg)
        {
            Debug.WriteLine(msg);
            Messages.Add(msg);
        }

        /// <summary>
        /// The delete all messages.
        /// </summary>
        public static void DeleteMessages()
        {
            Messages.Clear();
        }

        /// <summary>
        ///     The get all Messages collected during runtime
        /// </summary>
        /// <returns>
        ///     The <see cref="string"/>.
        /// </returns>
        public string GetAllMessages()
        {
            var allMsg = Messages.Aggregate(string.Empty, (current, m) => current + m);
            return allMsg;
        }


    }
}