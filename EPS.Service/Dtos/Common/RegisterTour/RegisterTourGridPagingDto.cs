using EPS.Service.Dtos.Tour;
using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.Common.RegisterTour
{
    public class RegisterTourGridPagingDto : PagingParams<RegisterTourGridDto>
    {
        public string FilterText { get; set; }
        public int id_tour { get; set; }
        public override List<Expression<Func<RegisterTourGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (!string.IsNullOrEmpty(FilterText))
            {
                predicates.Add(x => x.email_register.Contains(FilterText));
            }

            if (id_tour > 0)
            {
                predicates.Add(x => x.id_tour == id_tour);
            }
            return predicates;
        }
    }
}
