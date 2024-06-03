using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using MovieMVC.Models.Domain;
using MovieMVC.Repositories.Abstract;
using MovieMVC.Repositories.Implementation;

namespace MovieMVC.Controllers
{
	//[Authorize]
	public class GenreController : Controller
	{
		private IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
			_genreService = genreService;
        }
        public IActionResult Index()
		{
			return View();
		}
		public IActionResult Add()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Add(Genre model)
		{
			if (!ModelState.IsValid)

				return View(model);
			var result = _genreService.Add(model);
			if (result)
			{
				TempData["msg"] = "Added Successfully!!";
				return RedirectToAction(nameof(Add));
			}
			else
			{
				TempData["msg"] = "Server Error...";
				return View(model);
			}
		}
		public IActionResult Edit(int id)
		{
			var data = _genreService.GetById(id);
			return View(data);
		}
		[HttpPost]
		public IActionResult Update(Genre model)
		{
			if (!ModelState.IsValid)

				return View(model);
			var result = _genreService.Update(model);
			if (result)
			{
				TempData["msg"] = "Added Successfully!!";
				return RedirectToAction(nameof(GenreList));
			}
			else
			{
				TempData["msg"] = "Server Error...";
				return View(model);
			}
		}
		public IActionResult GenreList()
		{
			var data = this._genreService.List().ToList();
			return View(data);
		}
		
		public IActionResult Delete(int id)
		{
			var result = _genreService.Delete(id);
				return RedirectToAction(nameof(GenreList));
		}
	}
	}

