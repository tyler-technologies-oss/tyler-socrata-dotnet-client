using Socrata.DSMAPI;
using Socrata.SODA;
using System.Collections.Generic;

namespace Socrata.Abstractions
{    
    public interface IResource
    {
        Result Delete();
        Rows Rows();
        Revision OpenRevision(RevisionType type);
        WorkingCopy CreateWorkingCopy();

    }
}
