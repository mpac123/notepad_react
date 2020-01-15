using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using notepad_react.Models;
using notepad_react.Services;
using notepad_react.ViewModels;

namespace notepad_react.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {

        private readonly ILogger<NotesController> _logger;
        private INotesService _service;

        public NotesController(ILogger<NotesController> logger,
            INotesService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public ActionResult<NotesList> GetNotes(int page = 1, DateTime? dateFrom = null,
                DateTime? dateTo = null, int? category = null)
        {
            var notesList = _service.Read(page, dateFrom, dateTo, category);
            return Ok(notesList);
        }

        [HttpGet("{id}")]
        public ActionResult<NotesList> GetNote(int id)
        {
            var note = _service.GetNote(id);
            if (note == null)
            {
                return BadRequest();
            }
            return Ok(note);
        }

        [HttpGet("categories")]
        public ActionResult<IEnumerable<string>> GetCategories()
        {
            var categories = _service.GetCategories();
            return Ok(categories);
        }

        [HttpPost]
        public ActionResult PostNote([FromBody] NoteWithCategories note)
        {
            if (_service.DoesTheFileExist(note.Title))
            {
                return BadRequest();
            }
            _service.AddNote(note);
            _service.DeleteDanglingCategories();
            return Ok();
        }

        [HttpPost("{id}")]
        public ActionResult UpdateNote(int id, [FromBody] NoteWithCategories note)
        {
            try
            {
                _service.EditNote(id, note);
                _service.DeleteDanglingCategories();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteNote(int id)
        {
            try
            {
                _service.DeleteNote(id);
                _service.DeleteDanglingCategories();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
