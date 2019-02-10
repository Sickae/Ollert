﻿using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Managers.Interfaces;

namespace Ollert.Logic.Managers
{
    public class CardListManager : ManagerBase<CardList, CardListDTO>, ICardListManager
    {
        public CardListManager(ISession session) : base(session)
        { }
    }
}
