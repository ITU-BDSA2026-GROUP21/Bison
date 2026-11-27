// using Bison.Core.Models;

// public class AuthorService : IAuthorService
// {
//     private readonly IAuthorRepository _repository;
//     public AuthorService(IAuthorRepository repository)
//     {
//         _repository = repository;
//     }
//     public async Task<AuthorDTO> GetAuthorInformationAsync(string username)
//     {
//         Author? author = await _repository.GetAuthorByUsernameAsync(username);

//         if (author == null)
//         {
//             return null;
//         }

//         AuthorDTO dto = DTOConverter.ToDTO(author);

//         return dto;
//     }
// }