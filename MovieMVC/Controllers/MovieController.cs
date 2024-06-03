using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using MovieMVC.Models.Domain;
using MovieMVC.Repositories.Abstract;
using MovieMVC.Repositories.Implementation;
using static System.Net.Mime.MediaTypeNames;

namespace MovieMVC.Controllers
{
	//[Authorize]
	public class MovieController : Controller
	{
		private readonly IMovieService _movieService;
		private readonly IFileService _fileService;
		private readonly IGenreService _genreService;
        public MovieController(IMovieService movieService, IFileService fileService, IGenreService genreService)
        {
			_movieService = movieService;
			_fileService = fileService;
			_genreService = genreService;
        }
        public IActionResult Index()
		{
			return View();
		}
		public IActionResult Add()
		{
			var model = new Movie();
			model.GenreList = _genreService.List().Select(a => new SelectListItem{ Text = a.GenreName, Value = a.Id.ToString()});
			return View(model);
		}
		[HttpPost]
		public IActionResult Add(Movie model)
		{
            model.GenreList = _genreService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });

            if (!ModelState.IsValid)
				return View(model);
			if (model.ImageFile != null)
			{
				var fileResult = this._fileService.SaveImage(model.ImageFile);
				if (fileResult.Item1 == 0)
				{
					TempData["msg"] = "File cannot be saved...";
					return View(model);
				}
				var imageName = fileResult.Item2;
				model.MovieImage = imageName;
			}
			var result = _movieService.Add(model);
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
			var model = _movieService.GetById(id);
            var selectedGenres = _movieService.GetGenreMovieById(model.Id);
			MultiSelectList multiSelectList = new MultiSelectList(_genreService.List(),"Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiSelectList;

            return View(model);
		}
		[HttpPost]
		public IActionResult Edit(Movie model)
		{
            var selectedGenres = _movieService.GetGenreMovieById(model.Id);
            MultiSelectList multiSelectList = new MultiSelectList(_genreService.List(), "Id", "GenreName", selectedGenres);
			model.MultiGenreList = multiSelectList;

            if (!ModelState.IsValid)

				return View(model);
            if (model.ImageFile != null)
            {
                var fileResult = this._fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File cannot be saved...";
					return View(model);
                }
                var imageName = fileResult.Item2;
                model.MovieImage = imageName;
            }
            var result = _movieService.Update(model);
			if (result)
			{
				TempData["msg"] = "Added Successfully!!";
				return RedirectToAction(nameof(MovieList));
			}
			else
			{
				TempData["msg"] = "Server Error...";
				return View(model);
			}
		}
		public IActionResult MovieList()
		{
			var data = this._movieService.List();
			return View(data);
		}
		
		public IActionResult Delete(int id)
		{
			var result = _movieService.Delete(id);
				return RedirectToAction(nameof(MovieList));
		}
	}
	}

