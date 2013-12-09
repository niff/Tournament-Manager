using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IglaClub.ObjectModel.Entities
{
    public class BoardInstance : BaseEntity
    {

        public virtual Tournament Tournament { get; set; }

        public virtual BoardDefinition BoardDefinition { get; set; }

        public virtual IList<Result> Results { get; set; }

        [DisplayName("Board Number")]
        public int BoardNumber { get; set; }
    }
}