using AutoMapper;
using NHibernate;
using Ollert.DataAccess.Entitites;
using Ollert.Logic.DTOs;
using Ollert.Logic.Interfaces;
using Ollert.Logic.Managers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ollert.Logic.Managers
{
    public class CommentManager : ManagerBase<Comment, CommentDTO>, ICommentManager
    {
        public CommentManager(ISession session, IAppContext appContext) : base(session, appContext)
        { }

        //public new CommentDTO Get(int id)
        //{
        //    var entity = base.Get(id);
        //    var dto = Mapper.Map<Comment, CommentDTO>(entity);
        //    return dto;
        //}

        //public new IList<CommentDTO> Get(IList<int> ids)
        //{
        //    var entities = base.Get(ids);
        //    var dtos = Mapper.Map<IList<Comment>, IList<CommentDTO>>(entities);
        //    return dtos;
        //}

        //public new IList<CommentDTO> GetAll()
        //{
        //    var entities = base.GetAll();
        //    var dtos = Mapper.Map<IList<Comment>, IList<CommentDTO>>(entities);
        //    return dtos;
        //}

        //public new IList<CommentDTO> GetAll(Expression<Func<Comment, bool>> expression)
        //{
        //    var entities = base.GetAll(expression);
        //    var dtos = Mapper.Map<IList<Comment>, IList<CommentDTO>>(entities);
        //    return dtos;
        //}
    }
}
