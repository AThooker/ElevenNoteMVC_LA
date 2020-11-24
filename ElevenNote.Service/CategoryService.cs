using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Service
{
    public class CategoryService
    {
        public CategoryService()
        {
            
        }
        public bool CreateCategory(CategoryCreate model)
        {
            var entity = new Category()
            {
                Name = model.Name,
            };

            using (var _ctx = new ApplicationDbContext())
            {
                _ctx.Categories.Add(entity);
                return _ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CategoryDetail> GetCategories()
        {
            using(var _ctx = new ApplicationDbContext())
            {
                var entity = _ctx.Categories
                    .Select(p => new CategoryDetail
                    {
                        CategoryId = p.CategoryId,
                        Name = p.Name,
                        Notes = p.Notes.ToList(),
                    });
                return entity.ToList();
            }
        }
    }
}
