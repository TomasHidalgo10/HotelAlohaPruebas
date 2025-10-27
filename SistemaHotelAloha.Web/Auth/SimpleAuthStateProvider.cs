using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace SistemaHotelAloha.Web.Auth
{
    public record SessionUser { public int Id { get; set; } public string Name { get; set; } = ""; }

    public class SimpleAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _storage;
        private readonly ClaimsPrincipal _anon = new(new ClaimsIdentity());

        public SimpleAuthStateProvider(ProtectedSessionStorage storage)
        {
            _storage = storage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var result = await _storage.GetAsync<SessionUser>("auth");
            if (result.Success && result.Value is SessionUser u)
            {
                var id = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
                    new Claim(ClaimTypes.Name, u.Name),
                }, "AlohaAuth");

                return new AuthenticationState(new ClaimsPrincipal(id));
            }

            return new AuthenticationState(_anon);
        }

        public async Task SignInAsync(int id, string name)
        {
            await _storage.SetAsync("auth", new SessionUser { Id = id, Name = name });
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task SignOutAsync()
        {
            await _storage.DeleteAsync("auth");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}