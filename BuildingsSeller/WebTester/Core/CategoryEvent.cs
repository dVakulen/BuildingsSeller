
using System;

namespace BuildSeller.Core
{

    public class CategoryEventArgs : EventArgs
    {

        public CategoryEventArgs(string status)
        {
            this.Status = status;
        }

        public string Status { get; private set; }
    }
}
