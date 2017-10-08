using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderSystem
{
    /// <summary>
    /// Delegate Event DoHandler 
    /// </summary>
    public delegate void DoHandler();

    public class Timeout
    {
        private ManualResetEvent mTimeoutObject;
        private bool mBoTimeout;
        public DoHandler Do;

        public Timeout()
        {
            //Initial Status: Stop
            this.mTimeoutObject = new ManualResetEvent(true);
        }

        ///<summary>
        /// When TimeOut, Execute A Certain Method Asynchronously 
        ///</summary>
        ///<returns>Is TimeOut</returns>
        public bool DoWithTimeout(TimeSpan timeSpan)
        {
            if (this.Do == null)
            {
                return false;
            }
            this.mTimeoutObject.Reset();
            this.mBoTimeout = true;      //Flag
            this.Do.BeginInvoke(DoAsyncCallBack, null);
            // Wait Signal Set
            if (!this.mTimeoutObject.WaitOne(timeSpan, false))
            {
                this.mBoTimeout = true;
            }
            return this.mBoTimeout;
        }

        ///<summary>
        /// Asynchronous Delegates,  Callback Function
        ///</summary>
        ///<param name="result"></param>
        private void DoAsyncCallBack(IAsyncResult result)
        {
            try
            {
                this.Do.EndInvoke(result);
                //Excute No TimeOut
                this.mBoTimeout = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.mBoTimeout = true;
            }
            finally
            {
                this.mTimeoutObject.Set();
            }
        }
    }
}
