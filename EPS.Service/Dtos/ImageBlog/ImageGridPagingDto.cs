using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.ImageBlog
{
    public class ImageGridPagingDto : PagingParams<ImageGridDto>
    {
        public string type { get; set; }
        public int id { get; set; }
        public override List<Expression<Func<ImageGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (type != null)
            {
                predicates.Add(x => x.type == type);
            }
            if(id > 0)
            {
                predicates.Add(x => x.type_id == id);
            }
            predicates.Add(x => x.status == 1);
            return predicates;
        }
    }
}
