
using TestNewLine.Core.Dtos;
using TestNewLine.Core.ViewModel;
using TestNewLine.Core.ViewModels;

namespace TestNewLine.Web.Services
{
    public interface IPostBlogService
    {
        Task<ResponseDto> GetAll(Pagination pagination, Query query);
        Task<List<PostBlogViewModel>> GetAll2();
        Task<int> Delete(int id);
        Task<UpdatePostBlogDto> Get(int id);
        Task<UserViewModel> GetPostAuthors(string Id);
        Task<int> Create(CreatePostBlogDto dto);
        Task<int> Update(UpdatePostBlogDto dto);
        PagingViewModel GetAll(int page);
        Task<List<PostBlogViewModel>> GetSingle(int id);
    }
}
