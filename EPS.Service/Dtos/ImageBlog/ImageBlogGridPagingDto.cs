using EPS.Service.Dtos.ImageTour;
using EPS.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EPS.Service.Dtos.ImageBlog
{
    public class ImageBlogGridPagingDto : PagingParams<ImageBlogGridDto>
    {
        public int id_blog { get; set; }
        public override List<Expression<Func<ImageBlogGridDto, bool>>> GetPredicates()
        {
            var predicates = base.GetPredicates();

            if (id_blog > 0)
            {
                predicates.Add(x => x.id_blog == id_blog);
            }
            predicates.Add(x => x.status == 1);
            return predicates;
        }
    }
}
