using Blog.Application.Contracts;
using Blog.Application.Features.Auth.Dtos.Requests;
using Blog.Application.Features.Auth.Dtos.Responses;
using Blog.Domain.Interfaces;
using Blog.SharedResources;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application.Features.Authentication.Commands;

public class Login
{
    public record LoginCommand(LoginDto LoginDto) : IRequest<Result>;

    public class LoginCommandHandler(IAuthenticationRepository _authRepository, 
        IAuthorizationRepository _authorizationRepository, IAuthTokenFactory _tokenFactory) : IRequestHandler<LoginCommand, Result>
    {
        public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginResult = await _authRepository.Login(request.LoginDto.Email, request.LoginDto.Password, cancellationToken);
            if (loginResult.IsSuccess)
            {
                var identityUser = (IdentityUser)loginResult.Response.Result!;

                var rolesResult = await _authorizationRepository.GetUserRoles(identityUser.Id, cancellationToken);
                if (rolesResult.IsSuccess)
                {
                    var userInfoDto = new UserInfoDto(identityUser.Id, identityUser.Email!, "", (List<string>)rolesResult.Response.Result!);
                    var token = _tokenFactory.GenerateToken(userInfoDto);
                    if (string.IsNullOrEmpty(token))
                        return Result.Failure(new Error("Token.Error", ["Failed to generate auth token."], ErrorType.Failure));

                    userInfoDto.Token = token;
                    return Result.Success(Success.Ok(userInfoDto));
                }

                return rolesResult;
            }

            return loginResult;
        }
    }
}
