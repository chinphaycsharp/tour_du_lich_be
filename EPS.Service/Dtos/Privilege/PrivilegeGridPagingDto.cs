using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.Privilege
{
    public class PrivilegeGridPagingDto : PagingParams<PrivilegeGridDto>
    {
        public string FilterText { get; set; }
        public override List<Expression<Func<PrivilegeGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (!string.IsNullOrEmpty(FilterText))
            {
                predicates.Add(x => x.Id.Contains(FilterText));
            }
            predicates.Add(x => x.Status == true);
            return predicates;
        }
    }
}
