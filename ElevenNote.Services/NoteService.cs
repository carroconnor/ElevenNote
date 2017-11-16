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
                entity = ctx.Notes.SingleOrDefault(e => e.NoteId == id && e.OwnerId == _userId);
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
    }
}
