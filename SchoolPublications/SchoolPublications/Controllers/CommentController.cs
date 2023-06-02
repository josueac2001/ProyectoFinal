using SchoolPublications.DAL;
//using SchoolPublications.DAL.Entities;
//using SchoolPublications.Enums;
//using SchoolPublications.Helpers;
//using SchoolPublications.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Authorization;

//namespace SchoolPublications.Controllers
//{
//    [Authorize(Roles = "Admin")]
//    [AllowAnonymous]
//    public class CommentController : Controller
//    {
//        private readonly ICommentRepository _commentRepository;

//        public CommentController(ICommentRepository commentRepository)
//        {
//            _commentRepository = commentRepository;
//        }

//        // Acción para mostrar los comentarios de una publicación específica
//        public IActionResult Index(int publicationId)
//        {
//            var comments = _commentRepository.GetCommentsByPublicationId(publicationId);
//            return View(comments);
//        }

//        // Acción para crear un nuevo comentario
//        [HttpPost]
//        public IActionResult Create(Comment comment)
//        {
//            if (ModelState.IsValid)
//            {
//                comment.CommentDate = DateTime.Now;
//                _commentRepository.Add(comment);
//                return RedirectToAction("Index", new { publicationId = comment.Publication.Id });
//            }

//            // Si hay errores de validación, regresar a la vista de creación con los errores mostrados
//            return View(comment);
//        }

//        // Acción para editar un comentario existente
//        public IActionResult Edit(int id)
//        {
//            var comment = _commentRepository.GetCommentById(id);

//            if (comment == null)
//            {
//                return NotFound();
//            }

//            return View(comment);
//        }

//        [HttpPost]
//        public IActionResult Edit(Comment comment)
//        {
//            if (ModelState.IsValid)
//            {
//                _commentRepository.Update(comment);
//                return RedirectToAction("Index", new { publicationId = comment.Publication.Id });
//            }

//            // Si hay errores de validación, regresar a la vista de edición con los errores mostrados
//            return View(comment);
//        }

//        // Acción para mostrar los detalles de un comentario
//        public IActionResult Details(int id)
//        {
//            var comment = _commentRepository.GetCommentById(id);

//            if (comment == null)
//            {
//                return NotFound();
//            }

//            return View(comment);
//        }

//        // Acción para eliminar un comentario
//        public IActionResult Delete(int id)
//        {
//            var comment = _commentRepository.GetCommentById(id);

//            if (comment == null)
//            {
//                return NotFound();
//            }

//            return View(comment);
//        }

//        [HttpPost, ActionName("Delete")]
//        public IActionResult DeleteConfirmed(int id)
//        {
//            var comment = _commentRepository.GetCommentById(id);

//            if (comment == null)
//            {
//                return NotFound();
//            }

//            _commentRepository.Delete(comment);
//            return RedirectToAction("Index", new { publicationId = comment.Publication.Id });
//        }

//        //public IActionResult Unauthorized()                      // //Vista de retorno en caso no estar autorizado

//        //    return View();
//        //}
//    }
//}
