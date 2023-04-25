using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.Image
{
    public class ImageGridPagingDto : PagingParams<ImageGridDto>
    {
        public int typeId { get; set; }
        public string type { get; set; }
        public override List<Expression<Func<ImageGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();
            predicates.Add(x => x.type == type);
            if (typeId > 0)
            {
                predicates.Add(x => x.typeId == typeId);
            }
            return predicates;
        }
    }
}
