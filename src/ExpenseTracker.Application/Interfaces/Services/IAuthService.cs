using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Dtos.Input;
using ExpenseTracker.Application.Dtos.Output;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterInputDto input);

    Task<LoginOutputDto> LoginAsync(LoginInputDto input);
}