using AutoMapper;
using BankingApp.Application.DTOs;
using BankingApp.Domain.Entities;
using FluentValidation;
using Infrastructure.Persistence.Repositories.UserAuth;

namespace BankingApp.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<UpdateDto> _updateValidator;

    public AuthService(ITokenService tokenService,IUserRepository userRepository, IMapper mapper,
        IValidator<LoginDto> loginValidator,
        IValidator<RegisterDto> registerValidator,
        IValidator<UpdateDto> updateValidator)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _mapper = mapper;
        _loginValidator = loginValidator;
        _registerValidator = registerValidator;
        _updateValidator = updateValidator;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        await _loginValidator.ValidateAndThrowAsync(loginDto);
        
        var user = await _userRepository.LoginAsync(_mapper.Map<ApplicationUser>(loginDto));
        var token = _tokenService.GenerateJwtToken(user);
        
        return token;
    }

    public async Task<ICollection<UserDto>> GetUsers()
    {
        var result = await _userRepository.GetAllUsersAsync();

        if (result.Any()) return _mapper.Map<ICollection<UserDto>>(result);

        return null;
    }

    public async Task<UserDto> FindByUserIdASync(string id)
    {
        var result = await _userRepository.FindByUserIdAsync(id);
        
        return _mapper.Map<UserDto>(result);
    }

    public async Task<bool> RegisterUserAsync(RegisterDto registerDto)
    {
        await _registerValidator.ValidateAndThrowAsync(registerDto);
        
        var user = _mapper.Map<ApplicationUser>(registerDto);

        return await _userRepository.AddUserAsync(user, registerDto.Password);
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        return await _userRepository.DeleteUserAsync(userId);
    }

    public async Task<bool> UpdateUserAsync(string userId, UpdateDto updateDto)
    {
        await _updateValidator.ValidateAndThrowAsync(updateDto);
        
        var user = _mapper.Map<ApplicationUser>(updateDto);
        
        return await _userRepository.UpdateUserAsync(userId, user);
    }
}