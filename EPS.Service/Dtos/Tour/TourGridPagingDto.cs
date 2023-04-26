using EPS.Service.Dtos.Privilege;
using EPS.Utils.Service;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace EPS.Service.Dtos.Tour
{
    public class TourGridPagingDto : PagingParams<TourGridDto>
    {
        public string FilterText { get; set; }
        public int id_category { get; set; }
        public override List<Expression<Func<TourGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (!string.IsNullOrEmpty(FilterText))
            {
                predicates.Add(x => x.name.Contains(FilterText));
            }

            if(id_category > 0)
            {
                predicates.Add(x => x.category_id == id_category);
            }
            predicates.Add(x => x.status == 1);
            return predicates;
        }
    }
}
