using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTracker.Application.Common.Constants;
using ExpenseTracker.Application.Common.Exceptions;
using ExpenseTracker.Application.Dtos.Input;
using ExpenseTracker.Application.Dtos.Output;
using ExpenseTracker.Application.Interfaces.Persistence;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Application.Settings;
using ExpenseTracker.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IUserRepository userRepository,
        IOptions<JwtSettings> jwtOptions)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task RegisterAsync(RegisterInputDto input)
    {
        var existingUser = await _userRepository
            .GetByEmailAsync(input.Email);

        if (existingUser is not null)
        {
            throw new ConflictException(ErrorMessages.EmailAlreadyExists);
        }

        var user = new User
        {
            Email = input.Email,
            PasswordHash = BCrypt.Net.BCrypt
                .HashPassword(input.Password)
        };

        await _userRepository.AddAsync(user);
    }

    public async Task<LoginOutputDto> LoginAsync(LoginInputDto input)
    {
        var user = await _userRepository
            .GetByEmailAsync(input.Email);

        if (user is null)
        {
            throw new UnauthorizedException(ErrorMessages.InvalidCredentials);
        }

        var passwordIsValid = BCrypt.Net.BCrypt.Verify(
            input.Password,
            user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new UnauthorizedException(ErrorMessages.InvalidCredentials);
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new LoginOutputDto
        {
            Token = new JwtSecurityTokenHandler()
                .WriteToken(token)
        };
    }
}