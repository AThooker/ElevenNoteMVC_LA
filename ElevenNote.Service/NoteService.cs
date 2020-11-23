using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Service
{
    public class NoteService
    {
        private readonly Guid _userId;
        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateNote(NoteCreate model)
        {
            var entity = new Note()
            {
                OwnerId = _userId,
                Title = model.Title,
                Content = model.Content,
                CreatedUtc = DateTimeOffset.Now
            };

            using (var _ctx = new ApplicationDbContext())
            {
                _ctx.Notes.Add(entity);
                return _ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<NoteListItem> GetNotes()
        {
            using(var _ctx = new ApplicationDbContext())
            {
                var query = _ctx.Notes.Where(p => p.OwnerId == _userId)
                    .Select(e => new NoteListItem
                    {
                        NoteId = e.NoteId,
                        Title = e.Title,
                        CategoryId = (int)e.CategoryId,
                        IsStarred = e.IsStarred,
                        CreatedUtc = e.CreatedUtc
                    });
                return query.ToArray();
            }
        }
        public NoteDetail GetNoteById(int? id)
        {
            using(var _ctx = new ApplicationDbContext())
            {
                var entity = _ctx.Notes.Single(p => p.NoteId == id && p.OwnerId == _userId);
                return new NoteDetail
                {
                    NoteId = entity.NoteId,
                    Title = entity.Title,
                    Content = entity.Content,
                    CategoryId = (int)entity.CategoryId,
                    CategoryName = _ctx.Categories.Where(p => p.Name == entity.Category.Name).ToString(),
                    IsStarred = entity.IsStarred,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
            }
        }
        public bool UpdateNote(NoteEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == model.NoteId && e.OwnerId == _userId);

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.CategoryId = model.CategoryId;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;
                entity.IsStarred = model.IsStarred;

                return ctx.SaveChanges() == 1;
            }
        }
        public bool DeleteNote(int noteId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == noteId && e.OwnerId == _userId);

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
