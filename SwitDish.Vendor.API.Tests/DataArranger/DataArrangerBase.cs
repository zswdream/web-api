using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SwitDish.Vendor.API.Tests.DataArranger
{
    public abstract class DataArrangerBase
    {
        private static readonly object LockObject = new object();

        /// <summary>
        /// Initializes static members of the <see cref="DataArrangerBase"/> class.
        /// </summary>
        static DataArrangerBase()
        {
            ResetSavedError();
        }

        /// <summary>
        /// lets you specify what to do with the entity in an arranger's constructor
        /// </summary>
        public enum Behaviour
        {
            /// <summary>
            /// Don't create the entity in the database, just prepare a placeholder for one so it gets deleted at the end
            /// </summary>
            TakeOwnership = 0,

            /// <summary>
            /// Create the entity in the using statement and call SaveChanges so it has a populated primary key
            /// </summary>
            CreateEntity,

            /// <summary>
            /// Reload the entity in the using statement from the database by its primary key.
            /// </summary>
            ReloadEntity
        }

        /// <summary>
        /// Gets or sets the saved exception collection.
        /// </summary>
        /// <value>
        /// a list of saved exceptions.
        /// </value>
        public static IList<Exception> SavedExceptions
        {
            get; set;
        }

        /// <value>
        /// The last error message.
        /// </value>
        public static string ErrorMessage
        {
            get; private set;
        }

        /// <summary>
        /// Determines whether there is an error message to throw
        /// </summary>
        public static bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Clears the error message.
        /// </summary>
        public static void ResetSavedError()
        {
            ErrorMessage = string.Empty;
            SavedExceptions = new List<Exception>();
        }

        /// <summary>
        /// Silently log an error, e.g. from an arranger dispose block.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="e">The e.</param>
        protected static void LogErrorForLater(string title, Exception e)
        {
            lock (LockObject)
            {
                SavedExceptions.Add(e);

                string message = title + Environment.NewLine + e;
                Console.WriteLine(message);
                if (Debugger.IsAttached)
                {
                    Trace.WriteLine(message);
                }

                AppendErrorMessage(message);
            }
        }

        private static void AppendErrorMessage(string message)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage += "\r\n";
            }

            ErrorMessage += message;
        }
    }
}
