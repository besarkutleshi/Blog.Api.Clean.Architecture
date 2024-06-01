using Blog.Application.Contracts;
using Blog.Application.Helpers;
using Blog.IntegrationTests.Factories;
using Bogus;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<CustomWebApplicationFactory>
{
    public readonly IServiceScope serviceScope;
    protected readonly UserManager<IdentityUser> _userManager;
    protected readonly RoleManager<IdentityRole> _roleManager;
    protected readonly SignInManager<IdentityUser> _signInManager;
    protected readonly IMapper _mapper;
    protected readonly IRequestService _requestService;
    protected readonly Faker<IdentityUser> _userFaker;
    private readonly string _password = "Password1.";
    protected readonly IAuthTokenFactory _authTokenFactory;

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        serviceScope = factory.Services.CreateScope();

        _userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        _roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        _signInManager = serviceScope.ServiceProvider.GetRequiredService<SignInManager<IdentityUser>>();
        _signInManager.Context = new DefaultHttpContext { RequestServices = serviceScope.ServiceProvider };
        _mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();
        _requestService = serviceScope.ServiceProvider.GetRequiredService<IRequestService>();
        _userFaker = IdentityUserFakerFactory.Create();
        _authTokenFactory = serviceScope.ServiceProvider.GetRequiredService<IAuthTokenFactory>();
    }

    protected async Task<IdentityUser> CreateUser()
    {
        var identityUser = _userFaker.Generate();

        await _userManager.CreateAsync(identityUser, _password);

        return identityUser;
    }
}
