using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace NextHireAPI.Applications.Validations
{
    /// <summary>
    /// Requisição de autorização que permite acesso somente ao próprio usuário
    /// identificado no token (claim NameIdentifier igual ao usuárioId da rota)
    /// ou a um usuário com perfil de Administrador.
    /// </summary>
    public class SameUserRequirement : IAuthorizationRequirement
    {
    }

    /// <summary>
    /// Handler que avalia a SameUserRequirement comparando o ID do usuário
    /// autenticado com o parâmetro de rota "usuarioId".
    /// </summary>
    public class SameUserOrAdmHandler : AuthorizationHandler<SameUserRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SameUserRequirement requirement)
        {
            //? Administrador pode remover qualquer usuário
            if (context.User.IsInRole("Adm"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var idClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //? O dono da conta pode remover a si mesmo
            if (context.Resource is HttpContext httpContext
                && int.TryParse(httpContext.GetRouteValue("usuarioId")?.ToString(), out int usuarioId)
                && int.TryParse(idClaim, out int tokenUserId)
                && tokenUserId == usuarioId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
