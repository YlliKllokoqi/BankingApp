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

    public AuthService(
        ITokenService tokenService,
        IUserRepository userRepository,
        IMapper mapper,
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

    public async Task<ResultDto<string>> LoginAsync(LoginDto loginDto)
    {
        var validationResult = await _loginValidator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
        {
            return ResultDto<string>.Failure(new ErrorResponseDto
            {
                Message = "Validation Failed",
                Details = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
            });
        }

        var loginResult = await _userRepository.LoginAsync(_mapper.Map<ApplicationUser>(loginDto));
        if (!loginResult.IsSuccess)
        {
            return ResultDto<string>.Failure(_mapper.Map<ErrorResponseDto>(loginResult.Error));
        }

        var token = await _tokenService.GenerateJwtToken(loginResult.Data);
        return ResultDto<string>.Success(token);
    }

    public async Task<ResultDto<ICollection<UserDto>>> GetUsers()
    {
        var result = await _userRepository.GetAllUsersAsync();
        if (!result.IsSuccess)
        {
            return ResultDto<ICollection<UserDto>>.Failure(_mapper.Map<ErrorResponseDto>(result.Error));
        }

        var usersDto = _mapper.Map<ICollection<UserDto>>(result.Data);
        return ResultDto<ICollection<UserDto>>.Success(usersDto);
    }

    public async Task<ResultDto<UserDto>> FindUserById(string id)
    {
        var result = await _userRepository.FindUserById(id);
        if (!result.IsSuccess)
        {
            return ResultDto<UserDto>.Failure(_mapper.Map<ErrorResponseDto>(result.Error));
        }

        var userDto = _mapper.Map<UserDto>(result.Data);
        return ResultDto<UserDto>.Success(userDto);
    }

    public async Task<ResultDto<bool>> RegisterUserAsync(RegisterDto registerDto)
    {
        var validationResult = await _registerValidator.ValidateAsync(registerDto);
        if (!validationResult.IsValid)
        {
            return ResultDto<bool>.Failure(new ErrorResponseDto
            {
                Message = "Validation Failed",
                Details = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
            });
        }

        var user = _mapper.Map<ApplicationUser>(registerDto);
        var result = await _userRepository.AddUserAsync(user, registerDto.Password);

        if (!result.IsSuccess)
        {
            return ResultDto<bool>.Failure(_mapper.Map<ErrorResponseDto>(result.Error));
        }

        var roleAssignmentResult = await _userRepository.AssignRoleAsync(user, "User");

        if (!roleAssignmentResult.IsSuccess)
        {
            return ResultDto<bool>.Failure(_mapper.Map<ErrorResponseDto>(roleAssignmentResult.Error));
        }

        return ResultDto<bool>.Success(true);
    }

    public async Task<ResultDto<bool>> DeleteUserAsync(string userId)
    {
        var result = await _userRepository.DeleteUserAsync(userId);

        return result.IsSuccess
            ? ResultDto<bool>.Success(true)
            : ResultDto<bool>.Failure(_mapper.Map<ErrorResponseDto>(result.Error));
    }

    public async Task<ResultDto<bool>> UpdateUserAsync(string userId, UpdateDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return ResultDto<bool>.Failure(new ErrorResponseDto
            {
                Message = "Validation Failed",
                Details = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
            });
        }

        var user = _mapper.Map<ApplicationUser>(updateDto);
        var result = await _userRepository.UpdateUserAsync(userId, user);

        return result.IsSuccess
            ? ResultDto<bool>.Success(true)
            : ResultDto<bool>.Failure(_mapper.Map<ErrorResponseDto>(result.Error));
    }

     public async Task<ResultDto<bool>> AssignRole(string userId, string role)
    {
        var assignedUser = await _userRepository.FindUserById(userId);

        if (!assignedUser.IsSuccess)
        {
            return ResultDto<bool>.Failure(_mapper.Map<ErrorResponseDto>(assignedUser.Error));
        }

        var appUser = _mapper.Map<ApplicationUser>(assignedUser.Data);
        var result = await _userRepository.AssignRoleAsync(appUser, role);

        return result.IsSuccess
            ? ResultDto<bool>.Success(true)
            : ResultDto<bool>.Failure(_mapper.Map<ErrorResponseDto>(result.Error));
    }
}
