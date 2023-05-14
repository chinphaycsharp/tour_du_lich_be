using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.Blog
{
    public class BlogPagingGridDto : PagingParams<BlogGridDto>
    {
        public string FilterText { get; set; }
        public override List<Expression<Func<BlogGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (!string.IsNullOrEmpty(FilterText))
            {
                predicates.Add(x => x.title.Contains(FilterText));
            }
            return predicates;
        }
    }
}
