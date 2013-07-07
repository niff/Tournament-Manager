using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IglaClub.ObjectModel
{
    public class OperationStatus
    {
        public OperationStatus(bool ok)
        {
            this.Ok = ok;
            this.ErrorMessage = string.Empty;
        }

        public OperationStatus(bool ok, string errorMessage) : this(ok)
        {
            this.ErrorMessage = errorMessage;
        }

        public bool Ok { get; private set; }
        public string ErrorMessage { get; private set; }
    }
}
