﻿using System.Collections.Generic;

namespace Ollert.DataAccess.Entitites
{
    public class CardList : Entity
    {
        public virtual string Name { get; set; }

        public virtual IEnumerable<Card> Cards { get; set; }
    }
}
