using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using notepad_react.Models;
using notepad_react.ViewModels;

namespace notepad_react.Services
{
    public interface INotesService
    {
        NotesList Read(int page = 1, DateTime? dateFrom = null, DateTime? dateTo = null, int? categoryId = null);
        Note GetNote(int id);
        List<Category> GetCategories();
        bool DoesTheFileExist(string title);
        int GetPageWhereTheNoteIs(int noteId, DateTime? dateFrom = null, DateTime? dateTo = null, int? categoryId = null);
        int AddNote(NoteWithCategories note);
        void EditNote(int id, NoteWithCategories newNote);
        void DeleteDanglingCategories();
        void DeleteNote(int id);
    }

    public class NotesService : INotesService
    {
        private readonly NotesContext _context;

        public NotesService(NotesContext context)
        {
            _context = context;
        }

        public int GetPageWhereTheNoteIs(int noteId, DateTime? dateFrom = null, DateTime? dateTo = null, int? categoryId = null)
        {
            var filteredList = GetFilteredList(dateFrom, dateTo, categoryId);
            var ids = filteredList.Select(l => l.NoteID).ToList();
            var index = ids.IndexOf(noteId);
            var page = (int)((index + 1) / 10) + 1;
            return page;
        }


        public void EditNote(int id, NoteWithCategories newNote)
        {
            var oldNote = _context.Notes.Where(n => n.NoteID == id
                && n.Timestamp == newNote.Timestamp).FirstOrDefault();
            if (oldNote == null)
            {
                throw new Exception("The file does not exist or has been changed");
            }
            // Find differences in standard fields and replace if needed
            if (oldNote.Title != newNote.Title
                || oldNote.Description != newNote.Description
                || oldNote.NoteDate != newNote.NoteDate)
            {
                oldNote.Title = newNote.Title;
                oldNote.Description = newNote.Description;
                oldNote.NoteDate = newNote.NoteDate;
            }

            // Find differences in categories

            var categoriesInDb = _context.NoteCategories
                .Where(nc => nc.Note == oldNote)
                .Include(nc => nc.Category);

            var newCategories = newNote.Categories
                .Except(categoriesInDb.Select(c => c.Category.Title));

            foreach (var newCategory in newCategories)
            {
                var categoryInDb = _context.Categories
                    .Where(c => c.Title == newCategory)
                    .FirstOrDefault();

                if (categoryInDb == null)
                {
                    categoryInDb = new Category
                    {
                        Title = newCategory
                    };
                }
                _context.Add(new NoteCategory
                {
                    Note = oldNote,
                    Category = categoryInDb
                });
            }

            var deletedCategories = categoriesInDb
                .Where(c => !newNote.Categories.Contains(c.Category.Title));
            _context.RemoveRange(deletedCategories);

        }

        public void DeleteDanglingCategories()
        {
            var danglingCategories = _context.Categories
                .Where(c => c.NoteCategories.Count() == 0);
            _context.RemoveRange(danglingCategories);
        }

        private IIncludableQueryable<Models.Note, Category> GetFilteredList(DateTime? dateFrom = null, DateTime? dateTo = null, int? categoryId = null)
        {
            var listQuery = _context.Notes.Where(n => true);
            if (dateFrom.HasValue)
            {
                listQuery = listQuery.Where(n => n.NoteDate.Date >= dateFrom.Value.Date);
            }
            if (dateTo.HasValue)
            {
                listQuery = listQuery.Where(n => n.NoteDate.Date <= dateTo.Value.Date);
            }
            if (categoryId != null)
            {
                var notesIds = _context.NoteCategories.Where(n => n.CategoryID == categoryId.Value)
                    .Select(nc => nc.NoteID).ToList();
                listQuery = listQuery.Where(n => notesIds.Contains(n.NoteID));
            }
            var filteredList = listQuery
                .Include(n => n.NoteCategories)
                .ThenInclude(nc => nc.Category);

            return filteredList;
        }

        public NotesList Read(int page = 1, DateTime? dateFrom = null, DateTime? dateTo = null, int? categoryId = null)
        {
            var filteredList = GetFilteredList(dateFrom, dateTo, categoryId);

            var allPages = (int)Math.Ceiling((decimal)(filteredList.Count()) / 10);

            if (page == 0)
            {
                page = 1;
            }
            if (page > allPages)
            {
                page = allPages;
            }

            var notes = filteredList
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();

            var notesList = new NotesList
            {
                Notes = notes,
                CurrentPage = page,
                AllPages = allPages == 0 ? 1 : allPages,
            };

            return notesList;
        }

        public Note GetNote(int id)
        {
            var note = _context.Notes.Where(n => n.NoteID == id).FirstOrDefault();
            return note;
        }

        public List<Category> GetCategories()
        {
            var categories = _context.Categories.ToList();
            return categories;
        }

        public bool DoesTheFileExist(string title)
        {
            var note = _context.Notes.Where(n => n.Title == title).FirstOrDefault();
            if (note == null)
            {
                return false;
            }
            return true;
        }

        public int AddNote(NoteWithCategories note)
        {
            var oldCategories = _context.Categories
                    .Where(c => note.Categories.Contains(c.Title))
                    .Include(c => c.NoteCategories)
                    .ToList();

            var newCategories = note.Categories
                .Except(oldCategories.Select(oc => oc.Title));

            var newNote = new Note
            {
                NoteDate = note.NoteDate,
                Title = note.Title,
                Description = note.Description
            };
            _context.Add(newNote);
            foreach (var oldCat in oldCategories)
            {
                var newNoteCat = new NoteCategory
                {
                    Note = newNote,
                    Category = oldCat
                };
                _context.Add(newNoteCat);
            }
            foreach (var newCat in newCategories)
            {
                var newNoteCat = new NoteCategory
                {
                    Note = newNote,
                    Category = new Category
                    {
                        Title = newCat
                    }
                };
                _context.Add(newNoteCat);
            }
            _context.SaveChanges();
            return newNote.NoteID;
        }

        public void DeleteNote(int id)
        {
            var note = _context.Notes.Where(n => n.NoteID == id)
                .FirstOrDefault();
            if (note != null)
            {
                _context.Remove(note);
            }
            else
            {
                throw new Exception($"Cannot delete the note with id {id}");
            }
        }
    }
}