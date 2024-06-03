using MovieMVC.Models.Domain;
using MovieMVC.Models.DTO;

namespace MovieMVC.Repositories.Abstract
{
    public interface IMovieService
    {
        bool Add(Movie movie);
        bool Update(Movie movie);
        bool Delete(int id);
        Movie GetById(int id);
        MovieListVm List(string term = "", bool paging = false, int currentPage = 0);
        List<int> GetGenreMovieById(int movieId);
    }
}
