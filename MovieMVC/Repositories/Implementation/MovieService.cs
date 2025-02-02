﻿using MovieMVC.Models.Domain;
using MovieMVC.Models.DTO;
using MovieMVC.Repositories.Abstract;
using System.Threading.Tasks.Dataflow;

namespace MovieMVC.Repositories.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly DataBaseContext ctx;
        public MovieService(DataBaseContext ctx)
        {
            this.ctx = ctx;
        }
        public bool Add(Movie model)
        {
            try
            {
                ctx.Movie.Add(model);
                ctx.SaveChanges();
                foreach(int genreId in model.Genres)
                {
                    var movieGenre = new MovieGenre
                    {
                        MovieId = model.Id,
                        GenreId = genreId,

                    };
                    ctx.MovieGenre.Add(movieGenre);
                    ctx.SaveChanges();
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null)
                    return false;
                var movieGenres = ctx.MovieGenre.Where(a => a.MovieId == data.Id);
                foreach (var movieGenre in movieGenres)
                {
                    ctx.MovieGenre.Remove(movieGenre);
                }
                ctx.Movie.Remove(data);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Movie GetById(int id)
        {
            return ctx.Movie.Find(id);
        }

        public MovieListVm List(string term="", bool paging=false, int currentPage=0)
        {
            var data = new MovieListVm();
            
            var list = ctx.Movie.ToList();
           
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a => a.Title.ToLower().StartsWith(term)).ToList();
            }
            if (paging)
            {
                int pageSize = 5;
                int count = list.Count;
                int totalPages = (int) Math.Ceiling(count  /(double) pageSize);
                list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = totalPages;
            }
            foreach (var movie in list)
            {
                var genres = (from genre in ctx.Genre
                              join mg in ctx.MovieGenre
                              on genre.Id equals mg.MovieId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                              ).ToList();
                var genreNames = string.Join(',', genres);
                movie.GenreNames = genreNames;
            }
            data.MovieList = list.AsQueryable();
            return data;
        }

        public bool Update(Movie model)
        {
            try
            {
                // these genre ids are not selected by users and still present in movieGenre table corresponding to 
                // this movieId. so these ids should be removed. 
                var genresToDelete = ctx.MovieGenre.Where(a => a.MovieId == model.Id && !model.Genres.Contains(a.GenreId)).ToList();
                foreach (var mGenre in genresToDelete)
                {
                    ctx.MovieGenre.Remove(mGenre);
                }

                foreach (int genId in model.Genres)
                {
                    var movieGenre = ctx.MovieGenre.FirstOrDefault(a => a.MovieId == model.Id && a.GenreId == genId);
                    if (movieGenre == null)
                    {
                        movieGenre = new MovieGenre { GenreId = genId, MovieId = model.Id };
                        ctx.MovieGenre.Add(movieGenre);
                    }
                }

                ctx.Movie.Update(model);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // log the exception if necessary
                return false;
            }
        }

        public List<int> GetGenreMovieById(int movieId)
        {
            var genreIds = ctx.MovieGenre.Where(a => a.MovieId == movieId).Select(a => a.GenreId).ToList();
            return genreIds;
        }
    }
}
