using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.ImageBlog
{
    public class ImageGridPagingDto : PagingParams<ImageGridDto>
    {
        public int id_type { get; set; }
        public override List<Expression<Func<ImageGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (id_type > 0)
            {
                predicates.Add(x => x.type_id == id_type);
            }
            predicates.Add(x => x.status == 1);
            return predicates;
        }
    }
}
