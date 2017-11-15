using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevenNote.Data.Models;
using ElevenNote.Models;

namespace ElevenNote.Services
{
    public class NoteService
    {
        public IEnumerable<NoteListsItemModel> GetNotes()
        {
            using (var ctx = new ElevenNoteDBContext())
            {
                //basically a foreachloop going through database 
                return 
                    ctx.Notes.Select(
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
    }
}
