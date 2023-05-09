using EPS.Service.Dtos.ImageTour;
using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.Hotel
{
    public class HotelPagingGridDto : PagingParams<HotelGridDto>
    {
        public string name { get; set; }
        public override List<Expression<Func<HotelGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (name != null && name != "")
            {
                predicates.Add(x => x.name == name);
            }
            predicates.Add(x => x.status == 1);
            return predicates;
        }
    }
}
