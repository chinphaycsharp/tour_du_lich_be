using EPS.Data.Entities;
using EPS.Service.Dtos.User;
using EPS.Service.Helpers;
using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EPS.Service.Dtos.User
{
    public class UserGridPagingDto : PagingParams<UserGridDto>, IUnitTraversal<Data.Entities.User>
    {
        public string Username { get; set; }
        public int isAdmin { get; set; }
        public int? UnitId { get; set; }
        public UnitTraversalOption? UnitTraversalOption { get; set; }

        public override List<Expression<Func<UserGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (!string.IsNullOrEmpty(Username))
            {
                predicates.Add(x => x.Username.Contains(Username));
            }

            if (isAdmin == 1)
            {
                predicates.Add(x => x.IsAdministrator == true);
            }
            if(isAdmin == 2)
            {
                predicates.Add(x => x.IsAdministrator == false);
            }

            predicates.Add(x => x.DeletedUserId == null);
            return predicates;
        }

        public void Traversing(IQueryable<UnitAncestor> unitAncestors, ref IQueryable<Data.Entities.User> query)
        {
            if (UnitId.HasValue)
            {
                switch (UnitTraversalOption.GetValueOrDefault())
                {
                    case Service.Helpers.UnitTraversalOption.IncludeChildren: query = query.Where(x => x.UnitId == UnitId.Value || x.UnitId == UnitId.Value); break;
                    case Service.Helpers.UnitTraversalOption.IncludeDescendants:
                        //query = query.Where(x => x.Unit.Ancestors.Select(y => y.UnitAncestorId).Contains(UnitId.Value));
                        // using join instead of where in to improve performance
                        query = query.Join(unitAncestors.Where(x => x.UnitAncestorId == UnitId.Value), x => x.UnitId, x => x.UnitId, (x, y) => x);
                        break;
                    default: query = query.Where(x => x.UnitId == UnitId.Value); break;
                }
            }
        }
    }
}
