using ElevenNote.Data;
using ElevenNote.Models;
using ElevenNote.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElevenNoteMVC.Controllers
{
    public class NoteController : Controller
    {
        private ApplicationDbContext _ctx = new ApplicationDbContext();
        // GET: Note
        [Authorize]
        public ActionResult Index()
        {
            NoteService service = CreateNoteService();
            var model = service.GetNotes();
            return View(model);
        }
        //GET: Note Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Note Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var service = CreateNoteService();
            if(!service.CreateNote(model))
            {
                ModelState.AddModelError("","Your note could not be created");
                return View(model);
            }
            TempData["SaveResult"] = "Note created successfully!";
            return RedirectToAction("Index");
        }

        //GET: Note Detail
        [ActionName("Detail")]
        public ActionResult Detail(int? id)
        {
            var service = CreateNoteService();
            if(id == null)
            {
                return RedirectToAction("Index");
            }
            var model = service.GetNoteById(id);
            return View(model);
        }
        //GET: Note Edit
        public ActionResult Edit(int? id)
        {
            var service = CreateNoteService();
            var model = service.GetNoteById(id);
            var detail = new NoteEdit
            {
                NoteId = model.NoteId,
                Title = model.Title,
                Content = model.Content,
                IsStarred = model.IsStarred
            };
            return View(detail);
        }
        //POST: Note Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.NoteId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateNoteService();

            if (service.UpdateNote(model))
            {
                TempData["SaveResult"] = "Your note was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your note could not be updated.");
            return View(model);
        }
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var svc = CreateNoteService();
            var model = svc.GetNoteById(id);

            return View(model);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateNoteService();

            service.DeleteNote(id);

            TempData["SaveResult"] = "Your note was deleted";

            return RedirectToAction("Index");
        }
        //SERVICE: Create Note Service through finding the id of the user, passing it to the service, and giving the user access to the methods within the service
        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
    }
}