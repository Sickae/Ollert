﻿using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;

namespace Ollert.Logic.Managers
{
    public class CardManager : ManagerBase<Card, CardDTO>, ICardManager
    {
        public CardManager(ISession session) : base(session)
        { }
    }
}
