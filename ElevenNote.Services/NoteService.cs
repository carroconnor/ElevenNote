using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevenNote.Data;
using ElevenNote.Data.Models;
using ElevenNote.Models;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<NoteListsItemModel> GetNotes()
        {
            using (var ctx = new ElevenNoteDBContext())
            {
                //basically a foreachloop going through database 
                return 
                    ctx.Notes
                       .Where(e => e.OwnerId == _userId)
                       .Select(
                            e => 
                                new NoteListsItemModel 
                                {
                                    NoteId = e.NoteId,
                                    Title = e.Title,
                                    CreatedUtc = e.CreatedUtc,
                                    ModifiedUtc = e.ModifiedUtc
                                })
                        .ToArray();
            }

            
        }

        public bool CreateNote(NoteCreateModel model)


        {
            using (var ctx = new ElevenNoteDBContext())
            {
                var entity =
                    new NoteEntity
                    {
                        OwnerId = _userId,
                        Title = model.Title,
                        Content = model.Content,
                        CreatedUtc = DateTime.UtcNow
                    };

                ctx.Notes.Add(entity);

                return ctx.SaveChanges() ==1;
            }
        }

        public NoteDetailModel GetNoteById(int id)
        {
            NoteEntity entity;

            using (var ctx = new ElevenNoteDBContext())
            {
                entity = GetNoteById(ctx, id);
            }

            if (entity == null) return new NoteDetailModel();

            return new NoteDetailModel
            {
                NoteId = entity.NoteId,
                Title = entity.Title,
                Content = entity.Content,
                CreatedUtc = entity.CreatedUtc,
                ModifiedUtc = entity.ModifiedUtc
            };
        }

        public bool UpdateNote(NoteEditModel model)
        {

            using (var ctx = new ElevenNoteDBContext())
            {
                var entity = GetNoteById(ctx, model.NoteId);

                if (entity == null) return false;

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.ModifiedUtc = DateTime.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        private NoteEntity GetNoteById(ElevenNoteDBContext context, int noteId)
        {
            return context.Notes.SingleOrDefault(e => e.NoteId == noteId && e.OwnerId == _userId);
        }

        public bool DeleteNote(int noteId)
        {
            using(var ctx = new ElevenNoteDBContext())
            {
                var entity = GetNoteById(ctx, noteId);

                if (entity == null) return false;

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
