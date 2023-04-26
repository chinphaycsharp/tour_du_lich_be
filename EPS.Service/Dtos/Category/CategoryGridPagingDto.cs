using EPS.Service.Dtos.Privilege;
using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.Category
{
    public class CategoryGridPagingDto : PagingParams<CategoryGridDto>
    {
        public string FilterText { get; set; }
        public override List<Expression<Func<CategoryGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (!string.IsNullOrEmpty(FilterText))
            {
                predicates.Add(x => x.name.Contains(FilterText));
            }
            predicates.Add(x => x.status == 1);
            return predicates;
        }
    }
}
