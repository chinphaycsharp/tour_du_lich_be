using EPS.Service.Dtos.Common.EvaluateTour;
using EPS.Service.Dtos.Tour;
using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.Common.Evaluate_tour
{
    public class EvaluateTourGridPagingDto : PagingParams<EvaluateTourGridDto>
    {
        public string FilterText { get; set; }
        public int id_tour { get; set; }
        public override List<Expression<Func<EvaluateTourGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (id_tour > 0)
            {
                predicates.Add(x => x.id_tour == id_tour);
            }
            predicates.Add(x => x.status == 1);
            return predicates;
        }
    }
}
