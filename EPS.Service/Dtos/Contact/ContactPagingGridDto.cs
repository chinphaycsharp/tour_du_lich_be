using EPS.Service.Dtos.Category;
using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.Contact
{
    public class ContactPagingGridDto : PagingParams<ContactGridDto>
    {
        public string FilterText { get; set; }
        public override List<Expression<Func<ContactGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (!string.IsNullOrEmpty(FilterText))
            {
                predicates.Add(x => x.email_register.Contains(FilterText));
            }
            predicates.Add(x => x.status == 1);
            return predicates;
        }
    }
}
