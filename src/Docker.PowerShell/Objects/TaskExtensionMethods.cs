using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Docker.PowerShell.Objects
{
    internal static class TaskExtensionMethods
    {
        /// <summary>
        /// Waits for the result of the task, unwrapping and re-throwing any aggregate 
        /// exceptions that result from it.
        /// </summary>
        /// <typeparam name="T">The type of the result in the task</typeparam>
        /// <param name="task">The task to wait on.</param>
        /// <returns></returns>
        internal static T AwaitResult<T>(this Task<T> task)
        {
            task.WaitUnwrap();
            return task.Result;
        }

        /// <summary>
        /// Waits for the task to complete, unwrapping and re-throwing any aggregate
        /// exceptions that result form it.
        /// </summary>
        /// <param name="task">The task to wait on.</param>
        internal static void WaitUnwrap(this Task task)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }

                throw;
            }

        }
    }
}
